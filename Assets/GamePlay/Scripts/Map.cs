using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using UnityEngine;

public class Map : Singleton<Map>
{
    [SerializeField] private Transform upperRightTrans;
    [SerializeField] private Transform lowerLeftTrans;

    public Vector3 GetRandomPosInMap()
    {
        return new Vector3(Random.Range(lowerLeftTrans.position.x, upperRightTrans.position.x), 0,
            Random.Range(lowerLeftTrans.position.z, upperRightTrans.position.z));
    }
}
