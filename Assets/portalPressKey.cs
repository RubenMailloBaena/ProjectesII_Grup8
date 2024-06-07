using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class portalPressKey : MonoBehaviour
{
    [SerializeField] private string nextSceneName; 

    private void Update()
    {
        // Verifica si se ha presionado cualquier tecla
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}