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
    [SerializeField] private ParticleSystem ButterPartciles;
    private void Start()
    {
         animator = GetComponent<Animator>();
        ButterPartciles = GameObject.Find("ButterParticles").GetComponent<ParticleSystem>();
    }
    private void Update()
    {
     animator.SetBool("New Bool", Ceck);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            Ceck = true;
            onCheckPoint?.Invoke(gameObject);
            Destroy(GetComponent<Collider2D>());
            ButterPartciles.Play();
            Invoke("waitTime",0.5f);
        }
    }

    private void  waitTime()
    {
        ButterPartciles.Stop();
    }

   
    
            
}
