using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMoveState : State
{
    private BotController bot;
    
    public bool IsGoToDestination { get; private set; }
    public void OnInit(Character player, StateMachine stateMachine, BotController botController)
    {
        this.OnInit(player, stateMachine);
        this.bot = botController;
    }

    public override void Enter()
    {
        base.Enter();
        bot.MoveToPosition(bot.GetRandomPositionInMap());
        IsGoToDestination = false;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        IsGoToDestination = bot.IsReachDestination();
    }
}
