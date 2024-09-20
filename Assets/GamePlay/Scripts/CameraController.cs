using System;
using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using UnityEngine;

public enum CameraMode
{
    HomeMode,
    GameplayMode,
    SkinShopMode,
    WeaponShopMode
}
public class CameraController : Singleton<CameraController>
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform target;
    
    [Header("Gameplay Mode")]
    [SerializeField] private Vector3 offset;

    [SerializeField] private Vector3 angleGameplayMode;
    [SerializeField] private float speedFollow;
  
    [SerializeField] private float paraChangeCameraField;
    
    [Header("Home Mode")]
    [SerializeField] private Vector3 offsetHomeMode;
    [SerializeField] private Vector3 angleHomeMode;

    [Header("Shop Skin Mode")] 
    [SerializeField]
    private Vector3 offsetShopSkinMode;

    [SerializeField] private Vector3 angleShopSkinMode;

   
    private Vector3 currentOffset;
    private CameraMode currentCameraMode;
    
    private void Start()
    {
        
        currentOffset = offset;
    }

    private void FixedUpdate()
    {
        //Gameplay mode
        if (target != null && currentCameraMode == CameraMode.GameplayMode)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + currentOffset, Time.deltaTime * speedFollow);
        }
    }

    public void UpdateOffset(int level)
    {
        var change = Mathf.Clamp(level * Constants.ALPHA_CHANGE_PER_LEVEL_UP*paraChangeCameraField, 0, 4);
        currentOffset = offset + new Vector3(0, change, change * -1f);
    }

    public void ChangeCameraMode(CameraMode mode)
    {
        this.currentCameraMode = mode;

        if (mode == CameraMode.HomeMode)
        {
            UpdateAngleAndOffset(offsetHomeMode, angleHomeMode);
        }

        if (mode == CameraMode.SkinShopMode)
        {
            UpdateAngleAndOffset(offsetShopSkinMode, angleShopSkinMode);
        }

        if (mode == CameraMode.GameplayMode)
        {
            UpdateAngleAndOffset(offset,angleGameplayMode);
        }
    }

    private void UpdateAngleAndOffset(Vector3 offset, Vector3 angle)
    {
        currentOffset = offset;
        cameraTransform.eulerAngles = angle;
        transform.position = target.position + currentOffset;
    }
}
