using System;
using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using TMPro;
using UnityEngine;

public class PopupRevive : UICanvas
{
   [SerializeField] private RectTransform rotateGameObject;
   [SerializeField] private float speedRotate;

   [SerializeField] private TextMeshProUGUI counterTxt;
   [SerializeField] private TextMeshProUGUI coinToReviveTxt;
   [SerializeField] private float maxCounterTime;
   private float counterTime;
   private bool isCounter;
   public override void Setup()
   {
      base.Setup();
      counterTime = maxCounterTime;
      isCounter = true;
      counterTxt.text = Mathf.RoundToInt(counterTime).ToString();
      coinToReviveTxt.text = Constants.COST_TO_REVIVE.ToString();
   }

   private void FixedUpdate()
   {
      rotateGameObject.Rotate(0,0,speedRotate*Time.deltaTime);
      if (isCounter)
      {
         if (counterTime > 0)
         {
            counterTime -= Time.deltaTime;
            counterTxt.text = Mathf.RoundToInt(counterTime).ToString();
         }
         else
         {
            isCounter = false;
            ShowPopUpEndGame();
         }
      }
   }

   private void ShowPopUpEndGame()
   {
      GameController.Instance.ChangeGameState(GameState.GameLose);
   }

   public void OnExitClick()
   {
      GameController.Instance.ChangeGameState(GameState.GameLose);

   }

   public void OnReviveClick()
   {
      if (PlayerSavingData.PlayerCurrentCoin >= Constants.COST_TO_REVIVE)
      {
         PlayerSavingData.PlayerCurrentCoin -= Constants.COST_TO_REVIVE;
         UIManager.Instance.CloseUI<PopupRevive>();
         GameController.Instance.RevivePlayer();
      }
   }
}
