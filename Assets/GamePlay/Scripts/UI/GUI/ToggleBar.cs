using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ToggleBar : MonoBehaviour
{
    public enum ToggleEnum
    {
        Sound,
        Vibration,
    }
    [SerializeField] private Image targetImage;
    [SerializeField] private RectTransform containerToggle;
    [SerializeField] private RectTransform toggle;

    [SerializeField] private Sprite onImage;
    [SerializeField] private Sprite offImage;
    
    [SerializeField] private Color offColor;
    [SerializeField] private Color onColor;

    [SerializeField] private ToggleEnum toggleType;
    private bool toggleCurrentState;
    private Image toggleImg;
    
    private void Awake()
    {
        toggleImg = toggle.GetComponent<Image>();
    }

    private void ActiveToggle(bool isOn)
    {
        Debug.Log(toggle.anchoredPosition + " "+containerToggle.sizeDelta);
        if (!isOn)
        {
            toggle.anchoredPosition = new Vector2(toggle.sizeDelta.x / 2, toggle.anchoredPosition.y);
        }
        else
        {
            toggle.anchoredPosition = new Vector2(containerToggle.sizeDelta.x - toggle.sizeDelta.x/2,
                toggle.anchoredPosition.y);
        }

        toggleImg.color = isOn ? onColor : offColor;
        targetImage.sprite = isOn ? onImage : offImage;
        toggleCurrentState = isOn;
    }

    public void ToggleClick()
    {
        ActiveToggle(!toggleCurrentState);
    }

    private void OnEnable()
    {
        bool active = toggleType == ToggleEnum.Sound
            ? PlayerSavingData.PlayerSoundStatus
            : PlayerSavingData.PlayerVibrationStatus;
        ActiveToggle(active);
    }
}
