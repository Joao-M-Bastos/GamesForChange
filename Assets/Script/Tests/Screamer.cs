using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screamer : MonoBehaviour, IHitable
{
    public void TakePlayerHit()
    {
        Debug.Log("ACERTOU O CASTOR");
        gameObject.GetComponent<DialogueTrigger>().TriggerDialogueEvent();
    }
}
