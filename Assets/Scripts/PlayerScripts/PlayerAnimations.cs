using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private static PlayerAnimations instance;
    public static PlayerAnimations Instance
    {
        get { 
            if (instance == null)
                instance = FindAnyObjectByType<PlayerAnimations>();
            return instance;
        }
    }

    [SerializeField] private Animator animator;
    [SerializeField] private Animator head;
    private string idleName="PlayerIdle";
    private string jumpname = "PlayerJump";
    private string walkName = "PlayerWalk";
    private string shootName = "PlayerShoot";
    private string fallName = "PlayerFall";
    private string swimName = "PlayerSwim";
    private string swimDownName = "PlayerSwimDown";
    private string dieName = "PlayerDie";
    private string openName = "Open";
    private string closeName = "Close";
    private string idleHeadName = "Idle";
    private string currentState;
    private string headstate;


    public void ChangeAnimation(PlayerAnim newAnim)
    {

        switch (newAnim) {
            case PlayerAnim.Idle: 
                ChangeAnimationState(idleName); break;

            case PlayerAnim.Jump:
                ChangeAnimationState(jumpname); break;

            case PlayerAnim.Walk:
                ChangeAnimationState(walkName); break;

            case PlayerAnim.Shoot:
                ChangeAnimationState(shootName); break;

            case PlayerAnim.Fall:
                ChangeAnimationState(fallName); break;

            case PlayerAnim.Swim:
                ChangeAnimationState(swimName); break;

            case PlayerAnim.SwimDown:
                ChangeAnimationState(swimDownName); break;

            case PlayerAnim.Die:
                ChangeAnimationState(dieName); break;

            default: throw new System.Exception("error"); 
        }
    }
    
    public void ChangeHeadAnimation(HeadAnim newAnim)
    {

        switch (newAnim)
        {

            case HeadAnim.Open:
                ChangeHeadAnimationState(openName); break;

            case HeadAnim.Close:
                ChangeHeadAnimationState(closeName); break;
            
            case HeadAnim.IdleHead:
                ChangeHeadAnimationState(idleHeadName);
                break;
            

            default: throw new System.Exception("error");
        }
    }
    
 
    
    private void ChangeHeadAnimationState(string newState)
    {
        if (headstate == newState) return;
        head.Play(newState);
        headstate = newState;
    }
    

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
}

public enum PlayerAnim
{
    Idle,
    Jump,
    Walk,
    Shoot,
    Fall,
    Swim,
    SwimDown,
    Die
}
public enum HeadAnim
{
    Open, 
    Close,
    IdleHead
}
