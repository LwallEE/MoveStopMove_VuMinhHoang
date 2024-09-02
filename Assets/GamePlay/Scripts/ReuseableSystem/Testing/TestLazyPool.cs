using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using ReuseSystem.ObjectPooling;
using UnityEngine;

public class TestLazyPool : MonoBehaviour
{
    public GameObject prefab;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LazyPool.Instance.GetObj(prefab);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            LazyPool.Instance.ReleaseObjectPool(prefab);
        }
    }
}
