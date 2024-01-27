using UnityEngine;

public class RangedWeapon : Weapon
{
    [SerializeField]
    private GameObject _weaponColliderObject;

    private void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (IsWielded)
        {
            this.transform.position = Wielder.transform.position;


            if (!IsAttacking)
            {
                if (Quaternion.Angle(this.transform.rotation, Wielder.transform.rotation * Quaternion.AngleAxis(HoldingAngle, Vector3.up)) > 3)
                {
                    transform.rotation = Quaternion.Lerp(this.transform.rotation, Wielder.transform.rotation * Quaternion.AngleAxis(HoldingAngle, Vector3.up), Time.deltaTime * 5);
                }
                else
                {
                    transform.rotation = Wielder.transform.rotation * Quaternion.AngleAxis(HoldingAngle, Vector3.up);
                    ReadyToAttack = true;
                }

            }
            else if (WeaponBody.angularVelocity.sqrMagnitude <= 5f && WeaponBody.angularVelocity.sqrMagnitude > 0)
            {
                _weaponColliderObject.SetActive(false);
                IsAttacking = false;
                WeaponBody.angularVelocity = Vector3.zero;
            }
        }

        Debug.DrawRay(transform.position, transform.forward, Color.red, 0.5f);
    }

    public override void Attack()
    {
        if (IsAttacking || !ReadyToAttack)
            return;

        IsAttacking = true;
        ReadyToAttack = false;
    }

    public override void Equip(Player pWielder)
    {
        _weaponColliderObject.SetActive(false);

        base.Equip(pWielder);
    }

    public override void Yeet(Vector3 pDirection, Vector3 pRelativeVelocity)
    {
        _weaponColliderObject.SetActive(true);
        transform.position = Wielder.transform.position;

        base.Yeet(pDirection, pRelativeVelocity);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (IsYeeted)
            {
                Player colPlayer = collision.gameObject.GetComponent<Player>();
                colPlayer.TakeImpact(transform.forward * YeetDamageMult, Damage * YeetDamageMult, BounceDamage * YeetDamageMult);
                IsYeeted = false;
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            IsYeeted = false;
        }
    }
}
