using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class Cam2 : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;

    [Range(0.001f, 10f)] 
    public float smoothFactor;

    public Vector3 minValue, maxValue;

    void LateUpdate()
    {
        Follow();
    }

    void Follow()
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position,targetPosition,smoothFactor*Time.deltaTime);
        transform.position = smoothPosition;
    }
}
