using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteractShop : MonoBehaviour
{
    [SerializeField] private Color selectButtonColor;
    [SerializeField] private Color unEquipButtonColor;
    [SerializeField] private Color buyButtonColor;

    [SerializeField] private Image buttonImage;
    [SerializeField] private TextMeshProUGUI selectTxt;
    [SerializeField] private TextMeshProUGUI unEquipTxt;
    [SerializeField] private TextMeshProUGUI priceTxt;
    [SerializeField] private GameObject priceSection;

    private SlotItemLayout currentItemChoose;

    private ItemStatusEnum currentItemStatus;
    public enum ItemStatusEnum
    {
        Buy,
        Equip,
        UnEquip
    }
    public void UpdateButtonAccordingToItem(SlotItemLayout item)
    {
        DisableAllText();
        this.currentItemChoose = item;
        if (!currentItemChoose.IsUnlock)
        {
           priceSection.gameObject.SetActive(true);
           priceTxt.text = currentItemChoose.currentData.cost.ToString();
           buttonImage.color = buyButtonColor;
           currentItemStatus = ItemStatusEnum.Buy;
        }
        else
        {
            if (currentItemChoose.IsEquipped)
            {
                buttonImage.color = unEquipButtonColor;
                unEquipTxt.gameObject.SetActive(true);
                currentItemStatus = ItemStatusEnum.UnEquip;
            }
            else
            {
                buttonImage.color = selectButtonColor;
                selectTxt.gameObject.SetActive(true);
                currentItemStatus = ItemStatusEnum.Equip;
            }
        }
    }

    public void ButtonOnClick()
    {
        UIManager.Instance.GetUI<SkinShopCanvas>().InteractItem(currentItemChoose, currentItemStatus);
    }

    private void DisableAllText()
    {
        selectTxt.gameObject.SetActive(false);
        unEquipTxt.gameObject.SetActive(false);
        priceSection.gameObject.SetActive(false);
    }
}
