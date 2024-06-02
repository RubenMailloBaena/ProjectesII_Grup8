using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class PlayerInputs : MonoBehaviour
{
    public static PlayerInputs instance;

    [Header("CONTROLES")]
    [SerializeField] private KeyCode pauseGame;
    [SerializeField] private KeyCode shootKey;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private KeyCode swapRightColor;
    [SerializeField] private KeyCode swapLeftColor;

    //Tecla para disparar
    public event Action onShoot;
    //Movimiento
    public event Action onJump;

    private bool inWater;
    //Cambiar Colores
    public event Action onSwapRightColor;
    public event Action onSwapLeftColor;
    //Pausar Juego
    public event Action onPauseGame;

    private void Awake()
    {
        instance = this;
    }

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

        //COLORS
        if(Input.GetKeyDown(swapRightColor))
            onSwapRightColor?.Invoke();
        
        if(Input.GetKeyDown(swapLeftColor))
            onSwapLeftColor?.Invoke();

        if(Input.GetKeyDown(pauseGame))
            onPauseGame?.Invoke();
    }

    private void FixedUpdate()
    {
        if(inWater)
            if (Input.GetKey(jumpKey) || Input.GetKeyDown(jumpKey))
                onJump?.Invoke();
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
