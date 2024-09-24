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
    [SerializeField] private Transform shieldContainer;
    [SerializeField] private Transform wingContainer;
    [SerializeField] private Transform tailContainer;
    [SerializeField] private SkinnedMeshRenderer bodySkinnedMeshRenderer;
    [SerializeField] private Color normalPlayerColor;

   
    
    //Color random for bot
    [SerializeField] private List<Color> colorToRandom;
    
    [field:SerializeField] public Weapon WeaponPrefab { get; private set; }
    public void RandomSkin()
    {
        RandomEquipment();
    }
    
    public Color GetColor()
    {
        return bodySkinnedMeshRenderer.material.color;
    }
    private void RandomFullSkin()
    {
        
    }

  
    public void ResetSkinToNormal()
    {
        hairContainer.DisableAllChild();
        shieldContainer.DisableAllChild();
        wingContainer.DisableAllChild();
        tailContainer.DisableAllChild();
        bodySkinnedMeshRenderer.material.mainTexture = null;
        bodySkinnedMeshRenderer.material.color = normalPlayerColor;
        pantSkinMeshRender.enabled = true;
        ChangeSkin(EquipmentType.Pant);
    }

    private void RandomEquipment()
    {
        ResetSkinToNormal();
        Color bodyColor = colorToRandom.GetRandomElement();
        bodySkinnedMeshRenderer.material.color = bodyColor;
        bodySkinnedMeshRenderer.material.mainTexture = null;
        //Random pant
        if (Helper.IsPercentTrigger(0.2f)) //pant with none texture
        {
            ChangeSkin(EquipmentType.Pant);
        }
        else //pant with texture
        {
           ChangeSkin(EquipmentType.Pant, (GameAssets.Instance.EquipmentManager.GetRandomEquipmentData(EquipmentType.Pant) as PantEquipmentData)
               .pantTexture);
        }
        //Random hair
        hairContainer.DisableAllChild();
        if (Helper.IsPercentTrigger(0.3f))
        {
            var hairData = GameAssets.Instance.EquipmentManager.GetRandomEquipmentData(EquipmentType.Hat);
            if(hairData != null) ChangeSkin(EquipmentType.Hat, null,hairData.equipmentInPlayerIndex);
        }
        //Random weapon
        weaponContainer.DisableAllChild();
        var weaponData = GameAssets.Instance.EquipmentManager.GetRandomEquipmentData(EquipmentType.Weapon) as WeaponEquipmentData;
        if (weaponData != null)
        {
           ChangeSkin(EquipmentType.Weapon, null,weaponData.equipmentInPlayerIndex, weaponData.weaponPrefab);
        }
    }

    public void EquipFullSkin(FullSkinEquipmentData data)
    {
        ResetSkinToNormal();
        bodySkinnedMeshRenderer.material.mainTexture = data.bodyTexture;
        bodySkinnedMeshRenderer.material.color = data.bodyTexture == null ? data.bodyColor : Color.white;
        pantSkinMeshRender.enabled = data.bodyTexture == null;
        pantSkinMeshRender.material.color = data.bodyColor;
        foreach (var part in data.skinParts)
        {
            Transform container = null;
            if (part.partPosition == SkinPartEnum.Head)
            {
                container = hairContainer;
            }
            else if (part.partPosition == SkinPartEnum.Tail)
            {
                container = tailContainer;
            }
            else if (part.partPosition == SkinPartEnum.Wing)
            {
                container = wingContainer;
            }
            else if (part.partPosition == SkinPartEnum.LeftArm)
            {
                container = shieldContainer;
            }

            if (container != null)
            {
                var partGameObject = container.GetChild(part.partIndex).gameObject;
                partGameObject.SetActive(true);
                if (!part.isNotApplyColor)
                {
                    partGameObject.GetComponentInChildren<MeshRenderer>().material.color = part.color;
                }
            }
        }
    }
    public void ChangeSkin(EquipmentType type,Texture mainTexture = null, int indexSkin = 0 ,Weapon weaponPrefab = null)
    {
        if (type == EquipmentType.Hat)
        {
            hairContainer.DisableAllChild();
            if (indexSkin >= 0 && indexSkin < hairContainer.childCount)
            {
                hairContainer.GetChild(indexSkin).gameObject.SetActive(true);
            }
            return;
        }

        if (type == EquipmentType.Pant)
        {
            pantSkinMeshRender.material.mainTexture = mainTexture;
            Color textureColor = GetColor();
            if (mainTexture != null) textureColor = Color.white;
            pantSkinMeshRender.material.color = textureColor;
            return;
        }

        if (type == EquipmentType.Weapon)
        {
            weaponContainer.DisableAllChild();
            if (indexSkin >= 0 && indexSkin < weaponContainer.childCount)
            {
                weaponContainer.GetChild(indexSkin).gameObject.SetActive(true);
            }
            WeaponPrefab = weaponPrefab;
            return;
        }

        if (type == EquipmentType.Shield)
        {
            shieldContainer.DisableAllChild();
            if (indexSkin >= 0 && indexSkin < shieldContainer.childCount)
            {
                shieldContainer.GetChild(indexSkin).gameObject.SetActive(true);
            }
        }
    }
}
