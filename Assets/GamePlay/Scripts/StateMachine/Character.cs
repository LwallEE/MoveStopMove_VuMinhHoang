using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Animator Anim { get; private set; }
    protected StateMachine stateMachine;
    
    protected virtual void Awake()
    {
        Anim = GetComponentInChildren<Animator>();
        stateMachine = new StateMachine();
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        if(stateMachine.CurrentState != null)
            stateMachine.CurrentState.LogicUpdate();
    }

    protected virtual void FixedUpdate()
    {
        if(stateMachine.CurrentState != null)
            stateMachine.CurrentState.PhysicsUpdate();
    }

    public virtual void PlayAnimation(string animationName, bool value)
    {
        Anim.SetBool(animationName, value);
    }

    public virtual void ChangeFromStateToState(State fromState)
    {
        
    }

    public float GetSpeed()
    {
        return Constants.CHARACTER_BASE_SPEED;
    }
}
