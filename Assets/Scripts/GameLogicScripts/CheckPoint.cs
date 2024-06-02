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
    private bool Animation1 = false;
    private bool Animation2 = false;
    [SerializeField] private ParticleSystem ButterPartciles;
    private void Start()
    {
        animator = GetComponent<Animator>();
        ButterPartciles = GameObject.Find("ButterParticles").GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        animator.SetBool("New Bool", Animation1);
        animator.SetBool("New Bool 0", Animation2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            Animation1 = true;
            Invoke("waitTimeAnimation", 0.5f);
            onCheckPoint?.Invoke(gameObject);
            Destroy(GetComponent<Collider2D>());
            ButterPartciles.Play();
            Invoke("waitTimeParticles", 0.5f);
        }
    }

    private void  waitTimeParticles()
    {
        ButterPartciles.Pause();
    }
    public void waitTimeAnimation()
    {
        Animation2 = true;
    }



}
