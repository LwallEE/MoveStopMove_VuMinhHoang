using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
   Pant,
   Hat,
   Weapon,
   FullSkin,
   Shield
}

public enum StatsType
{
   None,
   Range,
   Speed
}
[Serializable]
public struct StatsAttributeEquipment
{
   public StatsType type;
   public float amount;
   public bool isPercentage;
}
[CreateAssetMenu(menuName = "MyAssets/EquipmentData/BaseEquipmentData")]
public class EquipmentData : ScriptableObject
{
   public string id;
   public int cost;
   public Sprite imageSprite;
   public EquipmentType equipmentType;
   public int equipmentInPlayerIndex;
   public StatsAttributeEquipment statsAttribute;

   public float GetAttributeBuff(StatsType type)
   {
      return type == statsAttribute.type ? statsAttribute.amount : 0f;
   }
}
