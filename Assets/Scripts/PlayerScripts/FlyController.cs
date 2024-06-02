using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyController : MonoBehaviour
{
    void Update()
    {
        if (Time.timeScale != 0)
        {
            Vector3 mousePosition = PlayerInputs.instance.getFlyPosition();

            transform.position = mousePosition;
        }
    }
}
