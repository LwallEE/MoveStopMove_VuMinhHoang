using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotItemLayout : MonoBehaviour
{
    [SerializeField] private Image itemImage;

    [SerializeField] private GameObject borderChooseGameObject;

    [SerializeField] private GameObject lockIconGameObject;
    [SerializeField] private GameObject equipIndicatorGameObject;
    
    public string ItemId { get; private set; }
    
    public bool IsUnlock { get; private set; }
    public bool IsEquipped { get; private set; }
    public EquipmentData currentData { get; private set; }

    private ItemScrollView scrollViewManager;

    public void Init(EquipmentData data,ItemScrollView scrollView)
    {
        this.scrollViewManager = scrollView;
        this.currentData = data;
        this.ItemId = data.id;
        
        borderChooseGameObject.SetActive(false);
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        this.IsUnlock = PlayerSavingData.GetPlayerEquipmentData().IsItemUnlock(this.ItemId);
        this.IsEquipped = PlayerSavingData.GetPlayerEquipmentData().IsItemEquipped(this.ItemId) && IsUnlock;

        itemImage.sprite = currentData.imageSprite;
        lockIconGameObject.SetActive(!this.IsUnlock);
        equipIndicatorGameObject.SetActive(IsEquipped);
    }

    public void ChooseItem(bool isChoose)
    {
        borderChooseGameObject.SetActive(isChoose);
    }

    public void OnItemClick()
    {
        if (scrollViewManager != null)
        {
            scrollViewManager.ChooseItem(this);
        }
    }
}
