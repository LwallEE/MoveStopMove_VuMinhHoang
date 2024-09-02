using System;
using System.Collections;
using System.Collections.Generic;
using ReuseSystem.ObjectPooling;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Animator Anim { get; private set; }
    protected Collider collider;
    protected StateMachine stateMachine;
    
    //attack
    [SerializeField] private Weapon weaponPrefab;
    [SerializeField] private Transform firingPoint;
    private AttackState currentAttackState;

    protected Transform target;
    private Collider[] colliderOpponent = new Collider[2];
    [SerializeField] private float rangeCheckOpponent;

    #region UnityEvent
    protected virtual void Awake()
    {
        Anim = GetComponentInChildren<Animator>();
        collider = GetComponent<Collider>();
        stateMachine = new StateMachine();
    }

    protected virtual void Start()
    {
        OnInit();
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
    
    #endregion
    
    protected virtual void OnInit()
    {
        Debug.Log(Constants.PLAYER_LAYER_MASK.value);
        gameObject.layer = Constants.PLAYER_LAYER_MASK;
        collider.enabled = true;
    }

    public virtual void OnDeath()
    {
        Debug.Log(gameObject.name + " death");
        collider.enabled = false;
    }
    public virtual void PlayAnimation(string animationName, bool value)
    {
        Anim.SetBool(animationName, value);
    }

    public virtual void ChangeFromStateToState(State fromState)
    {
        
    }

    
    public void SetAttackState(AttackState attackState)
    {
        this.currentAttackState = attackState;
    }
   

    public bool CheckOpponentInRange()
    {
        var numberCast = Physics.OverlapSphereNonAlloc(transform.position, rangeCheckOpponent, colliderOpponent,
            GetCharacterLayerMask());
        if (numberCast <= 1) return false;
        foreach (var colliderItem in colliderOpponent)
        {
            if (colliderItem.transform != transform)
            {
                target = colliderItem.transform;
                return true;
            }
        }

        return false;
    }

    public void LookAtTarget()
    {
        if (target != null)
        {
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        }
    }

    #region Get Properties
    public float GetSpeed()
    {
        return Constants.CHARACTER_BASE_SPEED;
    }
    private LayerMask GetCharacterLayerMask()
    {
        return (LayerMask)1 << Constants.PLAYER_LAYER_MASK;
    }
    public Transform GetTarget()
    {
        return target;
    }

    public Weapon GetWeapon()
    {
        return LazyPool.Instance.GetObj<Weapon>(weaponPrefab.gameObject);
    }

    public float GetRange()
    {
        return rangeCheckOpponent;
    }

    public Vector3 GetFiringPosition()
    {
        return firingPoint.transform.position;
    }
    

    #endregion
   

    #region AnimationCallback

    public void TriggerAttack()
    {
        if(currentAttackState != null)
            currentAttackState.TriggerAttack();
    }

    public void AnimationFinish()
    {
        if(stateMachine.CurrentState != null)
            stateMachine.CurrentState.AnimationFinishTrigger();
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangeCheckOpponent);
    }
}
