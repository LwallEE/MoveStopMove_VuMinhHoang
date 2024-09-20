using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : State
{
    private PlayerController player;
    [SerializeField] private float timeToRefreshCheckOpponent;
    private float currentTimeToRefresh;
    public virtual void OnInit(Character player, StateMachine stateMachine, PlayerController playerControl)
    {
        this.OnInit(player, stateMachine);
        this.player = playerControl;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.MoveVelocity(player.GetMoveDirectionInput(), true);
        
        if (currentTimeToRefresh < 0)
        {
            player.CheckOpponentInRange();
            currentTimeToRefresh = timeToRefreshCheckOpponent;
        }
        else
        {
            currentTimeToRefresh -= Time.fixedDeltaTime;
        }
    }
}
