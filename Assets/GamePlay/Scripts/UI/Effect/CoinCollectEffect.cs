using System;
using System.Collections;
using System.Collections.Generic;
using ReuseSystem.ObjectPooling;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoinCollectEffect : MonoBehaviour
{
    [SerializeField] private RectTransform coinBarPosition;
    [SerializeField] private int numberOfCoinInstantiate;
    [SerializeField] private CoinCollect prefab;
    [SerializeField] private float offset;
    [SerializeField] private float offsetBar;
    [SerializeField] private float timeWait;
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    [ContextMenu("fire")]
    public void ShowCoinAddEffect()
    {
        StartCoroutine(StartEffect());
    }

    IEnumerator StartEffect()
    {
        WaitForSeconds waitEachInstantiate = new WaitForSeconds(0.1f);
        for (int i = 0; i < numberOfCoinInstantiate; i++)
        {
            var coinCollect = LazyPool.Instance.GetObj<CoinCollect>(prefab.gameObject);
            coinCollect.Init(GetRandomPosition(rect.position,offset),rect, GetRandomPosition(coinBarPosition.position, offsetBar),timeWait);
            yield return waitEachInstantiate;
        }
        
    }

    private Vector2 GetRandomPosition(Vector3 originPos,float offsetAround)
    {
        return originPos + new Vector3(Random.Range(-offsetAround, offsetAround), Random.Range(-offsetAround, offsetAround),0);
    }
}
