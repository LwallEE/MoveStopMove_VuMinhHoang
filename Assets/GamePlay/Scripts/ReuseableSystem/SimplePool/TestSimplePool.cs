using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestSimplePool : MonoBehaviour
{
   [SerializeField] private GameUnit prefab1;
   [SerializeField] private GameUnit prefab2;

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.J))
      {
         SimplePool.Instance.Spawn<GameUnit>(prefab1, GetRandom(), Quaternion.identity);
      }

      if (Input.GetKeyDown(KeyCode.K))
      {
         SimplePool.Instance.Spawn<DisableAfterFewSecond>(prefab2, GetRandom(), Quaternion.identity);
      }

      if (Input.GetKeyDown(KeyCode.C))
      {
         SimplePool.Instance.Collect(prefab1);
         SimplePool.Instance.Collect(prefab2);
      }

      if (Input.GetKeyDown(KeyCode.D))
      {
         SimplePool.Instance.Release(prefab1);
         SimplePool.Instance.Release(prefab2);
      }
   }

   private Vector3 GetRandom()
   {
      return new Vector3(Random.Range(-3f, 3f),
         Random.Range(-3f, 3f),
         Random.Range(-3f, 3f));
   }
}
