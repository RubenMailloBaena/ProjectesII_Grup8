using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true;
    }

    public void LevelChanger(String name)
    {
        SceneManager.LoadScene(name);
    }

    public void LeaveLevel() {
        Debug.Log("Leaving Game");
;       SceneManager.LoadScene("LevelSelector");
    }

    public void ResetLevel() {
        Debug.Log("ResetLevel");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
