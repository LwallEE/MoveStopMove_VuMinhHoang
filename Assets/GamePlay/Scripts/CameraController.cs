using System;
using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private Vector3 offset;

    [SerializeField] private float speedFollow;
    [SerializeField] private Transform target;
    [SerializeField] private float paraChangeCameraField;
    private Vector3 currentOffset;

    private void Start()
    {
        currentOffset = offset;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + currentOffset, Time.deltaTime * speedFollow);
        }
    }

    public void UpdateOffset(int level)
    {
        var change = Mathf.Clamp(level * Constants.ALPHA_CHANGE_PER_LEVEL_UP*paraChangeCameraField, 0, 4);
        currentOffset = offset + new Vector3(0, change, change * -1f);
    }
}
