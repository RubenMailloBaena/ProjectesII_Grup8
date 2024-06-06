using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionManager : MonoBehaviour
{
  

    private void Start()
    {
       

        Invoke("LoadNextLevel", 3.2f); 
    }

    private void LoadNextLevel()
    {
        string nextLevelName = SceneTransitionData.NextLevelName;
        SceneManager.LoadScene(nextLevelName);
    }
}
