using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get { 
            if (instance == null)
                instance = FindAnyObjectByType<GameManager>();
            return instance;
        }
    }
    
    public event Action<ColorType> onPlayerDeath;
    
    //CHECK POINTS
    [SerializeField] private List<GameObject> checkPoints;
    private int currentIndex = 0;

    //PAUSE GAME
    [SerializeField] private GameObject pauseUIPrefab;
    private GameObject pauseUIInstance;
    private bool gamePaused = false;
    private bool playerDeath = false;

    //ANIMACION MUERTE PLAYER
    [SerializeField] private GameObject sceneTransitionPrefab;
    private Animator animator;
    [SerializeField] private ParticleSystem deathPartciles; 
    
    //[SerializeField] private float deathTime;
    [Header("DEATH ANIMATION PARAMETERS")] 
    [SerializeField] private float deathTime;
    
    [SerializeField] private float changeTime;
    [SerializeField] private int restoreSpeed;
    [SerializeField] private float delay;
    
    private float speed; 
    private bool restoreTime = false;
   
    
    private void Start()
    {
        pauseUIInstance = Instantiate(pauseUIPrefab);

        GameObject prefabInstance = Instantiate(sceneTransitionPrefab);
        animator = prefabInstance.GetComponent<Animator>();
    }

    private void Update()
    {
        ManagePauseGame();
        ManageDeathAnimation();
    }
    
    private void ManagePauseGame() {
        if (gamePaused)
        {
            pauseUIInstance.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
        }
        else
        {
            pauseUIInstance.SetActive(false);
            if(!playerDeath)
                Time.timeScale = 1f;
            Cursor.visible = false;
        }
    }

    private void ManageDeathAnimation()
    {
        // Debug.Log(Time.timeScale);
        // if (restoreTime)
        // {
        //     if (Time.timeScale < 1f)
        //     {
        //         Time.timeScale += Time.deltaTime * speed;
        //     }
        //     else
        //     {
        //         Time.timeScale = 1f;
        //         restoreTime = false;
        //         playerDeath = false;
        //     }
        // }
    }


    public void MoveToCheckPoint()
    {
        if (!playerDeath)
        {
            playerDeath = true;
            StartCoroutine(WaitForRevive());
        }
        // playerDeath = true;
        // speed = restoreSpeed;
        //
        // if (delay > 0)
        // {
        //     StopCoroutine(WaitForRevive(delay));
        //     StartCoroutine(WaitForRevive(delay));
        // }
        // else
        // {
        //     restoreTime = true;
        // }
        //
        // PlayerAnimations.Instance.ChangeAnimation(PlayerAnim.Die);
        // Time.timeScale = changeTime;
        // // animator.SetTrigger("End");
        // // CharacterMovement.Instance.SetPlayerPosition(checkPoints[currentIndex].transform.position);
        // // onPlayerDeath.Invoke(ColorType.Default);
    }

    private IEnumerator WaitForRevive()
    {
        // restoreTime = true;
        // yield return new WaitForSecondsRealtime(amt);

        //deathPartciles.Play();
        //yield return new WaitForSeconds(deathPartciles.main.duration);
        //Debug.Log("Tendrian que salir las particulas");
        //deathPartciles.Stop();

         PlayerAnimations.Instance.ChangeAnimation(PlayerAnim.Die);
         Time.timeScale = 0f;
         yield return new WaitForSecondsRealtime(deathTime);
         Time.timeScale = 1f;
         animator.SetTrigger("End");
         CharacterMovement.Instance.SetPlayerPosition(checkPoints[currentIndex].transform.position);
         onPlayerDeath.Invoke(ColorType.Default);
         playerDeath = false;
         animator.SetTrigger("Start");
    }

    private void nextCheckPoint(GameObject checkPoint) {
        if (checkPoint != checkPoints[currentIndex]) {
            currentIndex++;
        }
    }

    private void PauseGame() {
        gamePaused = !gamePaused;
    }

    private void OnEnable()
    {
        CheckPoint.onCheckPoint += nextCheckPoint;
        PlayerInputs.Instance.onPauseGame += PauseGame;
    }

    private void OnDisable()
    {
        CheckPoint.onCheckPoint -= nextCheckPoint;
        if(PlayerInputs.Instance != null) 
            PlayerInputs.Instance.onPauseGame -= PauseGame;
    }
}
