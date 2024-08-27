using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    private Rigidbody rigibody;
    private FloatingJoystick joystickInput;
    
    //States
    [SerializeField] private PlayerIdleState playerIdleState;
    [SerializeField] private PlayerMoveState playerMoveState;
    
    protected override void Awake()
    {
        base.Awake();
        rigibody = GetComponent<Rigidbody>();
        joystickInput = FindObjectOfType<FloatingJoystick>();
        
        playerIdleState.OnInit(this, stateMachine,this );
        playerMoveState.OnInit(this, stateMachine, this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(playerIdleState);
    }

    public override void ChangeFromStateToState(State fromState)
    {
        if (fromState == playerIdleState)
        {
            if (GetMoveDirectionInput() != Vector3.zero)
            {
                stateMachine.ChangeState(playerMoveState);
                return;
            }
        }

        if (fromState == playerMoveState)
        {
            if (GetMoveDirectionInput() == Vector3.zero)
            {
                stateMachine.ChangeState(playerIdleState);
                return;
            }
        }
    }

    public void MoveVelocity(Vector3 direction,bool isRefreshRotation)
    {
        direction.Normalize();
        direction.y = 0f;
        rigibody.velocity = GetSpeed() * direction;
        if(isRefreshRotation)
            transform.forward = direction;
    }

    public Vector3 GetMoveDirectionInput()
    {
        return new Vector3(joystickInput.Direction.x, 0, joystickInput.Direction.y);
    }
}
