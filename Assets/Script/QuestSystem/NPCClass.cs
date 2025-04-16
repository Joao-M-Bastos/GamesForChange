using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCClass : MonoBehaviour
{
    public bool needs1;
    [Header("If they have a second need, please toggle 'hasSecondNeed'")]
    public bool hasSecondNeed;
    public bool needs2;

    public void Start()
    {
        if(hasSecondNeed == false)
        {
            needs2 = true;
        }
    }

    public void NeedSatisfied()
    {
        if(needs1 == false)
        {
            needs1 = true;
        }

        if (hasSecondNeed == true && needs2 == false)
        {
            needs2 = true;
        }
    }

    public void QuestCompleted()
    {
        if(needs1 && needs2)
        {
            OnReward();
        }
    }

    public abstract void OnReward();
}