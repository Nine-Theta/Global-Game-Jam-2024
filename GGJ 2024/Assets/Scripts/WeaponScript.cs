using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class Weapon : MonoBehaviour, I_Interactable
{
    public bool IsWielded = false;

    public float Damage = 1;
    public float BounceDamage = 1;
    public float YeetDamageMult = 2;

    public float AttackSpeed = 10f;

    public float AttackAngleSize = 90f;

    [SerializeField]
    private Transform _impactPoint;

    [SerializeField]
    private Player _wielder = null;

    [SerializeField]
    private GameObject _weaponObject;

    private float _startAngle;

    private bool _isAttacking = false;
    private bool _isYeeted = false;


    [SerializeField]
    private float _angleTracker = 0;

    private Rigidbody _body;

    [SerializeField]
    private SphereCollider _interactCollider;

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
        _interactCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if (IsWielded)
        {
            this.transform.position = _wielder.transform.position;
        }

        if (_isAttacking)
        {
            if (_angleTracker <= 0)
            {
                _weaponObject.SetActive(false);
                _isAttacking = false;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, _startAngle, transform.rotation.eulerAngles.z);
                return;
            }
            transform.Rotate(Vector3.up, -AttackSpeed * Time.deltaTime);
            _angleTracker -= AttackSpeed * Time.deltaTime;
        }

        Debug.DrawRay(_impactPoint.position, _impactPoint.forward, Color.red, 0.5f);
    }

    public void Interact(Player pPlayer)
    {
        Equip(pPlayer);
    }

    public void Attack()
    {
        if (_isAttacking)
            return;

        _weaponObject.SetActive(true);
        _startAngle = transform.rotation.eulerAngles.y;
        _isAttacking = true;
        _angleTracker = AttackAngleSize;
    }

    public void Equip(Player pWielder)
    {
        _wielder = pWielder;
        IsWielded = true;

        pWielder.EquipWeapon(this);

        _isYeeted = false;

        _body.isKinematic = true;
        _body.useGravity = false;

        _interactCollider.enabled = false;
    }

    public void Yeet(Vector3 pDirection, Vector3 pRelativeVelocity)
    {
        IsWielded = false;
        _wielder = null;

        _isYeeted = true;
        _isAttacking = false;

        _weaponObject.SetActive(true);

        transform.LookAt(pDirection, Vector3.up);

        Rigidbody body = GetComponent<Rigidbody>();

        _body.isKinematic = false;
        _body.useGravity = true;

        _body.AddForce(pDirection * 25f, ForceMode.VelocityChange);
        _body.AddForce(pRelativeVelocity, ForceMode.VelocityChange);
        _body.AddTorque(0, -20f, 0, ForceMode.VelocityChange);

        _interactCollider.enabled = true;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (IsWielded && collision.gameObject.tag == "Player")
        {
            Debug.Log("Collided with player!");

            if (_isYeeted)
            {
                collision.gameObject.GetComponent<Player>().TakeImpact(transform.forward*YeetDamageMult, Damage*YeetDamageMult, BounceDamage*YeetDamageMult);
            }
            else
            {
                collision.gameObject.GetComponent<Player>().TakeImpact(_impactPoint.forward, Damage, BounceDamage);
            }
        }
        else if(collision.gameObject.tag == "Ground")
        {
            _isYeeted = false;
        }
    }
}
