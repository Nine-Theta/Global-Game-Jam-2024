using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    [SerializeField]
    private float _bulletSpeed = 10f;

    [SerializeField]
    private GameObject _weaponColliderObject;

    [SerializeField]
    private Transform _bulletSpawnPoint;

    [SerializeField]
    private GameObject _bullet;

    [SerializeField]
    private Animator _animator;

    private float _fireWatch = 0;

    private void Start()
    {
        _animator.speed = AttackSpeed;

        Debug.Log("called");

        base.Start();
    }

    private void Update()
    {
        if (IsWielded)
        {
            this.transform.position = Wielder.transform.position;
            transform.rotation = Quaternion.Lerp(this.transform.rotation, Wielder.transform.rotation * Quaternion.AngleAxis(HoldingAngle, Vector3.up), Time.deltaTime * 5);

            if (IsAttacking && _fireWatch <= 0)
            {
                _fireWatch = (1 / AttackSpeed);

                SpawnBullet();
            }
            else
            {
                _fireWatch -= Time.deltaTime;
            }
        }
    }

    private void SpawnBullet()
    {
        GameObject bullet = Instantiate(_bullet, _bulletSpawnPoint.position, this.transform.rotation);
        bullet.GetComponent<Bullet>().SetDamage(Damage, BounceDamage);
        bullet.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, _bulletSpeed), ForceMode.VelocityChange);
    }

    public override void Attack()
    {
        if (IsAttacking || !ReadyToAttack)
            return;

        _animator.SetBool("GunIsFiring", true);

        IsAttacking = true;
        ReadyToAttack = false;
    }

    public override void StopAttack()
    {
        _animator.SetBool("GunIsFiring", false);
        base.StopAttack();
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
