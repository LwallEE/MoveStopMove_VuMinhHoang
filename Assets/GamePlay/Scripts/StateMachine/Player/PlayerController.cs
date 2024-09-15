using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private string danceAnim;
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
        LoadAllSkin();
    }

    private void LoadAllSkin()
    {
        var playerData = PlayerSavingData.GetPlayerEquipmentData();
        var equipmentManager = GameAssets.Instance.EquipmentManager;
        if (!string.IsNullOrEmpty(playerData.fullSkinEquipedId))
        {
            EquipSkin(equipmentManager.GetEquipmentDataById(playerData.fullSkinEquipedId),EquipmentType.FullSkin);
            return;
        }
        CharacterSkin.ResetSkinToNormal();
        EquipSkin(equipmentManager.GetEquipmentDataById(playerData.hatEquipedId),EquipmentType.Hat);
        EquipSkin(equipmentManager.GetEquipmentDataById(playerData.pantEquipedId),EquipmentType.Pant);
        EquipSkin(equipmentManager.GetEquipmentDataById(playerData.shieldEquipedId), EquipmentType.Shield);
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

    public void SetJoyStick(FloatingJoystick joystick)
    {
        this.joystickInput = joystick;
    }

    public void ReturnToHome()
    {
        LoadAllSkin();
        PlayAnimation(danceAnim, false); 
        stateMachine.ChangeState(playerIdleState);
        rigibody.isKinematic = true;
        rangeBotIndicator.SetActive(false);
    }

    public void ReturnSkinShop()
    {
        stateMachine.CurrentState.Exit();
        rigibody.isKinematic = true;
        PlayAnimation(danceAnim, true); 
        rangeBotIndicator.SetActive(false);
    }

    public void EquipSkin(EquipmentData data,EquipmentType type)
    {
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
}
