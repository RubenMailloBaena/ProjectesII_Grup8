using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundEffects : MonoBehaviour
{
    private static GameSoundEffects instance;
    public static GameSoundEffects Instance
    {
        get { 
            if (instance == null)
                instance = FindAnyObjectByType<GameSoundEffects>();
            return instance;
        }
    }
    
    private AudioSource[] PlayerAudioSource;
    private AudioSource GameAudioSource;

    [Header("PLAYER SOUND CLIPS")]
    [SerializeField] private AudioClip tongueWrongDirection;
    [SerializeField] private AudioClip shootTongue;
    [SerializeField] private AudioClip playerJump;
    [SerializeField] private AudioClip playerEnterWater;
    [SerializeField] private AudioClip playerSwim;
    
    void Awake()
    {
        PlayerAudioSource = GameObject.Find("Player ").GetComponents<AudioSource>();
        GameAudioSource = GameObject.Find("Main Camera 1").GetComponent<AudioSource>();
    }

    public void PlayerSoundEffect(playerSounds sound)
    {
        AudioClip clip = null;
        int audioSourceIdx = 0;
        switch (sound)
        {
            case playerSounds.TongueWrongDirection: 
                clip = tongueWrongDirection;
                audioSourceIdx = 0;
                break;
            
            case playerSounds.ShootTongue: 
                clip = shootTongue;
                audioSourceIdx = 0;
                break;
            
            case playerSounds.PlayerJump: 
                clip = playerJump;
                audioSourceIdx = 1;
                break;
            
            case playerSounds.PlayerSwim: 
                clip = playerSwim;
                audioSourceIdx = 3;
                break;
            
            case playerSounds.EnterWater: 
                clip = playerEnterWater;
                audioSourceIdx = 2;
                break;
        }
        
        PlayClip(clip, audioSourceIdx);
    }
    
    private void PlayClip(AudioClip clip, int audioSourceIdx)
    {
        PlayerAudioSource[audioSourceIdx].clip = clip;
        PlayerAudioSource[audioSourceIdx].Play();
    }

    public void StopSoundEffect(playerSounds sound)
    {
        int audioSourceIdx = 0;
        switch (sound)
        {
            case playerSounds.PlayerSwim: 
                audioSourceIdx = 3;
                break;    
        }
        
        StopClip(audioSourceIdx);
    }

    private void StopClip(int audioSourceIdx)
    {
        PlayerAudioSource[audioSourceIdx].Stop();
    }
}

    public enum playerSounds
    {   
        TongueWrongDirection,
        ShootTongue,
        PlayerJump,
        PlayerSwim,
        EnterWater
    }
