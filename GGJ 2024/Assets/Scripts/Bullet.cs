using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Bullet : MonoBehaviour
{
    public float Damage = 1;
    public float BounceDamage = 1;

    [SerializeField]
    private bool _hasCollided = false;

    public void SetDamage(float pDamage, float pBounceDamage)
    {
        Damage = pDamage;
        BounceDamage = pBounceDamage;
    }

    private IEnumerator DespawnTimer(float pDespawnTime)
    {

        yield return new WaitForSeconds(pDespawnTime);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (_hasCollided) { return; }
        _hasCollided = true;

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeImpact(transform.forward, Damage, BounceDamage);
        }

        StartCoroutine(DespawnTimer(1.5f));
    }
}
