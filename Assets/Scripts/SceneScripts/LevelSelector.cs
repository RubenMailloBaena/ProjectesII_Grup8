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
        GameSoundEffects.Instance.PlayUISound(UISounds.ButonEffect);
;       SceneManager.LoadScene("InitialMenu");
    }

    public void ResetLevel() {
        Debug.Log("ResetLevel");
        GameSoundEffects.Instance.PlayUISound(UISounds.ButonEffect);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
