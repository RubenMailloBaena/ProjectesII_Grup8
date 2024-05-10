using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadAnimations : MonoBehaviour
{
    private static HeadAnimations instance;
    public static HeadAnimations Instance
    {
        get
        {
            if (instance == null)
                instance = FindAnyObjectByType<HeadAnimations>();
            return instance;
        }
    }

    [SerializeField] private Animator animator;
    private string openName = "Obrir";
    private string closeName = "Tancar";
    private string currentState;


    public void ChangeAnimation(HeadAnim newAnim)
    {

        switch (newAnim)
        {
            case HeadAnim.Open:
                ChangeAnimationState(openName); break;

            case HeadAnim.Close:
                ChangeAnimationState(closeName); break;

            default: throw new System.Exception("error");
        }
    }
    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
}

public enum HeadAnim
{
    Open,
    Close
}