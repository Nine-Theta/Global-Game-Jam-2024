using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class Weapon : MonoBehaviour, I_Interactable
{
    public bool IsWielded = false;

    public float Damage = 1;
    public float BounceDamage = 1;
    public float YeetDamageMult = 2;

    public float AttackSpeed = 10f;

    public float HoldingAngle = 70f;

    private bool _readyToAttack = true;

    [SerializeField]
    private Transform _impactPoint;

    [SerializeField]
    private Player _wielder = null;

    [SerializeField]
    private GameObject _weaponColliderObject;


    [SerializeField, ReadOnly]
    private bool _isAttacking = false;
    [SerializeField, ReadOnly]
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


            if (!_isAttacking)
            {
                if (Quaternion.Angle(this.transform.rotation, _wielder.transform.rotation * Quaternion.AngleAxis(HoldingAngle, Vector3.up)) > 3)
                {
                    transform.rotation = Quaternion.Lerp(this.transform.rotation, _wielder.transform.rotation * Quaternion.AngleAxis(HoldingAngle, Vector3.up), Time.deltaTime * 5);
                }
                else
                {
                    transform.rotation = _wielder.transform.rotation * Quaternion.AngleAxis(HoldingAngle, Vector3.up);
                    _readyToAttack = true;
                }

            }
            else if (_body.angularVelocity.sqrMagnitude <= 1f && _body.angularVelocity.sqrMagnitude >0)
            {
                Debug.Log("angle vel low: " + _body.angularVelocity.sqrMagnitude);
                _weaponColliderObject.SetActive(false);
                _isAttacking = false;
                _body.angularVelocity = Vector3.zero;
            }
        }

        Debug.DrawRay(_impactPoint.position, _impactPoint.forward, Color.red, 0.5f);
        Debug.Log("anglevel: " + _body.angularVelocity.sqrMagnitude);
    }

    public void Interact(Player pPlayer)
    {
        Equip(pPlayer);
    }

    public void Attack()
    {
        if (_isAttacking || !_readyToAttack)
            return;

        Debug.Log("start attack");

        _weaponColliderObject.SetActive(true);
        _isAttacking = true;
        _readyToAttack = false;

        _body.AddTorque(new Vector3(0, -AttackSpeed, 0), ForceMode.VelocityChange);        
    }

    public void Equip(Player pWielder)
    {
        _wielder = pWielder;
        IsWielded = true;

        pWielder.EquipWeapon(this);

        _isYeeted = false;
        _readyToAttack = true;
        _isAttacking = false;

        _body.useGravity = false;

        _body.velocity = Vector3.zero;
        _body.angularVelocity = Vector3.zero;

        _body.isKinematic = true;

        transform.rotation = Quaternion.Euler(0, 70, 0);

        _interactCollider.enabled = false;
        _weaponColliderObject.SetActive(false);
    }

    public void Yeet(Vector3 pDirection, Vector3 pRelativeVelocity)
    {
        IsWielded = false;
        _wielder = null;

        _isYeeted = true;
        _isAttacking = false;

        _weaponColliderObject.SetActive(true);

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
        if (IsWielded && collision.gameObject.CompareTag("Player"))
        {
            Player colPlayer = collision.gameObject.GetComponent<Player>();

            if (colPlayer == _wielder)
                return;

            Debug.Log("Collided with player!");

            if (_isYeeted)
            {
                colPlayer.TakeImpact(transform.forward * YeetDamageMult, Damage * YeetDamageMult, BounceDamage * YeetDamageMult);
            }
            else
            {
                colPlayer.TakeImpact(_impactPoint.forward, Damage, BounceDamage);
            }
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            _isYeeted = false;
        }
    }
}
