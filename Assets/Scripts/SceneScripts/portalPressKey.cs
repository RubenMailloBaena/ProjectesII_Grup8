using UnityEngine;
using UnityEngine.SceneManagement;

public class portalPressKey : MonoBehaviour
{
    [SerializeField] private string nextSceneName; 

    private void Update()
    {
        
        if (Input.anyKeyDown)
        {
            
            SceneManager.LoadScene(nextSceneName);
        }
    }
}