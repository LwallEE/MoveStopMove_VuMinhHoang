using System;
using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using UnityEngine;

public class ItemScrollView : MonoBehaviour
{
    [SerializeField] private EquipmentType type;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Transform contentContainer;
    private List<SlotItemLayout> slotItemLayouts;
    
    public void LoadItem()
    {
        List<EquipmentData> dataList = GameAssets.Instance.EquipmentManager.GetAllEquipmentDataOfType(type);
        slotItemLayouts = new List<SlotItemLayout>();
        foreach (var data in dataList)
        {
            var item = Instantiate(itemSlotPrefab, contentContainer).GetComponent<SlotItemLayout>();
            item.Init(data, this);
            slotItemLayouts.Add(item);
        }
    }

    public void ActiveView(bool isActive)
    {
        gameObject.SetActive(isActive);
        if (isActive)
        {
            ChooseFirstItem();
        }
    }

    private void ChooseFirstItem()
    {
        if (slotItemLayouts == null || slotItemLayouts.Count <= 0) return;
        ChooseItem(slotItemLayouts[0]);
    }
    public EquipmentType GetEquipmentType()
    {
        return type;
    }

    public void ChooseItem(SlotItemLayout item)
    {
        foreach (var itemLayout in slotItemLayouts)
        {
            if (itemLayout == item)
            {
                itemLayout.ChooseItem(true);
            }
            else
            {
                itemLayout.ChooseItem(false);
            }
        }
        
        UIManager.Instance.GetUI<SkinShopCanvas>().SetCurrentSlotItemChoose(item);
    }

    public void RefreshAllItem()
    {
        foreach (var item in slotItemLayouts)
        {
            item.UpdateVisual();
        }
    }
}
