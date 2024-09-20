using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProcessBar : MonoBehaviour
{
    [SerializeField] private RectTransform finalPositionRect;
    [SerializeField] private RectTransform initialPositionRect;
    [SerializeField] private RectTransform bestScoreRect;
    [SerializeField] private RectTransform currentProcessRect;

    [SerializeField] private TextMeshProUGUI currentZoneTxt;
    [SerializeField] private TextMeshProUGUI nextZoneTxt;

    [SerializeField] private Image bgNextZoneImage;
    [SerializeField] private Image statusLockNextZoneImage;

    [SerializeField] private Sprite statusLockSprite;
    [SerializeField] private Sprite statusUnlockSprite;

    [SerializeField] private Sprite bgLockSprite;
    [SerializeField] private Sprite bgUnLockSprite;

    public void UpdateProcessBar(bool isWin)
    {
        int total = LevelManager.Instance.GetCurrentMap().GetTotalNumberBot();
        int deadBot = SpawnManager.Instance.GetNumberDeadBot();
        int bestScore = PlayerSavingData.PlayerBestScore;
        bestScore = bestScore == 0 ? total : bestScore;
        
        Debug.Log(deadBot + " " + total);
        bestScoreRect.offsetMax = new Vector2(GetPositionAccordingToPercent((total - bestScore)*1f / total), bestScoreRect.offsetMax.y);
        currentProcessRect.offsetMax =
            new Vector2(GetPositionAccordingToPercent(deadBot *1f/ total), currentProcessRect.offsetMax.y);

        currentZoneTxt.text = "ZONE " + (PlayerSavingData.PlayerCurrentMapIndex + 1);
        nextZoneTxt.text = "ZONE " + (PlayerSavingData.PlayerCurrentMapIndex + 2);

        bgNextZoneImage.sprite = isWin ? bgUnLockSprite : bgLockSprite;
        statusLockNextZoneImage.sprite = isWin ? statusUnlockSprite : statusLockSprite;
    }

    private float GetInitialPos()
    {
        return initialPositionRect.anchoredPosition.x;
    }

    private float GetLastPosition()
    {
        return finalPositionRect.anchoredPosition.x ;
    }

    private float GetPositionAccordingToPercent(float percent)
    {
        //Debug.Log(percent);
        return GetInitialPos() + (GetLastPosition() - GetInitialPos()) * percent;
    }
}
