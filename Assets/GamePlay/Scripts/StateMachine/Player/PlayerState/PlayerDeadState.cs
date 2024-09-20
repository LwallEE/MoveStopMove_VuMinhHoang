using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : State
{
    private PlayerController player;
    public virtual void OnInit(Character player, StateMachine stateMachine, PlayerController playerControl)
    {
        this.OnInit(player, stateMachine);
        this.player = playerControl;
    }
}
