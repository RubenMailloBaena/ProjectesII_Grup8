using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private float parallaxMultiplierX;
    [SerializeField] private float parallaxMultiplierY;
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
        Vector3 parallaxMovement = new Vector3(deltaPosition.x * parallaxMultiplierX, deltaPosition.y * parallaxMultiplierY, 0);
        transform.Translate(parallaxMovement);
        previousCameraPosition = cameraTransform.position;
    }
}