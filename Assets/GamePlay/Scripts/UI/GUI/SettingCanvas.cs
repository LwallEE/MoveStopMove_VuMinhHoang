using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using UnityEngine;

public class SettingCanvas : UICanvas
{
    public void HomeButtonClick()
    {
        GameController.Instance.OnBackHome();
    }

    public void ContinueButtonClick()
    {
        UIManager.Instance.CloseUI<SettingCanvas>();
        UIManager.Instance.OpenUI<GamePlayCanvas>();
    }

    public override void Open()
    {
        base.Open();
        Time.timeScale = 0f;
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
        Time.timeScale = 1f;
    }
}
