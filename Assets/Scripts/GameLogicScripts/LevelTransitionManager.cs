using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionManager : MonoBehaviour
{
  

    private void Start()
    {
       

        Invoke("LoadNextLevel", 2f); // Ajusta el tiempo según la duración de la animación
    }

    private void LoadNextLevel()
    {
        string nextLevelName = SceneTransitionData.NextLevelName;
        SceneManager.LoadScene(nextLevelName);
    }
}
