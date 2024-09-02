using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : Character
{
   private NavMeshAgent agent;
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
      base.Start();
      stateMachine.Initialize(botIdleState);
   }

   public override void ChangeFromStateToState(State fromState)
   {
      base.ChangeFromStateToState(fromState);
      if (fromState == botIdleState)
      {
         if (botIdleState.IsDetectOpponent)
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
   }

   //TO DO: Update the limit x and z position of map later
   public Vector3 GetRandomPositionInMap()
   {
      return transform.position + new Vector3(
         Random.Range(Constants.minRandomOffsetXPos, Constants.maxRandomOffsetXPos) *RandomNegativeAndPositive(),
         0,
         Random.Range(Constants.minRandomOffsetZPos , Constants.maxRandomOffsetZPos)*RandomNegativeAndPositive());
   }

   private float RandomNegativeAndPositive()
   {
      if (Random.Range(0, 2) == 0) return -1f;
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
}
