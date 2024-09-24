using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    private EquipmentData pantData;

    private EquipmentData weaponData;

    private EquipmentData shieldData;

    private EquipmentData hatData;

    private EquipmentData fullSkinData;


    private void RemoveAllSkin()
    {
        pantData = null;
        shieldData = null;
        hatData = null;
        fullSkinData = null;
    }

    public void EquipEquipment(EquipmentType type, EquipmentData data)
    {
        if (type == EquipmentType.Hat) hatData = data;
        if (type == EquipmentType.Pant) pantData = data;
        if (type == EquipmentType.Shield) shieldData = data;
        if (type == EquipmentType.Weapon) weaponData = data;
        if (type == EquipmentType.FullSkin)
        {
            RemoveAllSkin();
            fullSkinData = data;
        }
    }

    public float GetAttributesBuff(StatsType type)
    {
        float amount = 0f;
        if (pantData != null) amount += pantData.GetAttributeBuff(type);
        if (weaponData != null) amount += weaponData.GetAttributeBuff(type);
        if (shieldData != null) amount += shieldData.GetAttributeBuff(type);
        if (hatData != null) amount += hatData.GetAttributeBuff(type);
        if (fullSkinData != null) amount += fullSkinData.GetAttributeBuff(type);
        return amount;
    }
    
    //Testing
    [ContextMenu("Get Range")]
    public void GetRangeBuff()
    {
        Debug.Log("Range " + GetAttributesBuff(StatsType.Range));
    }

    [ContextMenu("Get Speed")]
    public void GetSpeedBuff()
    {
        Debug.Log("Speed " + GetAttributesBuff(StatsType.Speed));
    }
}
