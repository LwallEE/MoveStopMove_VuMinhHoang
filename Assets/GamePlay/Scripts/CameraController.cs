using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    [SerializeField] private float speedFollow;
    [SerializeField] private Transform target;
    private void FixedUpdate()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * speedFollow);
        }
    }
}
