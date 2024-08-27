using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public State CurrentState { get; private set; }

    public void Initialize(State startingState)
    {
        if(CurrentState != null) CurrentState.Exit();
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(State state)
    {
        CurrentState.Exit();
        CurrentState = state;
        CurrentState.Enter();
    }
}
