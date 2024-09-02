using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    private Transform target;
    
    public override void Enter()
    {
        base.Enter();
        character.SetAttackState(this);
        target = character.GetTarget();
    }

    public virtual void TriggerAttack()
    {
        var weapon = character.GetWeapon();
        weapon.Init(transform.forward, character.GetRange(), character.GetFiringPosition(),character.transform);
    }
}
