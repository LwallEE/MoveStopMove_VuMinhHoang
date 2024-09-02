using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : State
{
    private PlayerController player;
    [SerializeField] private float timeToRefreshCheckOpponent;
    private float currentTimeToRefresh;
    public bool IsDetectOpponent { get; private set; }
    public virtual void OnInit(Character player, StateMachine stateMachine, PlayerController playerControl)
    {
        this.OnInit(player, stateMachine);
        this.player = playerControl;
    }

    public override void Enter()
    {
        base.Enter();
        player.MoveVelocity(Vector3.zero, false);
        IsDetectOpponent = false;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (currentTimeToRefresh < 0)
        {
            IsDetectOpponent = player.CheckOpponentInRange();
            currentTimeToRefresh = timeToRefreshCheckOpponent;
        }
        else
        {
            currentTimeToRefresh -= Time.fixedDeltaTime;
        }
    }
}
