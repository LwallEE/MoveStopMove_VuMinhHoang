using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PoolAmount
{
    public GameUnit prefab;
    public int amount;
}
public class PoolControl : MonoBehaviour
{
    [SerializeField] private PoolAmount[] poolAmounts;

    private void Awake()
    {
        for (int i = 0; i < poolAmounts.Length; i++)
        {
            PoolAmount poolAmount = poolAmounts[i];
            string name = poolAmount.prefab.GetType().ToString();
            GameObject obj = new GameObject(name);
            obj.transform.position = Vector3.zero;
            obj.transform.SetParent(transform);
            
            
        }
    }
}
