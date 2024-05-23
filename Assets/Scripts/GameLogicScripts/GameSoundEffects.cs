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
    
    private AudioSource PlayerAudioSource;
    private AudioSource GameAudioSource;

    [Header("PLAYER SOUND CLIPS")]
    [SerializeField] private AudioClip tongueWrongDirection;
    [SerializeField] private AudioClip shootTongue;
    [SerializeField] private AudioClip playerJump;
    [SerializeField] private AudioClip playerEnterWater;
    [SerializeField] private AudioClip playerSwim;
    
    void Awake()
    {
        PlayerAudioSource = GameObject.Find("Player ").GetComponent<AudioSource>();
        GameAudioSource = GameObject.Find("Main Camera 1").GetComponent<AudioSource>();
    }

    public void PlayerSoundEffect(playerSounds anim)
    {
        switch (anim)
        {
            case playerSounds.TongueWrongDirection: PlayerAudioSource.clip = tongueWrongDirection;
                break;
            
            case playerSounds.ShootTongue: PlayerAudioSource.clip = shootTongue;
                break;
            
            case playerSounds.PlayerJump: PlayerAudioSource.clip = playerJump;
                break;
            
            case playerSounds.PlayerSwim: PlayerAudioSource.clip = playerSwim;
                break;
            
            case playerSounds.EnterWater: PlayerAudioSource.clip = playerEnterWater;
                break;
        }
        
        PlayerAudioSource.Play();
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
