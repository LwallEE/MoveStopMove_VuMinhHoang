using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotIdleState : State
{
    private BotController bot;
    
    [SerializeField] private float timeToRefreshCheckOpponent = 0.2f;
    private float currentTimeToRefresh;
    private float idleTime;
    public bool IsOverIdleTime { get; private set; }
    public bool IsDetectOpponent { get; private set; }
    public void OnInit(Character player, StateMachine stateMachine, BotController botController)
    {
        this.OnInit(player, stateMachine);
        this.bot = botController;
    }

    public override void Enter()
    {
        base.Enter();
        bot.StopMoving();
        idleTime = Random.Range(Constants.MIN_BOT_IDLE_TIME, Constants.MAX_BOT_IDLE_TIME);
        IsOverIdleTime = false;
        IsDetectOpponent = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time > startTime + idleTime)
        {
            IsOverIdleTime = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (currentTimeToRefresh < 0)
        {
            IsDetectOpponent = bot.CheckOpponentInRange();
            currentTimeToRefresh = timeToRefreshCheckOpponent;
        }
        else
        {
            currentTimeToRefresh -= Time.fixedDeltaTime;
        }
    }
}
