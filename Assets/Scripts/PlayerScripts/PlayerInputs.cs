using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInputs : MonoBehaviour
{
    public static PlayerInputs instance;

    [Header("CONTROLES")]
    [SerializeField] private KeyCode pauseGame;
    [SerializeField] private KeyCode shootKey;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private KeyCode swapRightColor;
    [SerializeField] private KeyCode swapLeftColor;

    // Tecla para disparar
    public event Action onShoot;
    // Movimiento
    public event Action onJump;

    private bool inWater;
    // Cambiar Colores
    public event Action onSwapRightColor;
    public event Action onSwapLeftColor;
    // Pausar Juego
    public event Action onPauseGame;

    private bool usingController = true;

    // Variables para el control del ratón con el mando
    private Vector3 joystickMousePosition;
    private Vector3 lastJoystickMousePosition; // Almacenar la última posición del ratón con mando
    [SerializeField] private float joystickSensitivity = 100.0f;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        joystickMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        lastJoystickMousePosition = joystickMousePosition; // Inicializar la última posición del ratón
    }

    private void Update()
    {
        // SHOOT TONGUE
        if (Input.GetKeyDown(shootKey) || Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Mouse0))
        {
            onShoot?.Invoke();
            usingController = false;
        }
        else if (Input.GetButtonDown("R2"))
        {
            onShoot?.Invoke();
            usingController = true;
        }

        // JUMP
        if (!inWater)
        {
            if (Input.GetKeyDown(jumpKey))
            {
                onJump?.Invoke();
                usingController = false;
            }
            else if (Input.GetButtonDown("X"))
            {
                onJump?.Invoke();
                usingController = true;
            }
        }

        // COLORS
        if (Input.GetKeyDown(swapRightColor))
        {
            onSwapRightColor?.Invoke();
            usingController = false;
        }
        else if (Input.GetButtonDown("R1"))
        {
            onSwapRightColor?.Invoke();
            usingController = true;
        }

        if (Input.GetKeyDown(swapLeftColor))
        {
            onSwapLeftColor?.Invoke();
            usingController = false;
        }
        else if (Input.GetButtonDown("L1"))
        {
            onSwapLeftColor?.Invoke();
            usingController = true;
        }

        if (Input.GetKeyDown(pauseGame))
        {
            onPauseGame?.Invoke();
            usingController = false;
        }
        else if (Input.GetButtonDown("Options"))
        {
            onPauseGame?.Invoke();
            usingController = true;
        }

        // Lógica para mover el "ratón" con el mando
        if (usingController)
        {
            float rightStickHorizontal = Input.GetAxis("RightJoystickHorizontal");
            float rightStickVertical = Input.GetAxis("RightJoystickVertical");

            if (Mathf.Abs(rightStickHorizontal) > 0.1f || Mathf.Abs(rightStickVertical) > 0.1f)
            {
                // Actualizar la posición del ratón solo si el joystick se está moviendo
                lastJoystickMousePosition = joystickMousePosition;
                Vector3 joystickDirection = new Vector3(rightStickHorizontal, rightStickVertical, 0.0f);
                joystickMousePosition += joystickDirection * joystickSensitivity * Time.deltaTime;

                // Limitar la posición del "ratón" a los límites de la pantalla
                Vector3 screenPos = Camera.main.WorldToScreenPoint(joystickMousePosition);
                screenPos.x = Mathf.Clamp(screenPos.x, 0, Screen.width);
                screenPos.y = Mathf.Clamp(screenPos.y, 0, Screen.height);
                joystickMousePosition = Camera.main.ScreenToWorldPoint(screenPos);
                joystickMousePosition.z = 0.0f;
            }
        }
    }

    private void FixedUpdate()
    {
        if (inWater)
        {
            if (Input.GetKey(jumpKey) || Input.GetKeyDown(jumpKey))
            {
                onJump?.Invoke();
                usingController = false;
            }
            else if (Input.GetButton("X") || Input.GetButtonDown("X"))
            {
                onJump?.Invoke();
                usingController = true;
            }
        }
    }

    public Vector3 getFlyPosition()
    {
        Vector3 mousePositionWorld = Vector3.zero;
        if (!usingController)
        {
            mousePositionWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePositionWorld.z = 0f;
        }
        else
        {
            // Lógica para mover el ratón con mando
            mousePositionWorld = joystickMousePosition;
        }

        return mousePositionWorld;
    }

    private void InWater()
    {
        inWater = !inWater;
    }

    public bool GetUsingController()
    {
        return usingController;
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
