using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using TMPro;
using UnityEngine;

public class GameMenuCanvas : UICanvas
{
    [SerializeField] private TextMeshProUGUI zoneTxt;
    [SerializeField] private TextMeshProUGUI bestTxt;

    public override void Setup()
    {
        base.Setup();
        zoneTxt.text = "ZONE: "+ (PlayerSavingData.PlayerCurrentMapIndex + 1);
        bestTxt.text = "BEST: #" + (PlayerSavingData.PlayerBestScore);
    }

    public void SkinShopClick()
    {
        GameController.Instance.ChangeGameState(GameState.SkinShop);
    }

    public void WeaponShopClick()
    {
        GameController.Instance.ChangeGameState(GameState.WeaponShop);
    }

    public void GamePlayClick()
    {
        GameController.Instance.ChangeGameState(GameState.GamePlay);
    }
}
