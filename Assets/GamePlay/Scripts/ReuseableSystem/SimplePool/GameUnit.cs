using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit : MonoBehaviour
{

    private Transform tf;

    public Transform Tf
    {
        get
        {
            if (tf == null)
            {
                tf = transform;
            }

            return tf;
        }
    }

    private GameObject prefabKey;

    public GameObject GetPrefabKey()
    {
        return prefabKey;
    }

    public void SetPrefabKey(GameObject gameOB)
    {
        prefabKey = gameOB;
    }
}
