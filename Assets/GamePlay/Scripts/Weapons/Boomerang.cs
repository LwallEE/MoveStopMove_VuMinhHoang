using System.Collections;
using System.Collections.Generic;
using ReuseSystem.ObjectPooling;
using UnityEngine;

public class Boomerang : Weapon
{
   [SerializeField] private float speedGoingBack;
   [SerializeField] private float accelerationGoingBack;
   private bool isGoingBack;
   private float currentSpeedGoingBack;

   public override void Init(Vector3 direction, float range, Vector3 firePoint, Character sender, float scale)
   {
      base.Init(direction, range, firePoint, sender, scale);
      isGoingBack = false;
      currentSpeedGoingBack = speedGoingBack;
   }

   protected override void WeaponBehaviour()
   {
      if (!isGoingBack)
      {
         transform.position += this.direction * speed * Time.deltaTime;
         if (Vector3.Distance(previousPos, transform.position) > rangeToDisappear)
         {
            isGoingBack = true;
         }

         return;
      }

      var position = transform.position;
      var targetPosition = sender.transform.position;
      position = Vector3.Lerp(position,
         new Vector3(targetPosition.x, position.y, targetPosition.z),
         currentSpeedGoingBack * Time.deltaTime);
      transform.position = position;
      currentSpeedGoingBack += Time.deltaTime * accelerationGoingBack;
   }

   protected override void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag(Constants.CHARACTER_TAG) && other.transform != sender.transform)
      {
         Cache.GenCharacters(other).OnDeath();
         sender.LevelUp();
      }

      if (other.CompareTag(Constants.CHARACTER_TAG) && isGoingBack && other.transform == sender.transform)
      {
         DestroyWeapon();
      }
   }
}
