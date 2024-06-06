using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelDoor : MonoBehaviour
{
    [Header("DOOR LOGIC")]
    [SerializeField] private int doorHits;
    [SerializeField] private GameObject doorGameObject;
    [SerializeField] private GameObject openDoorSprite;
    private bool canGoNextLevel = false;

    [Header("NEXT LEVEL")]
    [SerializeField] private string nextLevelName;
    [SerializeField] private String firstLevel;

    [SerializeField] Animator transitionAnim;

    [SerializeField] private string transitionSceneName; 
    public void DoorCollided()
    {
        doorHits--;

        if (doorHits <= 0)
        {
            doorGameObject.SetActive(false);
            openDoorSprite.SetActive(true);
            canGoNextLevel = true;
            CheckPlayerOnTop();

        }
    }

    private void CheckPlayerOnTop() {
        //LAYER DEL PLAYER
        Collider2D[] collider = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f, 9);
        for (int i = 0; i < collider.Length; i++)
            if (collider[i].gameObject.CompareTag("Player"))
                Invoke("nextLevel", 1f);


    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name + " " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player") && canGoNextLevel)
        {
            StartTransition();
        }
    }


    private void StartTransition()
    {
        transitionAnim.SetTrigger("End");
        Invoke("LoadTransitionScene", 1f);
    }

    private void LoadTransitionScene()
    {
       
        SceneTransitionData.NextLevelName = nextLevelName;
        SceneManager.LoadScene(transitionSceneName);
    }


}
