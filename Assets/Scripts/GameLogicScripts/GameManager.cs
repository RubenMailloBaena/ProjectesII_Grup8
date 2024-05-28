using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public event Action<ColorType> onPlayerDeath;
    
    //CHECK POINTS
    [SerializeField] private List<GameObject> checkPoints;
    private int currentIndex = -1;

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

    private ParticleSystem deathParticles;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        pauseUIInstance = Instantiate(pauseUIPrefab);

        GameObject prefabInstance = Instantiate(sceneTransitionPrefab);
        animator = prefabInstance.GetComponent<Animator>();

        deathPartciles = GameObject.Find("DeathParticles").GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        ManagePauseGame();
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

    public void MoveToCheckPoint()
    {
        if (!playerDeath)
        {
            playerDeath = true;
            GameSoundEffects.Instance.PlayerSoundEffect(playerSounds.PlayerDie);
            StartCoroutine(WaitForRevive());
        }
    }

    private IEnumerator WaitForRevive()
    {
         Time.timeScale = 0f;
         deathPartciles.Play();
         PlayerAnimations.Instance.ChangeAnimation(PlayerAnim.Die);
         PlayerAnimations.Instance.ChangeHeadAnimation(HeadAnim.DieHead);
         yield return new WaitForSecondsRealtime(0.7f);
         yield return new WaitForSecondsRealtime(deathTime);
         Time.timeScale = 1f;
         animator.SetTrigger("End");
         CharacterMovement.Instance.SetPlayerPosition(checkPoints[currentIndex].transform.position);
         PlayerAnimations.Instance.ChangeAnimation(PlayerAnim.Idle);
         deathPartciles.Stop();
         onPlayerDeath.Invoke(ColorType.Default);
         playerDeath = false;
         animator.SetTrigger("Start");
    }

    private void nextCheckPoint(GameObject checkPoint)
    {
        GameObject currentCheckPoint;
        
        if (currentIndex < 0)
            currentCheckPoint = checkPoints[0];
        else
            currentCheckPoint = checkPoints[currentIndex];
        
        if (checkPoint != currentCheckPoint) {
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
