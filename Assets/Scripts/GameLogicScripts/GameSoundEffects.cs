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

    public levelSounds actualLevel;
    
    private AudioSource[] PlayerAudioSource;
    private AudioSource GameAudioSource;
    private AudioSource UIAudioSource;

    [Header("PLAYER SOUND CLIPS")]
    [SerializeField] private AudioClip tongueWrongDirection;
    [SerializeField] private AudioClip shootTongue;
    [SerializeField] private AudioClip playerJump;
    [SerializeField] private AudioClip playerEnterWater;
    [SerializeField] private AudioClip playerSwim;
    [SerializeField] private AudioClip playerDie;

    [Header("ENVIROMENT SOUND CLIPS")] 
    [SerializeField] private AudioClip startMenu;
    [SerializeField] private AudioClip ForestLevel;
    [SerializeField] private AudioClip DesertLevel;
    [SerializeField] private AudioClip SnowLevel;
    [SerializeField] private AudioClip StretchTutorial;
    [SerializeField] private AudioClip BounceTutorial;
    [SerializeField] private AudioClip WaterTutorial;


    [Header("UI SOUND EFFECTS")] 
    [SerializeField] private AudioClip ButtonEffect;
    
    void Awake()
    {
        if (actualLevel != levelSounds.Menu)
            PlayerAudioSource = GameObject.Find("Player ").GetComponents<AudioSource>();
        GameAudioSource = GameObject.Find("Main Camera 1").GetComponent<AudioSource>();
        UIAudioSource = GameObject.Find("UISoundEffects").GetComponent<AudioSource>();
        
        PlayLevelSound(actualLevel);
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
            
            case playerSounds.PlayerDie:
                clip = playerDie;
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


    public void PlayLevelSound(levelSounds sound)
    {
        
        switch (sound)
        {
            case levelSounds.Menu:
                GameAudioSource.clip = startMenu;
                break;
            
            case levelSounds.Forest:
                GameAudioSource.clip = ForestLevel;
                break;
            
            case levelSounds.Desert:
                GameAudioSource.clip = DesertLevel;
                break;
            
            case levelSounds.Snow:
                GameAudioSource.clip = SnowLevel;
                break;
            case levelSounds.StretchTutorial:
                GameAudioSource.clip = StretchTutorial;
                break;
            case levelSounds.BounceTutorial:
                GameAudioSource.clip = BounceTutorial;
                break;
            case levelSounds.WaterTutorial:
                GameAudioSource.clip = WaterTutorial;
                break;
        }
        GameAudioSource.Play();
    }

    public void PlayUISound(UISounds sound)
    {
        switch (sound)
        {
            case UISounds.ButonEffect:
                UIAudioSource.clip = ButtonEffect;
                break;
        }
        
        UIAudioSource.Play();
    }
}

public enum UISounds
{
    ButonEffect
}

public enum playerSounds
{   
    TongueWrongDirection,
    ShootTongue,
    PlayerJump,
    PlayerSwim,
    EnterWater,
    PlayerDie
}

public enum levelSounds
{
    Menu,
    Forest,
    Desert,
    Snow,
    StretchTutorial,  
    BounceTutorial,
    WaterTutorial
}
