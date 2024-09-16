using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ReuseSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopCanvas : UICanvas
{
    [SerializeField] private TextMeshProUGUI itemNameTxt;
    [SerializeField] private Image weaponImage;
    [SerializeField] private GameObject nextButtonSkin;
    [SerializeField] private GameObject previousButtonSkin;
    [SerializeField] private TextMeshProUGUI itemInformationTxt;
    
    
    //Button Interact
    [SerializeField] private TextMeshProUGUI selectButtonTxt;
    [SerializeField] private TextMeshProUGUI equippedButtonTxt;
    [SerializeField] private TextMeshProUGUI buyButtonTxt;
    [SerializeField] private Image buttonInteractImg;
    [SerializeField] private GameObject buySectionObj;
    [SerializeField] private Color normalButtonColor;
    [SerializeField] private Color buyButtonColor;
    
    
    private List<WeaponEquipmentData> data;
    private ItemStatusEnum weaponInteractStatus;
    private int currentIndexWeapon;
    private void Start()
    {
        LoadData();
        ShowItem(0);
    }
    void LoadData()
    {
        try
        {
            data = GameAssets.Instance.EquipmentManager.GetAllEquipmentDataOfType(EquipmentType.Weapon)
                .Cast<WeaponEquipmentData>().ToList();
            
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }

    public void InteractButtonClick()
    {
        var weaponData = data[currentIndexWeapon];
        var equipmentSavingData = PlayerSavingData.GetPlayerEquipmentData();
        if (weaponInteractStatus == ItemStatusEnum.Buy)
        {
            if (PlayerSavingData.PlayerCurrentCoin >= weaponData.cost)
            {
                PlayerSavingData.PlayerCurrentCoin -= weaponData.cost;
                equipmentSavingData.UpdateEquipmentStatus(weaponData.id,true);
            }
        }
        else if (weaponInteractStatus == ItemStatusEnum.Equip)
        {
            equipmentSavingData.EquipSkin(weaponData.id, EquipmentType.Weapon);
        }
        else if (weaponInteractStatus == ItemStatusEnum.UnEquip)
        {
            GameController.Instance.ChangeGameState(GameState.GameHome);
        }
        UpdateButtonAccordingToItemStatus();
    }
    public void ShowNextItem()
    {
        if(currentIndexWeapon < data.Count-1)
            ShowItem(currentIndexWeapon+1);
    }

    public void ShowPreviousItem()
    {
        if(currentIndexWeapon > 0)
            ShowItem(currentIndexWeapon-1);
    }
    private void ShowItem(int index)
    {
        if (index < 0 || index >= data.Count) return;
        previousButtonSkin.SetActive(index > 0);
        nextButtonSkin.SetActive(index < data.Count -1);

        currentIndexWeapon = index;
        var weaponData = data[index];
        weaponImage.sprite = weaponData.imageSprite;
        itemNameTxt.text = weaponData.weaponName;
        
        string description = string.IsNullOrEmpty(weaponData.weaponDescription) ? "None" : weaponData.weaponDescription;
        itemInformationTxt.text =
            "Speed: " + weaponData.weaponPrefab.GetSpeed() + "<br>" + "Description: " + description;

      
        UpdateButtonAccordingToItemStatus();
    }

    private void UpdateButtonAccordingToItemStatus()
    {
        var weaponData = data[currentIndexWeapon];
        var playerData = PlayerSavingData.GetPlayerEquipmentData();
        if (!playerData.IsItemUnlock(weaponData.id))
        {
            weaponInteractStatus = ItemStatusEnum.Buy;
        }
        else
        {
            if (playerData.IsItemEquipped(weaponData.id))
            {
                weaponInteractStatus = ItemStatusEnum.UnEquip;
            }
            else
            {
                weaponInteractStatus = ItemStatusEnum.Equip;
            }
        }

        buttonInteractImg.color = weaponInteractStatus == ItemStatusEnum.Buy ? buyButtonColor : normalButtonColor;
        
        buySectionObj.gameObject.SetActive(weaponInteractStatus == ItemStatusEnum.Buy);
        equippedButtonTxt.gameObject.SetActive(weaponInteractStatus == ItemStatusEnum.UnEquip);
        selectButtonTxt.gameObject.SetActive(weaponInteractStatus == ItemStatusEnum.Equip);
        if (weaponInteractStatus == ItemStatusEnum.Buy)
        {
            buyButtonTxt.text = weaponData.cost.ToString();
        }
    }

    public void ExitWeaponShop()
    {
        GameController.Instance.ChangeGameState(GameState.GameHome);
    }
}
