using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class portalPressKey : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    private bool inputEnabled = true;

    private void Update()
    {
        if (inputEnabled)
        {
            if (Input.anyKeyDown)
            {
                StartCoroutine(WaitTillInput());
            }

        }
        
    }

    private IEnumerator WaitTillInput()
    {
        inputEnabled = false; 

        yield return new WaitForSeconds(8f);

        inputEnabled = true;
        SceneManager.LoadScene(nextSceneName);
    }

}