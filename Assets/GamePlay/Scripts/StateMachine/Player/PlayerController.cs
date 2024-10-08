using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using ReuseSystem.ObjectPooling;
using UnityEngine;

public class PlayerController : Character
{
    private Rigidbody rigibody;
    private FloatingJoystick joystickInput;
    
    [SerializeField] private GameObject rangeBotIndicator;
    //States
    [SerializeField] private PlayerIdleState playerIdleState;
    [SerializeField] private PlayerMoveState playerMoveState;
    [SerializeField] private PlayerAttackState playerAttackState;
    [SerializeField] private PlayerDeadState playerDeadState;
    [SerializeField] private string danceAnim;

    [SerializeField] private PlayerEquipment equipment;
    protected override void Awake()
    {
        base.Awake();
        rigibody = GetComponent<Rigidbody>();
        joystickInput = FindObjectOfType<FloatingJoystick>();
        
        playerIdleState.OnInit(this, stateMachine,this );
        playerMoveState.OnInit(this, stateMachine, this);
        playerAttackState.OnInit(this, stateMachine, this);
        playerDeadState.OnInit(this, stateMachine, this);
    }
    
    public override void OnInit()
    {
        base.OnInit();
        rigibody.isKinematic = false;
        currentLevel = 0;
        Name = "You";
        indicator.Init(Name, currentLevel, CharacterSkin.GetColor(), transform);
        stateMachine.Initialize(playerIdleState);
    }

    private void LoadAllSkin()
    {
        var playerData = PlayerSavingData.GetPlayerEquipmentData();
        var equipmentManager = GameAssets.Instance.EquipmentManager;
        EquipSkin(equipmentManager.GetEquipmentDataById(playerData.GetWeaponEquippedId()),EquipmentType.Weapon);
        EquipSkin(equipmentManager.GetEquipmentDataById(playerData.GetFullSkinEquippedId()),EquipmentType.FullSkin);
        if (!string.IsNullOrEmpty(playerData.GetFullSkinEquippedId()))
        {
            return;
        }
        CharacterSkin.ResetSkinToNormal();
        EquipSkin(equipmentManager.GetEquipmentDataById(playerData.GetHatEquippedId()),EquipmentType.Hat);
        EquipSkin(equipmentManager.GetEquipmentDataById(playerData.GetPantEquippedId()),EquipmentType.Pant);
        EquipSkin(equipmentManager.GetEquipmentDataById(playerData.GetShieldEquippedId()), EquipmentType.Shield);
    }

    public override void OnDeath()
    {
        base.OnDeath();
        rigibody.isKinematic = true;
        stateMachine.ChangeState(playerDeadState);
        UIManager.Instance.OpenUI<PopupRevive>();
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

    public override float GetRange()
    {
        return base.GetRange() + equipment.GetAttributesBuff(StatsType.Range);
    }

    public override float GetSpeed()
    {
        return base.GetSpeed() *(1+equipment.GetAttributesBuff(StatsType.Speed)) ;
    }

    protected override void UpdateTargetDetect(Transform newTarget)
    {
        if (target != newTarget)
        {
            if(target != null) target.GetComponent<BotController>().ActiveBotInRangeIndicator(false);
            if(newTarget != null) newTarget.GetComponent<BotController>().ActiveBotInRangeIndicator(true);
        }
        base.UpdateTargetDetect(newTarget);
    }

    public override void LevelUp()
    {
        base.LevelUp();
        UpdateRangeBot();
        CameraController.Instance.UpdateOffset(currentLevel);
    }

    private void BackToNormal()
    {
        currentLevel = 0;
        UpdateStatsAccordingToLevel();
        indicator.UpdateLevel(currentLevel);
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
        return new Vector3(joystickInput.Direction.x, 0, joystickInput.Direction.y).normalized;
    }

    public void SetJoyStick(FloatingJoystick joystick)
    {
        this.joystickInput = joystick;
    }

    public void ReturnToHome()
    {
        transform.position = LevelManager.Instance.GetCurrentMap().GetPlayerPosition();
        LoadAllSkin();
        BackToNormal();
        transform.forward = Vector3.back;
        CharacterSkin.gameObject.SetActive(true);
        PlayAnimation(danceAnim, false); 
        stateMachine.ChangeState(playerIdleState);
        rigibody.isKinematic = true;
        rangeBotIndicator.SetActive(false);
        if(indicator != null) SimplePool.Instance.Despawn(indicator);
    }

    public void ReturnSkinShop()
    {
        CharacterSkin.gameObject.SetActive(true);
        stateMachine.CurrentState.Exit();
        rigibody.isKinematic = true;
        PlayAnimation(danceAnim, true); 
        rangeBotIndicator.SetActive(false);
    }

    public void ReturnToWeaponShop()
    {
        CharacterSkin.gameObject.SetActive(false);
    }

    private void UpdateRangeBot()
    {
        rangeBotIndicator.transform.localScale = Vector3.one * GetRange() / CurrentScale;
    }
    public void ReturnToGamePlay()
    {
        rangeBotIndicator.SetActive(true);
        UpdateRangeBot();
        rigibody.isKinematic = false;
        colliderr.enabled = true;
        stateMachine.ChangeState(playerIdleState);
        
       
        indicator = SimplePool.Instance.Spawn<BotIndicator>(GameAssets.Instance.characterIndicator);
        indicator.Init(Name, currentLevel, CharacterSkin.GetColor(), transform);
        indicator.transform.SetParent(UICanvasWorld.Instance.transform);
        
    }

    public void WinGame()
    {
        stateMachine.CurrentState.Exit();
        PlayAnimation(danceAnim, true); 
    }

    public void EquipSkin(EquipmentData data,EquipmentType type)
    {
        equipment.EquipEquipment(type, data);
        if (type == EquipmentType.FullSkin && data is FullSkinEquipmentData) //equip full skin
        {
            CharacterSkin.EquipFullSkin(data as FullSkinEquipmentData);
            return;
        }
        Texture texture = null;
        Weapon weapon = null;
        int indexInPlayer = -1;
        if (data != null) indexInPlayer = data.equipmentInPlayerIndex;
        if (data != null && data is PantEquipmentData) texture = (data as PantEquipmentData).pantTexture;
        if (data != null && data is WeaponEquipmentData) weapon = (data as WeaponEquipmentData).weaponPrefab;
        
        CharacterSkin.ChangeSkin(type, texture, indexInPlayer, weapon);
    }

    public void Revive()
    {
        stateMachine.ChangeState(playerIdleState);
        colliderr.enabled = true;
        rigibody.isKinematic = false;
    }
}
