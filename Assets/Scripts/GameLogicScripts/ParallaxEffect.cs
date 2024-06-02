using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private float parallaxMultiplier;
    private Transform cameraTransform;
    private Vector3 previousCameraPosition;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;
        
    }

    
    void LateUpdate()
    {
        Vector3 deltaPosition = cameraTransform.position - previousCameraPosition;
        Vector3 parallaxMovement = new Vector3(deltaPosition.x * parallaxMultiplier, deltaPosition.y * parallaxMultiplier, 0);
        transform.Translate(parallaxMovement);
        previousCameraPosition = cameraTransform.position;
    }
}
