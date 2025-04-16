using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCClass : MonoBehaviour
{
    public bool need1;
    public bool hasSecondNeed;
    public bool need2;

    [Header("Just a short description. Doesn't affect anything.")]
    public string needOne;
    public string needTwo;
    public string reward;

    [Header("Animator")]
    public Animator animator;

    [Header("State Stuff")]
    public NPCState currentState;
    
    public enum NPCState
    {
        Idle,
        Sad,
        OnReward
    }

    public virtual void Start()
    {
        currentState = NPCState.Sad;

        need2 = !hasSecondNeed;

        animator = GetComponent<Animator>();
    }

    public virtual void Update()
    {
        switch(currentState)
        {
            case NPCState.Idle:
                HandleIdle();
                break;
            case NPCState.Sad:
                HandleSad();
                break;
            case NPCState.OnReward:
                HandleOnReward();
                break;
        }
    }

    public virtual void HandleIdle()
    {
        animator.SetTrigger("Idle");
    }

    public virtual void HandleSad()
    {
        animator.SetTrigger("Sad");
    }
    public virtual void HandleOnReward()
    {
        animator.SetTrigger("OnReward");
    }


    public virtual void NeedSatisfied()
    {
        if(need1 == false)
        {
            need1 = true;
        }

        if (need2 == false)
        {
            need2 = true;
        }
    }

    public virtual void QuestCompleted()
    {
        if(need1 && need2)
        {
            OnReward();
            currentState = NPCState.OnReward;
        }
    }

    public abstract void OnReward();
}