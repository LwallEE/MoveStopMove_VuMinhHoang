using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : AttackState
{
   private PlayerController player;
   public virtual void OnInit(Character player, StateMachine stateMachine, PlayerController playerControl)
   {
      this.OnInit(player, stateMachine);
      this.player = playerControl;
   }

   public override void Enter()
   {
      base.Enter();
      player.MoveVelocity(Vector3.zero,false);
      player.LookAtTarget();
   }
}
