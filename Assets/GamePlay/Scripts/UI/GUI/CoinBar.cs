using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldTxt;
    [SerializeField] private CoinCollectEffect effect;
    [SerializeField] private float waitTime;
    public void UpdateCoin(bool isShowEffect)
    {
        if (string.IsNullOrEmpty(goldTxt.text) || !isShowEffect)
        {
            goldTxt.text = PlayerSavingData.PlayerCurrentCoin.ToString();
            return;
        }

        try
        {
            int currentGold = int.Parse(goldTxt.text);
            int updateCoin = PlayerSavingData.PlayerCurrentCoin;
            if (updateCoin > currentGold)
            {
                effect.ShowCoinAddEffect();
            }

            StartCoroutine(ChangeCoin(currentGold, updateCoin));
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    IEnumerator ChangeCoin(float currentCoin, float targetCoin)
    {
        goldTxt.text = currentCoin.ToString();
        float distance = targetCoin-currentCoin;
        while (Mathf.Abs(targetCoin-currentCoin) > 1)
        {
            currentCoin += distance*Time.deltaTime/waitTime;
            if (distance < 0 && currentCoin < targetCoin) break;
            if (distance > 0 && currentCoin > targetCoin) break;
            goldTxt.text = Mathf.RoundToInt(currentCoin).ToString();
            yield return null;
        }

        goldTxt.text = Mathf.RoundToInt(targetCoin).ToString();
    }
    
}
