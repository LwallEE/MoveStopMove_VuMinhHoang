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
[CreateAssetMenu(menuName = "MyAssets/EquipmentData/BaseEquipmentData")]
public class EquipmentData : ScriptableObject
{
   public string id;
   public int cost;
   public Sprite imageSprite;
   public EquipmentType equipmentType;
   public int equipmentInPlayerIndex;
}
