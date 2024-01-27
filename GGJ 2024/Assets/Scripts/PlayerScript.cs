using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
    private Rigidbody _body;

    private float _bounceMult = .75f;

    public float MaxHealth = 100;

    public float Health = 100;

    public float BouncePercent = 0.00f;

    public Weapon WieldedWeapon = null;

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void Update()
    {

    }

    public void Attack()
    {
        if (WieldedWeapon != null)
            WieldedWeapon.Attack();
    }

    public void Interact()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f, (1 << LayerMask.NameToLayer("Interactable")), QueryTriggerInteraction.Collide);
        foreach (Collider collider in colliders)
        {
            Debug.Log($"{collider.gameObject.name} is nearby");
            if (collider.gameObject.GetComponent<I_Interactable>() != null)
            {
                if (collider.gameObject.GetComponent<Weapon>() != null && WieldedWeapon != null)
                    Yeet();

                collider.gameObject.GetComponent<I_Interactable>().Interact(this);
                break;
            }
        }
    }

    public void Yeet()
    {
        if (WieldedWeapon != null)
        {
            WieldedWeapon.Yeet(transform.forward, new Vector3(_body.velocity.x, 2, _body.velocity.z));
            WieldedWeapon = null;
        }
    }

    public void EquipWeapon(Weapon pWeapon)
    {
        WieldedWeapon = pWeapon;
    }

    public void TakeImpact(Vector3 pDirection, float pDamage, float pBounceDamage)
    {
        Health -= pDamage;
        BouncePercent += pBounceDamage * 1;

        _body.AddForce(BouncePercent * _bounceMult * pDirection, ForceMode.VelocityChange);

        Debug.DrawRay(transform.position, pDirection * BouncePercent, Color.magenta, 10f);

        Debug.Log("Force Taken: " + BouncePercent * _bounceMult);
    }

    private void OnCollisionEnter(Collision collision)
    {

    }
}
