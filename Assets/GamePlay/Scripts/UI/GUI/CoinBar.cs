using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldTxt;

    public void UpdateCoin()
    {
        goldTxt.text = PlayerSavingData.PlayerCurrentCoin.ToString();
    }

    private void OnEnable()
    {
        UpdateCoin();
    }
}
