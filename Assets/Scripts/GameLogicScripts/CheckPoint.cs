using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class CheckPoint : MonoBehaviour
{
    public static event Action<GameObject> onCheckPoint;

    [Header("butttt_0 (1)")]
   
    private Animator animator;
    private bool Ceck = false;
    private void Start()
    {
         animator = GetComponent<Animator>();
    }
    private void Update()
    {
     animator.SetBool("isFlyiing", Ceck);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {

            Ceck = true;
            onCheckPoint?.Invoke(gameObject);
            Destroy(GetComponent<Collider2D>());
           
        } 
    }
}
