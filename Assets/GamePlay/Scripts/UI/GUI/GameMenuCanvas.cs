using System;
using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using TMPro;
using UnityEngine;

public class GameMenuCanvas : UICanvas
{
    [SerializeField] private TextMeshProUGUI zoneTxt;
    [SerializeField] private TextMeshProUGUI bestTxt;
    [SerializeField] private CoinBar coinBar;
    private bool isShowingCoinEffect;
    public override void Setup()
    {
        base.Setup();
        zoneTxt.text = "ZONE: "+ (PlayerSavingData.PlayerCurrentMapIndex + 1);
        bestTxt.text = "BEST: #" + (PlayerSavingData.PlayerBestScore == 0 ? "__" : PlayerSavingData.PlayerBestScore);
        
    }

    private void OnEnable()
    {
        coinBar.UpdateCoin(isShowingCoinEffect);
        isShowingCoinEffect = false;
    }

    public void ShowCoinUpdateEffect()
    {
        isShowingCoinEffect = true;
    }

    public void SkinShopClick()
    {
        GameController.Instance.OnSkinShop();
    }

    public void WeaponShopClick()
    {
        GameController.Instance.OnWeaponShop();
    }

    public void GamePlayClick()
    {
        GameController.Instance.OnPlay();
    }
}
