using System;
using System.Collections;
using System.Collections.Generic;
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

        private void Start()
        {
            PreLoadObject();
        }

        private void PreLoadObject()
        {
            if (listGameObjectToPreload == null) return;
            foreach (var obj in listGameObjectToPreload)
            {
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

        public void ReleaseAll()
        {
            _poolObj.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}