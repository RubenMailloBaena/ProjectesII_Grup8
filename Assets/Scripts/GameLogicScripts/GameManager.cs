using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

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
    private Animator gamePauseAnimator;
    private bool gamePaused = false;
    private bool playerDeath = false;

    [SerializeField] private GameObject EventSystemPrefab;
    private EventSystem eventSystemInstance;
    private GameObject lastSelected;

    public bool PlayerDeath
    {
        get { return playerDeath; }
        set { playerDeath = value; }
    }

    //ANIMACION MUERTE PLAYER
    [SerializeField] private GameObject sceneTransitionPrefab;
    private Animator animator;
    [SerializeField] private ParticleSystem deathPartciles; 
    
    //[SerializeField] private float deathTime;
    [Header("DEATH ANIMATION PARAMETERS")] 
   // [SerializeField] private float deathTime;

    private ParticleSystem deathParticles;

    private void Awake()
    {
        if (instance == null)
        {             instance = this;
        DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        eventSystemInstance = Instantiate(EventSystemPrefab, transform.position, Quaternion.identity).GetComponent<EventSystem>();

        pauseUIInstance = Instantiate(pauseUIPrefab);
        gamePauseAnimator = pauseUIInstance.transform.GetChild(0).GetComponent<Animator>();

        eventSystemInstance.firstSelectedGameObject =
            pauseUIInstance.transform.GetChild(0).transform.GetChild(1).gameObject;

        lastSelected = eventSystemInstance.firstSelectedGameObject;

        GameObject prefabInstance = Instantiate(sceneTransitionPrefab);
        animator = prefabInstance.GetComponent<Animator>();

        deathPartciles = GameObject.Find("DeathParticles").GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        ManagePauseGame();
    }
    
    private void ManagePauseGame()
    {
        if (lastSelected != eventSystemInstance.currentSelectedGameObject &&
            eventSystemInstance.currentSelectedGameObject != null)
            lastSelected = eventSystemInstance.currentSelectedGameObject;
        
        if (gamePaused)
        {
            gamePauseAnimator.Play("Appear Animation");
            Time.timeScale = 0f;
            if (PlayerInputs.instance.GetUsingController())
            {
                if (lastSelected != eventSystemInstance.currentSelectedGameObject && eventSystemInstance.currentSelectedGameObject != null)
                    lastSelected = eventSystemInstance.currentSelectedGameObject;
                eventSystemInstance.SetSelectedGameObject(lastSelected);
            }
            else
            {
                eventSystemInstance.SetSelectedGameObject(null);
                Cursor.visible = true;
            }
        }
        else
        {
            gamePauseAnimator.Play("Hide Animation");
            if(!playerDeath)
                Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.None;
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
         PlayerAnimations.Instance.ChangeHeadAnimation(HeadAnim.DieHead);
         PlayerAnimations.Instance.ChangeAnimation(PlayerAnim.Die);
         yield return new WaitForSecondsRealtime(1.9f);
         //yield return new WaitForSecondsRealtime(deathTime);
         Time.timeScale = 1f;
         animator.SetTrigger("End");
         deathPartciles.Stop();
         onPlayerDeath.Invoke(ColorType.Default);
         CharacterMovement.Instance.SetPlayerPosition(checkPoints[currentIndex].transform.position);
         PlayerAnimations.Instance.ChangeHeadAnimation(HeadAnim.IdleHead);
         PlayerAnimations.Instance.ChangeAnimation(PlayerAnim.Idle);
         animator.SetTrigger("Start");
         playerDeath = false;
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
        PlayerInputs.instance.onPauseGame += PauseGame;
    }

    private void OnDisable()
    {
        CheckPoint.onCheckPoint -= nextCheckPoint;
        PlayerInputs.instance.onPauseGame -= PauseGame;
    }
}
