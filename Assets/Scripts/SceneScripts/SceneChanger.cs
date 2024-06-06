using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private string startName;
    [SerializeField]
    private string levelName;
    [SerializeField] 
    private GameObject button;

    private GameObject lastSelected;
    private EventSystem eventSystem;

    void Start()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        lastSelected = eventSystem.firstSelectedGameObject;
    }
    
    void Update()
    {
        if (PlayerInputs.instance.GetUsingController())
        {
            Cursor.visible = false;
            if (lastSelected != eventSystem.currentSelectedGameObject && eventSystem.currentSelectedGameObject != null)
                lastSelected = eventSystem.currentSelectedGameObject;
            eventSystem.SetSelectedGameObject(lastSelected);
        }
        else
        {
            Cursor.visible = true;
            eventSystem.SetSelectedGameObject(null);
        }
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(startName);
    }

    public void EndGame()
    {
       Debug.Log("Ha salido correctamente"); 
       GameSoundEffects.Instance.PlayUISound(UISounds.ButonEffect);
       Application.Quit();
    }

    
    public void SelectLevel()
    {
        GameSoundEffects.Instance.PlayUISound(UISounds.ButonEffect);
        SceneManager.LoadScene(levelName);
    }
    
}
