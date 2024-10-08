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
   [SerializeField] private GameObject botInRangeIndicator;
   private NavMeshAgent agent;

   public Action<BotController> OnDeathAction;

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
      ActiveBotInRangeIndicator(false);
      CharacterSkin.RandomSkin();
      Name = GetRandomName();
      currentLevel = Random.Range(0,SpawnManager.Instance.GetNumberDeadBot());
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
      //SpawnManager.Instance.CallbackOnBotDie(this);
      OnDeathAction?.Invoke(this);
      Invoke(nameof(Disable), 3f);
   }

   private void Disable()
   {
      SimplePool.Instance.Despawn(this);
   }

   //TO DO: Update the limit x and z position of map later
   public Vector3 GetRandomPositionInMap()
   {
      /*return transform.position + new Vector3(
         Random.Range(Constants.minRandomOffsetXPos, Constants.maxRandomOffsetXPos) *RandomNegativeAndPositive(),
         0,
         Random.Range(Constants.minRandomOffsetZPos , Constants.maxRandomOffsetZPos)*RandomNegativeAndPositive());*/
      return GetRandomNavMeshPosition(transform.position,
         Random.Range(Constants.RANGE_BOT_RANDOM_MIN, Constants.RANGE_BOT_RANDOM_MAX));
   }

   //random -1f or 1 for target position of bot
   private float RandomNegativeAndPositive()
   {
      if (Helper.IsPercentTrigger(0.5f)) return -1f;
      return 1f;
   }
   private Vector3 GetRandomNavMeshPosition(Vector3 origin, float distance)
   {
      // Generate a random direction
      Vector3 randomDirection = Random.insideUnitSphere * distance;
      randomDirection += origin;

      NavMeshHit hit;
      NavMesh.SamplePosition(randomDirection, out hit, distance, 1);
      Vector3 finalPosition = hit.position;

      return finalPosition; // Return zero if no valid position found
   }
   
   public void MoveToPosition(Vector3 pos)
   {
      if (agent.isOnNavMesh)
      {
         if (agent.isStopped) agent.isStopped = false;
         agent.SetDestination(pos);
      }
   }
   public float GetPathDistance(Vector3 targetPosition)
   {
      NavMeshPath path = new NavMeshPath();
      if (agent.CalculatePath(targetPosition, path))
      {
         float distance = 0.0f;

         if (path.corners.Length > 1)
         {
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
               distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
         }

         return distance;
      }
      return -1f; // Return infinity if the path is invalid
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
   public void ActiveBotInRangeIndicator(bool isActive)
   {
      botInRangeIndicator.SetActive(isActive);
   }
}
