using System;
using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using UnityEngine;

public class SkinShopCanvas : UICanvas
{
    [SerializeField] private List<TabChooseTypeItem> tabChooseTypeItems;

    [SerializeField] private List<ItemScrollView> itemScrollViews;

    [SerializeField] private ButtonInteractShop btnInteract;
    [SerializeField] private CoinBar coinBar;
    private SlotItemLayout currentChooseSlotItem;

    private void Start()
    {
        foreach (var item in itemScrollViews)
        {
            item.LoadItem();
        }
        ActiveView(EquipmentType.Hat);
    }

    public void ActiveView(EquipmentType type)
    {
        foreach (var scroll in itemScrollViews)
        {
            scroll.ActiveView(scroll.GetEquipmentType() == type);
        }

        foreach (var tab in tabChooseTypeItems)
        {
            tab.ChooseTab(tab.GetEquipmentType() == type);
        }
    }

    public void SetCurrentSlotItemChoose(SlotItemLayout item)
    {
        currentChooseSlotItem = item;
        btnInteract.UpdateButtonAccordingToItem(currentChooseSlotItem);
        GameController.Instance.GetPlayer().CharacterSkin.ResetSkinToNormal();
        GameController.Instance.GetPlayer().EquipSkin(item.currentData, item.currentData.equipmentType);
    }

    private void RefreshAllItem()
    {
        foreach (var view in itemScrollViews)
        {
            view.RefreshAllItem();
        }
        coinBar.UpdateCoin();
    }
    //function to interact with item: buy, equip, unequipped
    public void InteractItem(SlotItemLayout itemSlot, ButtonInteractShop.ItemStatusEnum statusInteract)
    {
        var data = PlayerSavingData.GetPlayerEquipmentData();
        if (statusInteract == ButtonInteractShop.ItemStatusEnum.Buy)
        {
            if (PlayerSavingData.PlayerCurrentCoin >= itemSlot.currentData.cost)
            {
                PlayerSavingData.PlayerCurrentCoin = PlayerSavingData.PlayerCurrentCoin - itemSlot.currentData.cost;
                data.UpdateEquipmentStatus(itemSlot.currentData.id, true);
            }
        }
        else if (statusInteract == ButtonInteractShop.ItemStatusEnum.Equip)
        {
            data.EquipSkin(itemSlot.currentData.id, itemSlot.currentData.equipmentType);
           
        }
        else if (statusInteract == ButtonInteractShop.ItemStatusEnum.UnEquip)
        {
            data.EquipSkin("", itemSlot.currentData.equipmentType);
        }
        RefreshAllItem();
        btnInteract.UpdateButtonAccordingToItem(itemSlot);
    }

    public void ExitSkinShop()
    {
        GameController.Instance.ChangeGameState(GameState.GameHome);
    }
}
