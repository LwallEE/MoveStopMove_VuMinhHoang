using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using TMPro;
using UnityEngine;

public class GamePlayCanvas : UICanvas
{
    [SerializeField] private FloatingJoystick floatingJoystick;
    [SerializeField] private TextMeshProUGUI aliveTxt;
    
    public FloatingJoystick GetFloatingJoystick()
    {
        return floatingJoystick;
    }

    public void UpdateAliveText(int number)
    {
        aliveTxt.text = "Alive: " + number.ToString();
    }
}
