using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider)), SelectionBase]
public abstract class Weapon : MonoBehaviour, I_Interactable
{
    public bool IsWielded = false;
    public Player Wielder = null;

    public float Damage = 1;
    public float BounceDamage = 1;
    public float YeetDamageMult = 2;

    public float AttackSpeed = 10f;

    public float HoldingAngle = 70f;

    public float YeetForce = 25f;
    public float YeetTorque = -20f;

    public bool ReadyToAttack = true;
    public bool IsAttacking = false;
    public bool IsYeeted = false;

    public Rigidbody WeaponBody;

    public SphereCollider InteractCollider;

    protected void Start()
    {
        WeaponBody = GetComponent<Rigidbody>();
        InteractCollider = GetComponent<SphereCollider>();
    }

    public void Interact(Player pPlayer)
    {
        Equip(pPlayer);
    }

    public abstract void Attack();

    public virtual void StopAttack()
    {
        IsAttacking = false;
    }

    public virtual void Equip(Player pWielder)
    {
        if (IsWielded)
            return;

        Wielder = pWielder;
        IsWielded = true;

        pWielder.EquipWeapon(this);

        IsYeeted = false;
        ReadyToAttack = true;
        IsAttacking = false;

        WeaponBody.useGravity = false;

        WeaponBody.velocity = Vector3.zero;
        WeaponBody.angularVelocity = Vector3.zero;

        transform.rotation = Quaternion.Euler(0, HoldingAngle, 0);

        InteractCollider.enabled = false;
    }

    public virtual void Yeet(Vector3 pDirection, Vector3 pRelativeVelocity)
    {
        IsWielded = false;
        Wielder = null;

        IsYeeted = true;
        IsAttacking = false;

        transform.LookAt(transform.position + pDirection, Vector3.up);

        WeaponBody.isKinematic = false;
        WeaponBody.useGravity = true;

        WeaponBody.AddForce(pDirection * YeetForce, ForceMode.VelocityChange);
        WeaponBody.AddForce(pRelativeVelocity, ForceMode.VelocityChange);
        WeaponBody.AddTorque(0, YeetTorque, 0, ForceMode.VelocityChange);

        InteractCollider.enabled = true;
    }
}
