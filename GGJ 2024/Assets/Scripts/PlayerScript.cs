using UnityEngine;

//[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    private Rigidbody _body;

    private float _bounceMult = 5;

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

    public void TakeImpact(Vector3 pPoint, Vector3 pDirection, float pDamage, float pBounceDamage)
    {
        Debug.Log("Took Impact!");

        Health -= pDamage;
        BouncePercent += pBounceDamage*0.1f;

        _body.AddForce(pDirection * BouncePercent*_bounceMult, ForceMode.VelocityChange);

        Debug.DrawRay(transform.position, pDirection * BouncePercent,Color.magenta, 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            Weapon weapon = collision.gameObject.GetComponent<Weapon>();

            if (weapon.IsWielded)
            {

            }
        }
    }
}
