using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using UnityEngine;

public class GamePlayCanvas : UICanvas
{
    [SerializeField] private FloatingJoystick floatingJoystick;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public FloatingJoystick GetFloatingJoystick()
    {
        return floatingJoystick;
    }
}
