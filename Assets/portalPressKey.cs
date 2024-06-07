using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine;

public class coverPressKey : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private PlayableDirector timeline;

    private void Start()
    {
        timeline.stopped += OnTimelineFinished;
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        if(Input.anyKeyDown)
        SceneManager.LoadScene(nextSceneName);
    }
}

