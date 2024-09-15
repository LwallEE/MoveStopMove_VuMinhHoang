using System.Collections;
using System.Collections.Generic;
using ReuseSystem.SaveLoadSystem;
using UnityEngine;

public static class PlayerSavingData
{
   private const string PLAYER_CURRENT_COIN_KEY = "PLAYER_CURRENT_COIN";
   public static int PlayerCurrentCoin
   {
      get => PlayerPrefs.GetInt(PLAYER_CURRENT_COIN_KEY, 0);
      set => PlayerPrefs.SetInt(PLAYER_CURRENT_COIN_KEY, value);
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
