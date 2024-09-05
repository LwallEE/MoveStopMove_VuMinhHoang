using System;
using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using ReuseSystem.ObjectPooling;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private int totalNumberOfBot;
    [SerializeField] private int maxNumberOfBotAtSameTime;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private int numberOfTimeTry;
    [SerializeField] private float radiusCheck;

    private int currentNumberBotInMap;
    private int deadCounter;
    private void Start()
    {
        LoadMap();
    }

    private void SpawnBot()
    {
        StartCoroutine(SpawnBotCoroutine());
    }
    private IEnumerator SpawnBotCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(.5f);
        bool isSpawnSuccess = false;
        for (int i = 0; i < 10; i++)
        {
            if (TrySpawnBot())
            {
                isSpawnSuccess = true;
                break;
            }

            yield return wait;
        }
        if(isSpawnSuccess) yield break;
        
        var pos = Map.Instance.GetRandomPosInMap();
        currentNumberBotInMap++;
        var bot = LazyPool.Instance.GetObj<Character>(GameAssets.Instance.botPrefab);
        bot.transform.position = pos;
        bot.OnInit();
    }
    private bool TrySpawnBot()
    {
        for (int i = 0; i < numberOfTimeTry; i++)
        {
            var pos = Map.Instance.GetRandomPosInMap();
            if (!Physics.CheckSphere(pos, radiusCheck, obstacleLayer))
            {
                currentNumberBotInMap++;
                var bot = LazyPool.Instance.GetObj<Character>(GameAssets.Instance.botPrefab);
                bot.transform.position = pos;
                bot.OnInit();
                return true;
            }
        }

        return false;
    }

    //Spawn another bot if one bot die
    public void CallbackOnBotDie()
    {
        deadCounter++;
        if (currentNumberBotInMap < totalNumberOfBot)
        {
            SpawnBot();
        }
        Debug.Log($"Number of bot remain {totalNumberOfBot-deadCounter}".AddColor(Color.yellow));
    }

    public int GetNumberDeadBot()
    {
        return deadCounter;
    }
    public void LoadMap()
    {
        currentNumberBotInMap = 0;
        deadCounter = 0;
        for (int i = 0; i < maxNumberOfBotAtSameTime; i++)
        {
            SpawnBot();
        }
    }
    
}
