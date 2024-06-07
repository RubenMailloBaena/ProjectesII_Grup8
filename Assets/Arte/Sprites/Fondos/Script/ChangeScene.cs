using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour

{
    [Header("NEXT LEVEL")]
    private string nextLevelName = "InitialMenu";

    // M�todo para ser llamado cuando se reciba la se�al
    public void SceneChanger()
    {
        SceneManager.LoadScene(nextLevelName);
    }
}

