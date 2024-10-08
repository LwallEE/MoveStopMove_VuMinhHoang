using System;
using System.Collections;
using System.Collections.Generic;
using ReuseSystem.ObjectPooling;
using UnityEngine;

public class Character : GameUnit
{
    public string KillerName { get; private set; }
    public string Name { get; protected set; }
    [field:SerializeField] public Animator Anim { get; private set; }
    [field:SerializeField] public CharacterVisual CharacterSkin { get; private set; }
    protected Collider colliderr;
    protected StateMachine stateMachine;
    
    //attack
    [SerializeField] private Transform firingPoint;
    private AttackState currentAttackState;

    protected Transform target;
    private Collider[] colliderOpponent = new Collider[2];
    protected float rangeCheckOpponent;
    protected int currentLevel;
    public float CurrentScale { get; private set; }

    protected BotIndicator indicator;
    #region UnityEvent
    protected virtual void Awake()
    {
        colliderr = GetComponent<Collider>();
        stateMachine = new StateMachine();
    }

    protected virtual void Start()
    {
        OnInit();
    }

   

    protected virtual void Update()
    {
        if(stateMachine.CurrentState != null && GameController.Instance.IsInState(GameState.GamePlay))
            stateMachine.CurrentState.LogicUpdate();
    }

    protected virtual void FixedUpdate()
    {
        if(stateMachine.CurrentState != null && GameController.Instance.IsInState(GameState.GamePlay))
            stateMachine.CurrentState.PhysicsUpdate();
    }
    
    #endregion
    
    public virtual void OnInit()
    {
        //Debug.Log(Constants.PLAYER_LAYER_MASK.value);
        gameObject.layer = Constants.PLAYER_LAYER_MASK;
        colliderr.enabled = true;
        rangeCheckOpponent = Constants.CHARACTER_BASE_RANGE_ATTACK;
        CurrentScale = 1f;
        indicator = SimplePool.Instance.Spawn<BotIndicator>(GameAssets.Instance.characterIndicator);
        indicator.transform.SetParent(UICanvasWorld.Instance.transform);
    }

    public virtual void OnDeath()
    {
        //Debug.Log(gameObject.name + " death");
        colliderr.enabled = false;
        if(indicator != null)
            SimplePool.Instance.Despawn(indicator);
    }
    public virtual void PlayAnimation(string animationName, bool value)
    {
        Anim.SetBool(animationName, value);
    }

    public virtual void LevelUp()
    {
        currentLevel++;
        UpdateStatsAccordingToLevel();
        indicator.UpdateLevel(currentLevel);
    }

    protected virtual void UpdateStatsAccordingToLevel()
    {
        rangeCheckOpponent =
            Mathf.Clamp(Constants.CHARACTER_BASE_RANGE_ATTACK * (1 + Constants.ALPHA_CHANGE_PER_LEVEL_UP * currentLevel),
                Constants.CHARACTER_BASE_RANGE_ATTACK,Constants.CHARACTER_MAX_RANGE_ATTACK);
        CurrentScale = 
            Mathf.Clamp(1 + Constants.ALPHA_CHANGE_PER_LEVEL_UP * currentLevel,
                1, Constants.CHARACTER_MAX_SCALE_UP);
        Tf.localScale = CurrentScale * Vector3.one;
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
        var numberCast = Physics.OverlapSphereNonAlloc(Tf.position, GetRange(), colliderOpponent,
            GetCharacterLayerMask());
        if (numberCast <= 1)
        {
            UpdateTargetDetect(null);
            return false;
        }
        foreach (var colliderItem in colliderOpponent)
        {
            if (colliderItem.transform != Tf)
            {
                
                UpdateTargetDetect(colliderItem.transform);
                return true;
            }
        }

        return false;
    }

    protected virtual void UpdateTargetDetect(Transform newTarget)
    {
        target = newTarget;
    }
    public void LookAtTarget()
    {
        if (target != null)
        {
            Tf.LookAt(new Vector3(target.position.x, Tf.position.y, target.position.z));
        }
    }

    public void SetKillerName(string name)
    {
        KillerName = name;
    }
    #region Get Properties
    public virtual float GetSpeed()
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
        return SimplePool.Instance.Spawn<Weapon>(CharacterSkin.WeaponPrefab);
    }

    public virtual float GetRange()
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
        Gizmos.DrawWireSphere(transform.position, GetRange());
    }
}
