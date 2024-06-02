using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class HeadController : MonoBehaviour
{
    [SerializeField] private float maxTopHeadAngle;
    [SerializeField] private float maxBottomHeadAngle;

    [SerializeField] private GameObject playerHead;
    private Vector3 directionToLook;
    private Vector3 mousePositionWorld;

    private bool canRotateHead = true;
    private bool lookAtTongue = true;
    private bool cancelLookAt = false;

    private void Update()
    {
        RotatePlayerHead();
    }

    private void RotatePlayerHead()
    {
        mousePositionWorld = PlayerInputs.instance.getFlyPosition();
        mousePositionWorld.z = 0.0f;

        directionToLook = (mousePositionWorld - playerHead.transform.position).normalized;
        directionToLook.z = 0.0f;
        if (canRotateHead)
        {
            playerHead.transform.right = directionToLook * Mathf.Sign(transform.localScale.x);

            var eulerDir = playerHead.transform.localEulerAngles;
            eulerDir.z = Mathf.Clamp(eulerDir.z - (eulerDir.z > 180 ? 360 : 0),
                maxBottomHeadAngle,
                maxTopHeadAngle);
            playerHead.transform.localEulerAngles = eulerDir;
            lookAtTongue = true;
        }
        else //mirar directament a la direccio mentres disparem la llengua 
        {
            if (lookAtTongue) {
                ShootingFront();
                if(!cancelLookAt)
                    playerHead.transform.right = directionToLook * Mathf.Sign(transform.localScale.x);
            }
            lookAtTongue = false;
            cancelLookAt = false;
        }
    }

    private void ShootingFront()
    {
        Vector2 playerRight = transform.right;

        if (!CharacterMovement.Instance.GetFacingRight())
            playerRight = -transform.right;

        if (Vector2.Angle(directionToLook, playerRight) > TongueController.instance.GetMaxAngleToShoot()) {
            if (Vector2.Angle(directionToLook, transform.up) < TongueController.instance.GetExtraAngleToShoot())
            {
                directionToLook = transform.up;
            }
            else if (Vector2.Angle(directionToLook, -transform.up) < TongueController.instance.GetExtraAngleToShoot())
            {
                directionToLook = -transform.up;
            }
            else 
            {
                cancelLookAt = true;
            }
        }
    }

    private void CanNotFlip()
    {
        canRotateHead = false;
    }

    private void CanFlip()
    {
        canRotateHead = true;
    }

    private void OnEnable()
    {
        TongueController.instance.onShootingTongue += CanNotFlip;
        TongueController.instance.onNotMovingTongue += CanFlip;
    }

    private void OnDisable()
    {
        TongueController.instance.onShootingTongue -= CanNotFlip;
        TongueController.instance.onNotMovingTongue -= CanFlip;   
    }
}
