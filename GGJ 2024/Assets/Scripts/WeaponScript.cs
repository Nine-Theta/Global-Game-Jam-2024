using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Weapon : MonoBehaviour
{
    public bool IsWielded = false;

    public float Damage = 1;
    public float BounceDamage = 1;

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
    [SerializeField]
    private float _angleTracker = 0;

    private void Update()
    {
        if (IsWielded)
        {
            this.transform.position = _wielder.transform.position;
        }

        if (_isAttacking)
        {
            if(_angleTracker <= 0)
            {
                _weaponObject.SetActive(false);
                _isAttacking= false;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, _startAngle, transform.rotation.eulerAngles.z);
                return;
            }    
            transform.Rotate(Vector3.up, -AttackSpeed*Time.deltaTime);
            _angleTracker -= AttackSpeed * Time.deltaTime;
        }

        Debug.DrawRay(_impactPoint.position, _impactPoint.forward, Color.red, 0.5f);
    }

    public void Attack()
    {
        if (_isAttacking)
            return;

        _weaponObject.SetActive(true);
        _startAngle = transform.rotation.eulerAngles.y;
        _isAttacking= true;
        _angleTracker = AttackAngleSize;
    }

    public void Equip(Player pWielder)
    {
        _wielder = pWielder;
        IsWielded= true;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(IsWielded && collision.gameObject.tag == "Player")
        {
            Debug.Log("Collided with player!");
            collision.gameObject.GetComponent<Player>().TakeImpact(collision.GetContact(0).point,_impactPoint.forward, Damage, BounceDamage);
        }
    }
}
