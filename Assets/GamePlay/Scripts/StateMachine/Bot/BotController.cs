using System;
using System.Collections;
using System.Collections.Generic;
using ReuseSystem.Helper;
using ReuseSystem.Helper.Extensions;
using ReuseSystem.ObjectPooling;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class BotController : Character
{
   private NavMeshAgent agent;

   public string Name { get; private set; }
   //States
   [SerializeField] private BotIdleState botIdleState;
   [SerializeField] private BotMoveState botMoveState;
   [SerializeField] private BotAttackState botAttackState;
   [SerializeField] private BotDeathState botDeathState;
   protected override void Awake()
   {
      base.Awake();
      agent = GetComponent<NavMeshAgent>();
      botIdleState.OnInit(this, stateMachine, this);
      botMoveState.OnInit(this, stateMachine, this);
      botAttackState.OnInit(this, stateMachine, this);
      botDeathState.OnInit(this, stateMachine, this);
   }

   protected override void Start()
   {
   }

   public override void OnInit()
   {
      base.OnInit();
      CharacterSkin.RandomSkin();
      Name = GetRandomName();
      currentLevel = Random.Range(0,SpawnManager.Instance.GetNumberDeadBot()/2);
      UpdateStatsAccordingToLevel();
      indicator.Init(Name, currentLevel, CharacterSkin.GetColor(), transform);
      stateMachine.Initialize(botMoveState);
   }
   

   public override void ChangeFromStateToState(State fromState)
   {
      base.ChangeFromStateToState(fromState);
      if (fromState == botIdleState)
      {
         if (botAttackState.CanAttack() && botIdleState.IsDetectOpponent)
         {
            stateMachine.ChangeState(botAttackState);
            return;
         }
         if (botIdleState.IsOverIdleTime)
         {
            stateMachine.ChangeState(botMoveState);
            return;
         }
      }

      if (fromState == botMoveState)
      {
         if (botMoveState.IsGoToDestination)
         {
            stateMachine.ChangeState(botIdleState);
            return;
         }
      }

      if (fromState == botAttackState)
      {
         if (botAttackState.IsAnimationFinish())
         {
            stateMachine.ChangeState(botMoveState);
            return;
         }
      }
   }

   public override void OnDeath()
   {
      base.OnDeath();
      stateMachine.ChangeState(botDeathState);
      SpawnManager.Instance.CallbackOnBotDie();
      Invoke(nameof(Disable), 3f);
   }

   private void Disable()
   {
      LazyPool.Instance.AddObjectToPool(gameObject);
   }

   //TO DO: Update the limit x and z position of map later
   public Vector3 GetRandomPositionInMap()
   {
      return transform.position + new Vector3(
         Random.Range(Constants.minRandomOffsetXPos, Constants.maxRandomOffsetXPos) *RandomNegativeAndPositive(),
         0,
         Random.Range(Constants.minRandomOffsetZPos , Constants.maxRandomOffsetZPos)*RandomNegativeAndPositive());
   }

   //random -1f or 1 for target position of bot
   private float RandomNegativeAndPositive()
   {
      if (Helper.IsPercentTrigger(0.5f)) return -1f;
      return 1f;
   }
   
   public void MoveToPosition(Vector3 pos)
   {
      if (agent.isOnNavMesh)
      {
         if (agent.isStopped) agent.isStopped = false;
         agent.SetDestination(pos);
      }
   }

   public bool IsReachDestination()
   {
      if (!agent.isOnNavMesh) return false;
      return agent.remainingDistance < 0.1f;
   }

   public void StopMoving()
   {
      agent.isStopped = true;
   }

   private string GetRandomName()
   {
      return GameAssets.Instance.BotNames.GetRandomElement();
   }
}
