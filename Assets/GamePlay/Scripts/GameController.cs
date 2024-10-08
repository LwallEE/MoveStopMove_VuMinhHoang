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
        Application.targetFrameRate = 60;
        mainPlayer = FindObjectOfType<PlayerController>();
        SaveLoadManager.ClearGameData();
        PlayerSavingData.PlayerCurrentCoin = 10000;
    }

    private void Start()
    {
        /*LoadPlayer();
        ChangeGameState(GameState.GameHome);*/
        OnInit();
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
        /*
        if (currentGameState == GameState.GameHome)
        {
            UIManager.Instance.CloseAll();
            UIManager.Instance.OpenUI<GameMenuCanvas>();
            
           
            LevelManager.Instance.LoadCurrentMap();
            mainPlayer.ReturnToHome();
            CameraController.Instance.ChangeCameraMode(CameraMode.HomeMode);

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

        if (currentGameState == GameState.GamePlay)
        {
            UIManager.Instance.CloseAll();
            UIManager.Instance.OpenUI<GamePlayCanvas>();
            
            mainPlayer.ReturnToGamePlay();
            CameraController.Instance.ChangeCameraMode(CameraMode.GameplayMode);
            SpawnManager.Instance.LoadMap(LevelManager.Instance.GetCurrentMap());
        }

        if (currentGameState == GameState.GameLose)
        {
            
            UIManager.Instance.CloseAll();
            int deadBot = SpawnManager.Instance.GetNumberDeadBot();
            int total = LevelManager.Instance.GetCurrentMap().GetTotalNumberOfPlayer();
            PlayerSavingData.PlayerBestScore = total - deadBot;
            UIManager.Instance.GetUI<PopupEndGameCanvas>().ShowPopupFailed(deadBot, mainPlayer.KillerName,
                 total- deadBot);
            UIManager.Instance.GetUI<GameMenuCanvas>().ShowCoinUpdateEffect();
            
            PlayerSavingData.PlayerBestScore = Mathf.Max(PlayerSavingData.PlayerBestScore, total - deadBot);
            PlayerSavingData.PlayerCurrentCoin += deadBot;
            UIManager.Instance.OpenUI<PopupEndGameCanvas>();
        }
        if (currentGameState == GameState.GameWin)
        {
            
            UIManager.Instance.CloseAll();
            int deadBot = SpawnManager.Instance.GetNumberDeadBot();
            int total = LevelManager.Instance.GetCurrentMap().GetTotalNumberOfPlayer();
            PlayerSavingData.PlayerBestScore = total - deadBot;
            UIManager.Instance.GetUI<PopupEndGameCanvas>().ShowPopupWin(deadBot);
            UIManager.Instance.GetUI<GameMenuCanvas>().ShowCoinUpdateEffect();
            
            PlayerSavingData.PlayerBestScore = 1;
            PlayerSavingData.PlayerCurrentMapIndex += 1;
            PlayerSavingData.PlayerCurrentCoin += deadBot;
            mainPlayer.WinGame();
            UIManager.Instance.OpenUI<PopupEndGameCanvas>();
        }*/
    }

    public void RevivePlayer()
    {
        mainPlayer.Revive();
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

    public void OnInit()
    {
       SpawnManager.Instance.OnInit();
       LoadPlayer();
       OnBackHome();
    }

    public void OnBackHome()
    {
        ChangeGameState(GameState.GameHome);
        OnDespawn();
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<GameMenuCanvas>();
           
        LevelManager.Instance.LoadCurrentMap();
        mainPlayer.ReturnToHome();
        CameraController.Instance.ChangeCameraMode(CameraMode.HomeMode);
    }

    public void OnWeaponShop()
    {
        ChangeGameState(GameState.WeaponShop);
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<WeaponShopCanvas>();
            
        mainPlayer.ReturnToWeaponShop();
    }

    public void OnSkinShop()
    {
        ChangeGameState(GameState.SkinShop);
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<SkinShopCanvas>();
            
        CameraController.Instance.ChangeCameraMode(CameraMode.SkinShopMode);
        mainPlayer.ReturnSkinShop();
    }
    public void OnPlay()
    {
        ChangeGameState(GameState.GamePlay);
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<GamePlayCanvas>();
            
        mainPlayer.ReturnToGamePlay();
        CameraController.Instance.ChangeCameraMode(CameraMode.GameplayMode);
        SpawnManager.Instance.LoadMap(LevelManager.Instance.GetCurrentMap());
    }

    public void OnWin()
    {
        ChangeGameState(GameState.GameWin);
        UIManager.Instance.CloseAll();
        int deadBot = SpawnManager.Instance.GetNumberDeadBot();
        int total = LevelManager.Instance.GetCurrentMap().GetTotalNumberOfPlayer();
        PlayerSavingData.PlayerBestScore = total - deadBot;
        UIManager.Instance.GetUI<PopupEndGameCanvas>().ShowPopupWin(deadBot);
        UIManager.Instance.GetUI<GameMenuCanvas>().ShowCoinUpdateEffect();
            
        PlayerSavingData.PlayerBestScore = 1;
        PlayerSavingData.PlayerCurrentMapIndex += 1;
        PlayerSavingData.PlayerCurrentCoin += deadBot;
        mainPlayer.WinGame();
        UIManager.Instance.OpenUI<PopupEndGameCanvas>();
    }

    public void OnLose()
    {
        ChangeGameState(GameState.GameLose);
        UIManager.Instance.CloseAll();
        int deadBot = SpawnManager.Instance.GetNumberDeadBot();
        int total = LevelManager.Instance.GetCurrentMap().GetTotalNumberOfPlayer();
        PlayerSavingData.PlayerBestScore = total - deadBot;
        UIManager.Instance.GetUI<PopupEndGameCanvas>().ShowPopupFailed(deadBot, mainPlayer.KillerName,
            total- deadBot);
        UIManager.Instance.GetUI<GameMenuCanvas>().ShowCoinUpdateEffect();
            
        PlayerSavingData.PlayerBestScore = Mathf.Max(PlayerSavingData.PlayerBestScore, total - deadBot);
        PlayerSavingData.PlayerCurrentCoin += deadBot;
        UIManager.Instance.OpenUI<PopupEndGameCanvas>();
    }

    public void OnDespawn()
    {
        SimplePool.Instance.CollectAll();
    }
    
}
