using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement; // Añade esta línea

public class closeVideo : MonoBehaviour
{
    public VideoPlayer video;
    public VideoClip secondVideo;
    public string nextSceneName; // Nombre de la escena a la que quieres cambiar

    void Awake()
    {
        video = GetComponent<VideoPlayer>();
        video.Play();
        video.loopPointReached += CheckOver;
    }

    void CheckOver(VideoPlayer vp)
    {
        if (vp.clip == secondVideo) // Si el video que acaba de terminar es el segundo video
        {
            SceneManager.LoadScene(nextSceneName); // Cambia a la siguiente escena
        }
        else
        {
            // Cuando el primer video termina, cambia el clip del VideoPlayer al segundo video
            video.clip = secondVideo;
            video.Play();
        }
    }
}