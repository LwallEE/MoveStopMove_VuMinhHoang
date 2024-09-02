using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAttackState : AttackState
{
    private BotController bot;
    
    public void OnInit(Character player, StateMachine stateMachine, BotController botController)
    {
        this.OnInit(player, stateMachine);
        this.bot = botController;
    }

    public override void Enter()
    {
        base.Enter();
        bot.StopMoving();
        bot.LookAtTarget();
    }
    
}
