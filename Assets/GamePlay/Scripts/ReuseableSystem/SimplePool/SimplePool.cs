using System;
using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using UnityEngine;

public class SimplePool : Singleton<SimplePool>
{
    private Dictionary<GameObject, Pool> poolInstance = new Dictionary<GameObject, Pool>();
    [SerializeField] private PoolAmount[] poolAmounts;

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < poolAmounts.Length; i++)
        {
            Preload(poolAmounts[i].prefab, poolAmounts[i].amount);
        }
    }

    public void Preload(GameUnit prefab, int amount, Transform parent=null)
    {
        if (prefab == null)
        {
            Debug.LogError(" PREFAB IN POOL IS EMPTY");
            return;
        }



        var key = prefab.gameObject;
        if (!poolInstance.ContainsKey(key) || poolInstance[key] == null)
        {
            if (parent == null)
            {
                parent = new GameObject(prefab.gameObject.name).transform;
                parent.transform.position = Vector3.zero;
                parent.SetParent(transform);
            }
            Pool p = new Pool();
            p.PreLoad(prefab, amount, parent);
            poolInstance[key] = p;
        }
        
    }

    public T Spawn<T>(GameUnit unit, Vector3 pos = default, Quaternion rot = default) where T: GameUnit
    {
        var key = unit.gameObject;
        if (!poolInstance.ContainsKey(key))
        {
            Preload(unit,1);
        }

        return poolInstance[key].Spawn(pos, rot) as T;
    }

    //Take the object to pool
    public void Despawn(GameUnit unit)
    {
        var key = unit.GetPrefabKey();
        if (key == null || !poolInstance.ContainsKey(key))
        {
            Debug.LogError(key.name + " IS NOT PRELOAD");
        }
        poolInstance[key].Despawn(unit);
    }

    //Disable all object of type GameUnit
    public void Collect(GameUnit unit)
    {
        var key = unit.gameObject;
        if (!poolInstance.ContainsKey(key))
        {
            Debug.LogError(key.name + "IS NOT PRELOAD !!!");
            return;
        }
        poolInstance[key].Collect();
    }

    //Disable all pool
    public void CollectAll()
    {
        foreach (var item in poolInstance.Values)
        {
            item.Collect();
        }
    }

    //Destroy all object of type GameUnit
    public void Release(GameUnit unit)
    {
        var key = unit.gameObject;
        if (!poolInstance.ContainsKey(key))
        {
            //Debug.LogError(key.name + "IS NOT PRELOAD !!!");
            return;
        }
        poolInstance[key].Release();
    }

    public void ReleaseAll()
    {
        foreach (var item in poolInstance.Values)
        {
            item.Release();
        }
    }
    
}

public class Pool
{
    private Transform parent;
    private GameUnit prefab;
    //list contain unit is not using
    private Queue<GameUnit> inactives = new Queue<GameUnit>();
    //list contain unit is using
    private List<GameUnit> actives = new List<GameUnit>();

    //init pool
    public void PreLoad(GameUnit prefab, int amount, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;

        Debug.Log(amount);
        for (int i = 0; i < amount; i++)
        {
            Spawn(Vector3.zero, Quaternion.identity);
        }
        Collect();
    }

    //get element from pool
    public GameUnit Spawn(Vector3 pos, Quaternion rot )
    {
        GameUnit unit;
        if (inactives.Count <= 0)
        {
            unit = GameObject.Instantiate(prefab, parent);
        }
        else
        {
            unit = inactives.Dequeue();
        }
        unit.Tf.SetLocalPositionAndRotation(pos, rot);
        unit.SetPrefabKey(prefab.gameObject);
        unit.gameObject.SetActive(true);
        actives.Add(unit);
        return unit;
    }
    
    
    //return element to pool
    public void Despawn(GameUnit unit)
    {
        if (unit != null && unit.gameObject.activeSelf)
        {
            actives.Remove(unit);
            inactives.Enqueue(unit);
            unit.gameObject.SetActive(false);
            unit.Tf.SetParent(parent);
        }
    }

    //return all used element to pool
    public void Collect()
    {
        while (actives.Count > 0)
        {
            Despawn(actives[0]);
        }
    }

    //destroy all element in pool
    public void Release()
    {
        Collect();
        while(inactives.Count > 0)
        {
            GameObject.Destroy(inactives.Dequeue().gameObject);
        }
        inactives.Clear();
    }
}