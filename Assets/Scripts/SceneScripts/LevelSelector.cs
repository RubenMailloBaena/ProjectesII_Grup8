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
        if (Time.timeScale == 0f)
        {
            Debug.Log("Leaving Game");
            GameSoundEffects.Instance.PlayUISound(UISounds.ButonEffect);
            ; SceneManager.LoadScene("InitialMenu");
        }
    }

    public void ResetLevel() {
        if (Time.timeScale == 0f)
        {
            Debug.Log("ResetLevel");
            GameSoundEffects.Instance.PlayUISound(UISounds.ButonEffect);
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
}
