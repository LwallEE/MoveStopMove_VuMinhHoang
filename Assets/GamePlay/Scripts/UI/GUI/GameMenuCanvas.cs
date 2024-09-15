using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using UnityEngine;

public class GameMenuCanvas : UICanvas
{
    public void SkinShopClick()
    {
        GameController.Instance.ChangeGameState(GameState.SkinShop);
    }

    public void WeaponShopClick()
    {
        GameController.Instance.ChangeGameState(GameState.WeaponShop);
    }
}
