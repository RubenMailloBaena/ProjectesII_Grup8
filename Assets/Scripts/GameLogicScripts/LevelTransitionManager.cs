using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionManager : MonoBehaviour
{
  

    private void Start()
    {
       

        Invoke("LoadNextLevel", 2f); // Ajusta el tiempo seg�n la duraci�n de la animaci�n
    }

    private void LoadNextLevel()
    {
        string nextLevelName = SceneTransitionData.NextLevelName;
        SceneManager.LoadScene(nextLevelName);
    }
}
