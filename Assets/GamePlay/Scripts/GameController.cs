using System;
using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using ReuseSystem.ObjectPooling;
using ReuseSystem.SaveLoadSystem;
using UnityEngine;

public enum GameState
{
    GameHome,
    SkinShop,
    WeaponShop,
    GamePlay,
    GameWin,
    GameLose
}
public class GameController : Singleton<GameController>
{
    private GameState currentGameState;
    private PlayerController mainPlayer;

    protected override void Awake()
    {
        base.Awake();
        mainPlayer = FindObjectOfType<PlayerController>();
        //SaveLoadManager.ClearGameData();
        PlayerSavingData.PlayerCurrentCoin = 10000;
    }

    private void Start()
    {
        LoadPlayer();
        ChangeGameState(GameState.GameHome);
    }

    private void LoadPlayer()
    {
        var gamePlay = UIManager.Instance.GetUI<GamePlayCanvas>();
        gamePlay.CloseDirectly();
        mainPlayer.SetJoyStick(gamePlay.GetFloatingJoystick());
    }

    public void ChangeGameState(GameState state)
    {
        currentGameState = state;
        if (currentGameState == GameState.GameHome)
        {
            LazyPool.Instance.ReleaseAll();
            UIManager.Instance.CloseAll();
            UIManager.Instance.OpenUI<GameMenuCanvas>();
            
            CameraController.Instance.ChangeCameraMode(CameraMode.HomeMode);
            mainPlayer.ReturnToHome();
        }

        if (currentGameState == GameState.SkinShop)
        {
            UIManager.Instance.CloseAll();
            UIManager.Instance.OpenUI<SkinShopCanvas>();
            
            CameraController.Instance.ChangeCameraMode(CameraMode.SkinShopMode);
            mainPlayer.ReturnSkinShop();
        }

        if (currentGameState == GameState.WeaponShop)
        {
            UIManager.Instance.CloseAll();
            UIManager.Instance.OpenUI<WeaponShopCanvas>();
            
            mainPlayer.ReturnToWeaponShop();
        }
    }

    public bool IsInState(GameState state)
    {
        return currentGameState == state;
    }

    private void OnApplicationQuit()
    {
        PlayerSavingData.SaveAllToDisk();
    }

    public PlayerController GetPlayer()
    {
        return mainPlayer;
    }
}
