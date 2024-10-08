using System.Collections;
using System.Collections.Generic;
using ReuseSystem.SaveLoadSystem;
using UnityEngine;

public static class PlayerSavingData
{
   private const string PLAYER_CURRENT_COIN_KEY = "PLAYER_CURRENT_COIN";
   private const string PLAYER_BEST_SCORE_KEY = "PLAYER_BEST_SCORE";
   private const string PLAYER_CURRENT_MAP_INDEX_KEY = "PLAYER_CURRENT_MAP_INDEX";
   private const string PLAYER_SOUND_STATUS_KEY = "PLAYER_SOUND_STATUS_KEY";
   private const string PLAYER_VIBRATION_STATUS_KEY = "PLAYER_VIBRATION_STATUS_KEY";
   private const int PLAYER_MAX_MAP_INDEX = 1;
   public static int PlayerCurrentCoin
   {
      get => PlayerPrefs.GetInt(PLAYER_CURRENT_COIN_KEY, 0);
      set => PlayerPrefs.SetInt(PLAYER_CURRENT_COIN_KEY, value);
   }

   public static int PlayerCurrentMapIndex
   {
      get => PlayerPrefs.GetInt(PLAYER_CURRENT_MAP_INDEX_KEY, 0);
      set
      {
         if (value <= PLAYER_MAX_MAP_INDEX)
         {
            PlayerPrefs.SetInt(PLAYER_CURRENT_MAP_INDEX_KEY, value);
            PlayerBestScore = 0;
         }
      } 
   }

   //Score according to the number of char alive
   public static int PlayerBestScore
   {
      get => PlayerPrefs.GetInt(PLAYER_BEST_SCORE_KEY, 0);
      set
      {
         if (PlayerBestScore == 0) //0 mean there is no rank
         {
            PlayerPrefs.SetInt(PLAYER_BEST_SCORE_KEY, value);
            return;
         }
         PlayerPrefs.SetInt(PLAYER_BEST_SCORE_KEY, Mathf.Min(value,PlayerBestScore));
      }
   }

   public static bool PlayerSoundStatus
   {
      get => PlayerPrefs.GetInt(PLAYER_SOUND_STATUS_KEY, 0) == 1;
      set => PlayerPrefs.SetInt(PLAYER_SOUND_STATUS_KEY, value ? 1 : 0);
   }

   public static bool PlayerVibrationStatus
   {
      get => PlayerPrefs.GetInt(PLAYER_VIBRATION_STATUS_KEY, 0) == 1;
      set => PlayerPrefs.SetInt(PLAYER_VIBRATION_STATUS_KEY, value ? 1 : 0);
   }
   private static PlayerEquipmentData playerEquipmentData;

   public static PlayerEquipmentData GetPlayerEquipmentData()
   {
      if (playerEquipmentData != null) return playerEquipmentData;
      
      Debug.Log("Get player data from disk".AddColor(Color.blue));
      playerEquipmentData = SaveLoadManager.Instance.Get<PlayerEquipmentData>();
      return playerEquipmentData;
   }

   public static void SetPlayerEquipmentData(PlayerEquipmentData data, bool isSaveToDisk)
   {
      playerEquipmentData = data;
      if (!isSaveToDisk) return;
      playerEquipmentData.Save();
   }

   public static void SaveAllToDisk()
   {
      GetPlayerEquipmentData().Save();
   }
}
