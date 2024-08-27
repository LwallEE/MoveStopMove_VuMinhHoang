using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    protected Character character;

    protected StateMachine stateMachine;
    
    protected float startTime;
    protected bool isAnimationFinish;
    protected bool canBeChangedToAnotherState;
 
    [SerializeField] protected string animBoolName;

    public virtual void OnInit(Character player, StateMachine stateMachine)
    {
        this.character = player;
        this.stateMachine = stateMachine;
    }
    public void SetAnimationName(string str)
    {
        this.animBoolName = str;
    }

    public virtual void Enter()
    {
        DoCheck();
        character.PlayAnimation(animBoolName, true);
        startTime = Time.time;
        isAnimationFinish = false;
        canBeChangedToAnotherState = false;
    }

    public virtual void Exit()
    {
        if(!string.IsNullOrEmpty(animBoolName))
            character.PlayAnimation(animBoolName, false);
        //player.DisableEffectAfterImage();
        canBeChangedToAnotherState = false;
        isAnimationFinish = false;
    }

    public virtual void LogicUpdate()
    {
        character.ChangeFromStateToState(this);
    }

    public virtual void PhysicsUpdate()
    {
        DoCheck();
    }

    public bool IsAnimationFinish()
    {
        return isAnimationFinish;
    }
    public virtual void DoCheck()
    {
    }

    public virtual void ChangeStateTrigger() => canBeChangedToAnotherState = true;
    public virtual void AnimationFinishTrigger() => isAnimationFinish = true;

    public virtual void AnimationTrigger()
    {
    }
}
