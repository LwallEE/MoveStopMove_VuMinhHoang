using System;
using System.Collections;
using System.Collections.Generic;
using ReuseSystem.ObjectPooling;
using UnityEngine;

public class Weapon : MonoBehaviour
{
   [SerializeField] protected float speed;
   [SerializeField] protected bool isRotate;
   [SerializeField] protected float speedRotate = 500f;
   [SerializeField] protected bool isAlwaysCanThrow;
   protected Vector3 direction;
   protected float rangeToDisappear;
   protected Vector3 previousPos;
   protected Character sender;
   public virtual void Init(Vector3 direction, float range, Vector3 firePoint,Character sender,float scale)
   {
      this.direction = direction;
      this.rangeToDisappear = range;
      transform.position = firePoint;
      transform.forward = this.direction;
      previousPos = firePoint;
      this.sender = sender;
      transform.localScale = Vector3.one * scale;
   }

   private void Update()
   {
     WeaponBehaviour();
   }

   protected virtual void FixedUpdate()
   {
      if (isRotate)
      {
         Rotate();
      }
   }

   public virtual bool CanThrowNewWeapon()
   {
      if (isAlwaysCanThrow) return true;
      return !gameObject.activeSelf;
   }

   protected virtual void WeaponBehaviour()
   {
      transform.position += this.direction * speed * Time.deltaTime;
      CheckDisappear();
   }

   public float GetSpeed()
   {
      return speed;
   }

   protected virtual void Rotate()
   {
      transform.Rotate(0,Time.deltaTime*speedRotate, 0);
   }
   protected virtual void CheckDisappear()
   {
      if (Vector3.Distance(previousPos, transform.position) > rangeToDisappear)
      {
         LazyPool.Instance.AddObjectToPool(gameObject);
      }
   }

   protected virtual void DestroyWeapon()
   {
      LazyPool.Instance.AddObjectToPool(gameObject);
   }

   protected virtual void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag(Constants.CHARACTER_TAG) && other.transform != sender.transform)
      {
         var character = other.GetComponent<Character>();
         character.SetKillerName(sender.Name);
         character.OnDeath();
         sender.LevelUp();
         DestroyWeapon();
      }

      if (other.CompareTag("Obstacle"))
      {
         DestroyWeapon();
      }
   }
}
