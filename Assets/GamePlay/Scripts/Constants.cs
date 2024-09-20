using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public const float CHARACTER_BASE_SPEED = 5f;
    public static LayerMask PLAYER_LAYER_MASK = LayerMask.NameToLayer("Player");
    public const string WEAPON_TAG = "Weapon";
    public const string CHARACTER_TAG = "Player";
    public const float CHARACTER_BASE_RANGE_ATTACK = 6f;
    public const float CHARACTER_MAX_RANGE_ATTACK = 12f;
    public const float ALPHA_CHANGE_PER_LEVEL_UP = 0.04f;
    public const float CHARACTER_MAX_SCALE_UP = 2f;
    
    //Bot
    public const float RANGE_BOT_RANDOM_MIN = 4f;
    public const float RANGE_BOT_RANDOM_MAX = 15f;

    public const float MIN_BOT_IDLE_TIME = 1f;
    public const float MAX_BOT_IDLE_TIME = 2.5f;
    
    //
    public const int COST_TO_REVIVE = 100;
}
