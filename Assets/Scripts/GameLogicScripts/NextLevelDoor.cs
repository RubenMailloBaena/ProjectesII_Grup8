using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelDoor : MonoBehaviour
{
    [Header("DOOR LOGIC")]
    [SerializeField] private int doorHits;
    [SerializeField] private GameObject doorGameObject;
    [SerializeField] private Sprite openDoorSprite;
    private bool canGoNextLevel = false;

    [Header("NEXT LEVEL")]
    [SerializeField] private string nextLevelName;
    [SerializeField] private String firstLevel;
    public void DoorCollided()
    {
        doorHits--;

        if (doorHits <= 0){
            doorGameObject.GetComponent<SpriteRenderer>().sprite = openDoorSprite;
            canGoNextLevel = true;
            CheckPlayerOnTop();
        }
    }

    private void CheckPlayerOnTop() {
        //LAYER DEL PLAYER
        Collider2D[] collider = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f, 9);
        for (int i = 0; i < collider.Length; i++)
            if (collider[i].gameObject.CompareTag("Player"))
                SceneManager.LoadScene(nextLevelName);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name + " " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player") && canGoNextLevel)
        {
            SceneManager.LoadScene(nextLevelName);
        }
    }
}
