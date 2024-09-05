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
    [SerializeField] private PlayerAttackState playerAttackState;
    
    protected override void Awake()
    {
        base.Awake();
        rigibody = GetComponent<Rigidbody>();
        joystickInput = FindObjectOfType<FloatingJoystick>();
        
        playerIdleState.OnInit(this, stateMachine,this );
        playerMoveState.OnInit(this, stateMachine, this);
        playerAttackState.OnInit(this, stateMachine, this);
    }
    
    public override void OnInit()
    {
        base.OnInit();
        rigibody.isKinematic = false;
        currentLevel = 0;
        indicator.Init("You", currentLevel, CharacterSkin.GetColor(), transform);
        stateMachine.Initialize(playerIdleState);
    }

    public override void OnDeath()
    {
        base.OnDeath();
        rigibody.isKinematic = true;
    }

    public override void ChangeFromStateToState(State fromState)
    {
        if (fromState == playerIdleState)
        {
            if (playerAttackState.CanAttack() && playerIdleState.IsDetectOpponent && CheckOpponentInRange())
            {
                stateMachine.ChangeState(playerAttackState);
                return;
            }
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

        if (fromState == playerAttackState)
        {
            if (GetMoveDirectionInput() != Vector3.zero)
            {
                stateMachine.ChangeState(playerMoveState);
                return;
            }
            if (playerAttackState.IsAnimationFinish())
            {
                stateMachine.ChangeState(playerIdleState);
                return;
            }
        }
    }

    public override void LevelUp()
    {
        base.LevelUp();
        CameraController.Instance.UpdateOffset(currentLevel);
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
