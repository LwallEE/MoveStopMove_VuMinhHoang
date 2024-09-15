using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using UnityEngine;
using UnityEngine.UI;

public class TabChooseTypeItem : MonoBehaviour
{
    [SerializeField] private Color chooseTabColor;

    [SerializeField] private Color unChooseTabColor;

    [SerializeField] private Image bgImg;
    [SerializeField] private EquipmentType typeEquipment;
    
    public void TabClick()
    {
        UIManager.Instance.GetUI<SkinShopCanvas>().ActiveView(GetEquipmentType());
    }

    public void ChooseTab(bool isChoose)
    {
        if (isChoose)
        {
            bgImg.color = chooseTabColor;
        }
        else
        {
            bgImg.color = unChooseTabColor;
        }
    }

    public EquipmentType GetEquipmentType()
    {
        return typeEquipment;
    }
}
