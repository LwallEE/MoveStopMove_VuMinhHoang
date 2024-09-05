using System.Collections;
using System.Collections.Generic;
using ReuseSystem.ObjectPooling;
using UnityEngine;

public class AxeRotateWeapon : Weapon
{
   private bool isTouchGround;
   [SerializeField] private float speedFall;
   public override void Init(Vector3 direction, float range, Vector3 firePoint, Character sender,float scale)
   {
      base.Init(direction, range, firePoint, sender,scale);
      isTouchGround = false;
   }

   protected override void FixedUpdate()
   {
      if (isRotate && !isTouchGround)
      {
         Rotate();
      }
   }

   protected override void Rotate()
   {
     
      transform.Rotate(Time.deltaTime*speedRotate,0, 0);
   }

   protected override void WeaponBehaviour()
   {
      if (isTouchGround) return;
      transform.position += this.direction * speed * Time.deltaTime + Vector3.down*speedFall*Time.deltaTime;
   }

   protected override void OnTriggerEnter(Collider other)
   {
      base.OnTriggerEnter(other);
      if (other.CompareTag("Ground"))
      {
         isTouchGround = true;
        FixRotation();
      }
      if (other.CompareTag(Constants.CHARACTER_TAG) && other.transform == sender.transform)
      {
         LazyPool.Instance.AddObjectToPool(gameObject);
      }
   }

   private void FixRotation()
   {
      if (transform.eulerAngles.z > 100f)
      {
         transform.eulerAngles = new Vector3(-150f, transform.eulerAngles.y, transform.eulerAngles.z);
      }
      else
      {
         transform.eulerAngles = new Vector3(-30f, transform.eulerAngles.y, transform.eulerAngles.z);

      }
      
   }
}
