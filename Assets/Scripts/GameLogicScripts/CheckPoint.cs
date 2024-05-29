using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class CheckPoint : MonoBehaviour
{
    public static event Action<GameObject> onCheckPoint;
   
    
   
    
    private void Start()
    {
        
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
              
            
            onCheckPoint?.Invoke(gameObject);
            Destroy(GetComponent<Collider2D>());
           
        } 
    }
}
