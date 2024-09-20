using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ReuseSystem.ObjectPooling
{
    [Serializable]
    struct GameObjectPool
    {
        public GameObject objectToSpawn;
        public int numberToSpawn;
    }

    public class LazyPool : Singleton<LazyPool>
    {
        [SerializeField] private List<GameObjectPool> listGameObjectToPreload;
        private Dictionary<GameObject, List<GameObject>> _poolObj = new Dictionary<GameObject, List<GameObject>>();

        protected override void Awake()
        {
            base.Awake();
            PreLoadObject();
        }

        private void PreLoadObject()
        {
            if (listGameObjectToPreload == null) return;
            foreach (var obj in listGameObjectToPreload)
            {
                if (_poolObj.ContainsKey(obj.objectToSpawn)) continue;
                _poolObj.Add(obj.objectToSpawn, new List<GameObject>());
                for (int i = 0; i < obj.numberToSpawn; i++)
                {
                    GameObject gameObjInstance = Instantiate(obj.objectToSpawn, transform);
                    _poolObj[obj.objectToSpawn].Add(gameObjInstance);
                    gameObjInstance.SetActive(false);
                }
            }
        }

        public GameObject GetObj(GameObject objKey)
        {
            if (!_poolObj.ContainsKey(objKey))
            {
                _poolObj.Add(objKey, new List<GameObject>());
            }

            foreach (GameObject g in _poolObj[objKey])
            {
                if (g.activeSelf)
                    continue;
                g.SetActive(true);
                return g;
            }

            GameObject g2 = Instantiate(objKey, transform);
            _poolObj[objKey].Add(g2);
            return g2;

        }

        public T GetObj<T>(GameObject objKey) where T : Component
        {
            return this.GetObj(objKey).GetComponent<T>();
        }

        public void AddObjectToPool(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(transform);
        }

        public void ReleaseObjectPool(GameObject objectPrefab)
        {
            if (!_poolObj.ContainsKey(objectPrefab))
            {
                return;
            }

            foreach (var obj in _poolObj[objectPrefab])
            {
                Destroy(obj);
            }

            _poolObj[objectPrefab].Clear();
            _poolObj.Remove(objectPrefab);
        }
        
        //release all except preload, preload will be deactive
        public void ReleaseAll()
        {
            try
            {
                var keys = _poolObj.Keys.ToList();
                foreach (var key in keys)
                {
                    if (listGameObjectToPreload.Any(x => x.objectToSpawn == key))
                    {
                        DeActiveAll(key);
                        continue;
                    };
                    var objectInstance = _poolObj[key];

                    foreach (var instance in objectInstance)
                    {
                        Destroy(instance);
                    }

                    objectInstance.Clear();
                    _poolObj.Remove(key);
                }

            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
            
            
        }
        
        private void DeActiveAll(GameObject key)
        {
            var objectInstance = _poolObj[key];

            foreach (var instance in objectInstance)
            {
                instance.SetActive(false);
            }
        }
    }
  
}