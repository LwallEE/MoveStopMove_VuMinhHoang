using System;
using System.Collections;
using System.Collections.Generic;
using ReuseSystem.ObjectPooling;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string KillerName { get; private set; }
    public string Name { get; protected set; }
    public Animator Anim { get; private set; }
    public CharacterVisual CharacterSkin { get; private set; }
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
        Anim = GetComponentInChildren<Animator>();
        CharacterSkin = GetComponentInChildren<CharacterVisual>();
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
        indicator = LazyPool.Instance.GetObj<BotIndicator>(GameAssets.Instance.characterIndicator);
        indicator.transform.SetParent(UICanvasWorld.Instance.transform);
    }

    public virtual void OnDeath()
    {
        //Debug.Log(gameObject.name + " death");
        colliderr.enabled = false;
        if(indicator != null)
            LazyPool.Instance.AddObjectToPool(indicator.gameObject);
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
        transform.localScale = CurrentScale * Vector3.one;
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
        if (numberCast <= 1)
        {
            UpdateTargetDetect(null);
            return false;
        }
        foreach (var colliderItem in colliderOpponent)
        {
            if (colliderItem.transform != transform)
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
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        }
    }

    public void SetKillerName(string name)
    {
        KillerName = name;
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
        return LazyPool.Instance.GetObj<Weapon>(CharacterSkin.WeaponPrefab.gameObject);
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
