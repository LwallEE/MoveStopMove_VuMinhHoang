using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public const float CHARACTER_BASE_SPEED = 5f;
    public static LayerMask PLAYER_LAYER_MASK = LayerMask.NameToLayer("Player");
    public const string WEAPON_TAG = "Weapon";
    public const string CHARACTER_TAG = "Player";
    
    
    //Bot
    public const float maxRandomOffsetXPos = 5f;
    public const float minRandomOffsetXPos = 2f;
    public const float minRandomOffsetZPos = 2f;
    public const float maxRandomOffsetZPos = 5f;

    public const float MIN_BOT_IDLE_TIME = 1f;
    public const float MAX_BOT_IDLE_TIME = 2.5f;
}
