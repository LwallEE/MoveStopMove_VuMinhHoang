using System;
using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using ReuseSystem.ObjectPooling;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private int numberOfTimeTry;
    [SerializeField] private float radiusCheck;

    private Map currentMap;
    private int currentNumberBotInMap;
    private int deadCounter;
    

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
        
        var pos = currentMap.GetRandomPosInMap();
        currentNumberBotInMap++;
        var bot = LazyPool.Instance.GetObj<Character>(GameAssets.Instance.botPrefab);
        bot.transform.position = pos;
        bot.OnInit();
    }
    private bool TrySpawnBot()
    {
        for (int i = 0; i < numberOfTimeTry; i++)
        {
            var pos = currentMap.GetRandomPosInMap();
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
        if (currentNumberBotInMap < currentMap.GetTotalNumberBot() && GameController.Instance.IsInState(GameState.GamePlay))
        {
            SpawnBot();
        }
        Debug.Log($"Number of bot remain {currentMap.GetTotalNumberBot()-deadCounter}".AddColor(Color.yellow));
        UIManager.Instance.GetUI<GamePlayCanvas>().UpdateAliveText(currentMap.GetTotalNumberOfPlayer() - deadCounter);
        if (deadCounter >= currentMap.GetTotalNumberBot())
        {
            GameController.Instance.ChangeGameState(GameState.GameWin);
        }
    }

    public int GetNumberDeadBot()
    {
        return deadCounter;
    }
    
    public void LoadMap(Map map)
    {
        this.currentMap = map;
        UIManager.Instance.GetUI<GamePlayCanvas>().UpdateAliveText(currentMap.GetTotalNumberBot());
        currentNumberBotInMap = 0;
        deadCounter = 0;
        for (int i = 0; i < currentMap.GetMaxNumberOfBotAtSameTime(); i++)
        {
            SpawnBot();
        }
    }
    
}
