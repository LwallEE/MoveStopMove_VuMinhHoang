using System;
using System.Collections;
using System.Collections.Generic;
using ReuseSystem.ObjectPooling;
using UnityEngine;

public class Weapon : MonoBehaviour
{
   [SerializeField] private float speed;
   private Vector3 direction;
   private float rangeToDisappear;
   private Vector3 previousPos;
   private Transform sender;
   public void Init(Vector3 direction, float range, Vector3 firePoint,Transform sender)
   {
      this.direction = direction;
      this.rangeToDisappear = range;
      transform.position = firePoint;
      transform.forward = this.direction;
      previousPos = firePoint;
      this.sender = sender;
   }

   private void Update()
   {
      transform.position += this.direction * speed * Time.deltaTime;
      CheckDisappear();
   }

   protected virtual void CheckDisappear()
   {
      if (Vector3.Distance(previousPos, transform.position) > rangeToDisappear)
      {
         LazyPool.Instance.AddObjectToPool(gameObject);
      }
   }

   protected void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag(Constants.CHARACTER_TAG) && other.transform != sender)
      {
         other.GetComponent<Character>().OnDeath();
         LazyPool.Instance.AddObjectToPool(gameObject);
      }
   }
}
