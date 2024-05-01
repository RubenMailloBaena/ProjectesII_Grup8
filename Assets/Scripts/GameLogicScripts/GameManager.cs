using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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


    [SerializeField] private List<GameObject> checkPoints;
    private int currentIndex = 0;

    public event Action<ColorType> onPlayerDeath;

    private GameObject pauseUIInstance;
    private bool gamePaused = false;
    
    [SerializeField]
    private float deathTime; 

    private void Start()
    {
        pauseUIInstance = GameObject.Find("GamePauseUI");
    }

    private void Update()
    {
        ManagePauseGame();    
    }

    private void ManagePauseGame() {
        Debug.Log(pauseUIInstance + " " + gamePaused);

        if (gamePaused) {
            pauseUIInstance.SetActive(true);
            Time.timeScale = 0f;
        }
        else {
            pauseUIInstance.SetActive(false);
            Time.timeScale = 1f;  
        }
    }


    public void MoveToCheckPoint()
    {
        StartCoroutine(WaitForRevive());
    }

    private IEnumerator WaitForRevive()
    {
        yield return new WaitForSeconds(deathTime);
        GameObject player = GameObject.Find("Player");
        player.transform.position = checkPoints[currentIndex].transform.position;
        onPlayerDeath.Invoke(ColorType.Default);
        
       
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
        PlayerInputs.Instance.onPauseGame -= PauseGame;
    }
}
