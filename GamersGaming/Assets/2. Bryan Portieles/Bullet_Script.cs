using UnityEngine;
using System.Collections;

public class Bullet_Script : MonoBehaviour
{
   [SerializeField] public float bulletSpeed;
   [SerializeField] public float damage;

   private void OnCollisionEnter(Collision collision)
   {
      if (collision.gameObject.tag != "Weapon")
      {
         collision.gameObject.SendMessage("ApplyDamage", damage);
         Destroy(gameObject);
      }
   }
   public void Fire()
   {
      var rb = GetComponent<Rigidbody>();
      rb.velocity = transform.forward * bulletSpeed;
      transform.parent = null;
   }
    

    
}
