using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private int totalNumberOfBot;
    [SerializeField] private int maxNumberOfBotAtSameTime;
    [SerializeField] private Transform upperRightTrans;
    [SerializeField] private Transform lowerLeftTrans;
    [SerializeField] private Vector3 playerPosition;
    
    public Vector3 GetRandomPosInMap()
    {
        return new Vector3(Random.Range(lowerLeftTrans.position.x, upperRightTrans.position.x), 0,
            Random.Range(lowerLeftTrans.position.z, upperRightTrans.position.z));
    }

    public int GetTotalNumberBot()
    {
        return totalNumberOfBot;
    }

    public int GetTotalNumberOfPlayer()
    {
        return totalNumberOfBot + 1;
    }
    public int GetMaxNumberOfBotAtSameTime()
    {
        return maxNumberOfBotAtSameTime;
    }

    public Vector3 GetPlayerPosition()
    {
        return playerPosition;
    }
}
