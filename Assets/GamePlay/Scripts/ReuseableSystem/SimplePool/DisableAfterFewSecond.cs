using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterFewSecond : GameUnit
{
    [SerializeField] private float seconds;

    private bool isEnable;
    private float currentTime;
    private void Update()
    {
        if (isEnable)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                isEnable = false;
                SimplePool.Instance.Despawn(this);
            }
        }
    }

    private void OnEnable()
    {
        isEnable = true;
        currentTime = seconds;
    }
}
