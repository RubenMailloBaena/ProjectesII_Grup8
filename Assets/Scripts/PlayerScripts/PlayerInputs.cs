using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class PlayerInputs : MonoBehaviour
{
    private static PlayerInputs instance;
    public static PlayerInputs Instance
    {
        get { 
            if (instance == null)
                instance = FindAnyObjectByType<PlayerInputs>();
            return instance;
        }
    }

    [Header("CONTROLES")]
    [SerializeField] private KeyCode pauseGame;
    [SerializeField] private KeyCode shootKey;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private KeyCode swapColor;

    //Tecla para disparar
    public event Action onShoot;
    //Movimiento
    public event Action onJump;

    private bool inWater;
    //Cambiar Colores
    public event Action onSwapColor;
    //Pausar Juego
    public event Action onPauseGame;

    private void Update()
    {
        //SHOOT TONGUE
        if (Input.GetKeyDown(shootKey) || Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Mouse0))
            onShoot?.Invoke();

        //JUMP
        if (!inWater)
        {
            if(Input.GetKeyDown(jumpKey))
                onJump?.Invoke();
        }
        else
        {
            if (Input.GetKey(jumpKey) || Input.GetKeyDown(jumpKey))
                onJump?.Invoke();
        }

        //COLORS
        if(Input.GetKeyDown(swapColor))
            onSwapColor?.Invoke();

        if(Input.GetKeyDown(pauseGame))
            onPauseGame?.Invoke();
    }
    
    private void InWater() {
        inWater = !inWater;
    }

    private void OnEnable()
    {
        WaterEffect.onWater += InWater;
    }

    private void OnDisable()
    {
        WaterEffect.onWater -= InWater;
    }
}
