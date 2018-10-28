using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicPlayer : MonoBehaviour {
    //The cinematice animator
    public Animator CinematicAnimator;
    //Root UI object
    public GameObject UI;
    
    private bool playedIntro = false;

    void Awake()
    {
        //keep object on scene change
        DontDestroyOnLoad(gameObject);
        //destroy any duplicates
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        //if played intro is false then play the intro
        if (!playedIntro)
        {
            PlayCinematic("Intro");
            playedIntro = true;
        }
    }
    public void PlayCinematic(string trigger)
    {
        //hide UI
        UI.SetActive(false);
        //play animation with trigger
        CinematicAnimator.SetTrigger(trigger);
        Invoke("ResetUI", GetAnimationLength(trigger));
    }

    public void ResetUI()
    {
        //show UI
        UI.SetActive(true);
    }

    //Get the animation length of an animator state by name
    public float GetAnimationLength(string animationName)
    {
        float time = 0;
        RuntimeAnimatorController animatorController = CinematicAnimator.runtimeAnimatorController;
        for (int i = 0; i < animatorController.animationClips.Length; i++)           
        {
            if (animatorController.animationClips[i].name == animationName)  
            {
                time = animatorController.animationClips[i].length;
            }
        }
        return time;
    }
}
