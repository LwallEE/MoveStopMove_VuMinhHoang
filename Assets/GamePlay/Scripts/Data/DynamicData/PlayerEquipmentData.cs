using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ReuseSystem.SaveLoadSystem;
using UnityEngine;
[Serializable]
public class PlayerEquipmentStatus
{
    public string equipmentId;
    public bool isUnlock;

    public PlayerEquipmentStatus(string equipmentId, bool isUnlock)
    {
        this.equipmentId = equipmentId;
        this.isUnlock = isUnlock;
    }
}
public class PlayerEquipmentData : DataRecord
{
    private const string DEFAULT_WEAPON_ID = "axe0_weapon";
    public List<PlayerEquipmentStatus> equipmentStatus;
    public string hatEquipedId;
    public string pantEquipedId;
    public string shieldEquipedId;
    public string fullSkinEquipedId;
    public string weaponEquipedId;

    public string GetHatEquippedId()
    {
        return hatEquipedId;
    }
    public string GetPantEquippedId()
    {
        return pantEquipedId;
    }
    public string GetShieldEquippedId()
    {
        return shieldEquipedId;
    }
    public string GetFullSkinEquippedId()
    {
        return fullSkinEquipedId;
    }
    public string GetWeaponEquippedId()
    {
        if (string.IsNullOrEmpty(weaponEquipedId)) return DEFAULT_WEAPON_ID;
        return weaponEquipedId;
    }
    public void UpdateEquipmentStatus(string equipmentId, bool isUnlock)
    {
        bool isOnList = false;
        if (equipmentStatus == null) equipmentStatus = new List<PlayerEquipmentStatus>();
        foreach (var item in equipmentStatus)
        {
            if (item.equipmentId == equipmentId)
            {
                isOnList = true;
                item.isUnlock = isUnlock;
                break;
            }
        }

        if (isOnList) return;
        equipmentStatus.Add(new PlayerEquipmentStatus(equipmentId, isUnlock));
    }

    public bool IsItemUnlock(string itemId)
    {
        if (equipmentStatus == null) return false;
        foreach (var item in equipmentStatus)
        {
            if (item.equipmentId == itemId)
            {
                return item.isUnlock;
            }
        }

        return false;
    }

    public bool IsItemEquipped(string itemId)
    {
        return hatEquipedId == itemId || pantEquipedId == itemId || shieldEquipedId == itemId
               || fullSkinEquipedId == itemId || weaponEquipedId == itemId;
    }

    public void EquipSkin(string equipmentId, EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.Hat:
                hatEquipedId = equipmentId;
                break;
            case EquipmentType.Pant:
                pantEquipedId = equipmentId;
                break;
            case EquipmentType.Shield:
                shieldEquipedId = equipmentId;
                break;
            case EquipmentType.FullSkin:
                fullSkinEquipedId = equipmentId;
                break;
            case EquipmentType.Weapon:
                weaponEquipedId = equipmentId;
                break;
            default:
                break;
        }
    }
}
