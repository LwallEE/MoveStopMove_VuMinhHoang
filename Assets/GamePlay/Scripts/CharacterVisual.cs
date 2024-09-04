using System.Collections;
using System.Collections.Generic;
using ReuseSystem.Helper;
using ReuseSystem.Helper.Extensions;
using UnityEngine;

public class CharacterVisual : MonoBehaviour
{
    //Object link to skin
    [SerializeField] private SkinnedMeshRenderer pantSkinMeshRender;
    [SerializeField] private Transform hairContainer;
    [SerializeField] private Transform weaponContainer;
    [SerializeField] private SkinnedMeshRenderer bodySkinnedMeshRenderer;
    
    [SerializeField] private List<Color> colorToRandom;
    
    [field:SerializeField] public Weapon WeaponPrefab { get; private set; }
    public void RandomSkin()
    {
        RandomEquipment();
    }

    private void RandomFullSkin()
    {
        
    }

    private void RandomEquipment()
    {
        Color bodyColor = colorToRandom.GetRandomElement();
        bodySkinnedMeshRenderer.material.color = bodyColor;
        bodySkinnedMeshRenderer.material.mainTexture = null;
        //Random pant
        if (Helper.IsPercentTrigger(0.2f))
        {
            pantSkinMeshRender.material.mainTexture = null;
            pantSkinMeshRender.material.color = bodyColor;
        }
        else
        {
            pantSkinMeshRender.material.color = Color.white;
            pantSkinMeshRender.material.mainTexture =
                (GameAssets.Instance.EquipmentManager.GetRandomEquipmentData(EquipmentType.Pant) as PantEquipmentData)
                .pantTexture;
        }
        //Random hair
        hairContainer.DisableAllChild();
        if (Helper.IsPercentTrigger(0.5f))
        {
            var hairData = GameAssets.Instance.EquipmentManager.GetRandomEquipmentData(EquipmentType.Hat);
            if(hairData != null) hairContainer.GetChild(hairData.equipmentInPlayerIndex).gameObject.SetActive(true);
        }
        //Random weapon
        weaponContainer.DisableAllChild();
        var weaponData = GameAssets.Instance.EquipmentManager.GetRandomEquipmentData(EquipmentType.Weapon) as WeaponEquipmentData;
        if (weaponData != null)
        {
            weaponContainer.GetChild(weaponData.equipmentInPlayerIndex).gameObject.SetActive(true);
            WeaponPrefab = weaponData.weaponPrefab;
        }
    }
}
