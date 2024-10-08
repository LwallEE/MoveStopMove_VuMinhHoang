using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using TMPro;
using UnityEngine;

public class PopupEndGameCanvas : UICanvas
{
    [SerializeField] private TextMeshProUGUI goldGetTxt;
    [SerializeField] private TextMeshProUGUI killerTxt;
    [SerializeField] private TextMeshProUGUI rankingTxt;
    
    [SerializeField] private GameObject sectionGameFailed;

    [SerializeField] private GameObject sectionGameWin;
    [SerializeField] private ProcessBar processBar;
    
    public void ShowPopupWin(int goldGet)
    {
        sectionGameWin.SetActive(true);
        sectionGameFailed.SetActive(false);

        goldGetTxt.text = goldGet.ToString();
        processBar.UpdateProcessBar(true);
    }

    public void ShowPopupFailed(int goldGet, string killerName,int rank)
    {
        sectionGameWin.SetActive(false);
        sectionGameFailed.SetActive(true);

        goldGetTxt.text = goldGet.ToString();
        killerTxt.text = killerName;
        rankingTxt.text = $"#{rank}";
        
        processBar.UpdateProcessBar(false);
    }

    public void ChangeToGameHome()
    {
        GameController.Instance.OnBackHome();
    }
}
