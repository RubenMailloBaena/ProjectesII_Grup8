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

    private void RotatePlayerHead() {
        mousePositionWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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

        if (Vector2.Angle(directionToLook, playerRight) > TongueController.Instance.GetMaxAngleToShoot()) {
            if (Vector2.Angle(directionToLook, transform.up) < TongueController.Instance.GetExtraAngleToShoot())
            {
                directionToLook = transform.up;
            }
            else if (Vector2.Angle(directionToLook, -transform.up) < TongueController.Instance.GetExtraAngleToShoot())
            {
                directionToLook = -transform.up;
            }
            else 
            {
                cancelLookAt = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(playerHead.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
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
        TongueController.Instance.onShootingTongue += CanNotFlip;
        TongueController.Instance.onNotMovingTongue += CanFlip;
    }

    private void OnDisable()
    {
        TongueController.Instance.onShootingTongue -= CanNotFlip;
        TongueController.Instance.onNotMovingTongue -= CanFlip;   
    }
}
