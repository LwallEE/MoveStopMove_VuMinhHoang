using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
   [SerializeField] private List<Map> mapPrefabList;

   private Map currentMap;
   private int currentMapIndex = -1;

   private void LoadMap(int index)
   {
      if (index < 0 || index >= mapPrefabList.Count || index == currentMapIndex) return;
      currentMapIndex = index;
      PlayerSavingData.PlayerCurrentMapIndex = index;
      if(currentMap != null) Destroy(currentMap.gameObject);
      currentMap = Instantiate(mapPrefabList[index]);
   }

   public void LoadCurrentMap()
   {
      LoadMap(PlayerSavingData.PlayerCurrentMapIndex);
   }

   public Map GetCurrentMap()
   {
      return currentMap;
   }
}
