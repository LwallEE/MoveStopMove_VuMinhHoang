using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    private Transform target;
    private Weapon currentWeaponThrow;
    
    public override void Enter()
    {
        base.Enter();
        character.SetAttackState(this);
        target = character.GetTarget();
    }

    public virtual void TriggerAttack()
    {
        currentWeaponThrow = character.GetWeapon();
        currentWeaponThrow.Init(transform.forward, character.GetRange(), character.GetFiringPosition(),character,character.CurrentScale);
    }

    public bool CanAttack()
    {
        return currentWeaponThrow == null || currentWeaponThrow.CanThrowNewWeapon();
    }
}
