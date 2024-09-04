using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ReuseSystem.Helper.Extensions;
using UnityEngine;

[CreateAssetMenu(menuName = "MyAssets/EquipmentData/EquipmentManagerData")]
public class EquipmentManagerSO : ScriptableObject
{
   public List<EquipmentData> data;

   public EquipmentData GetRandomEquipmentData(EquipmentType type)
   {
      List<EquipmentData> filterData = data.Where(x => x.equipmentType == type).ToList();
      if (filterData.Count <= 0) return null;
      return filterData.GetRandomElement();
   }

   public EquipmentData GetEquipmentDataById(string id)
   {
      foreach (var item in data)
      {
         if (item.id == id)
         {
            return item;
         }
      }

      return null;
   }
}
