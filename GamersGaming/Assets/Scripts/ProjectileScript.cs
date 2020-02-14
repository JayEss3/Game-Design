using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _projectileSpeed;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Weapon" || collision.gameObject.tag != "Player")
        {
            collision.gameObject.SendMessage("ApplyDamage", _damage);
            Destroy(gameObject);
        }        
    }
    public void Fire()
    {
        var rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * _projectileSpeed;
        transform.parent = null;
    }
}
