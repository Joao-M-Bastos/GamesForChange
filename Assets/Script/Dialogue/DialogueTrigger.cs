using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private int dialogueEventIndex;

    public DialogueManager DialogueManagerToTrigger;

    public void TriggerDialogueEvent()
    {
        if (dialogueEventIndex >= 0)
            DialogueManagerToTrigger.StartDialogue(dialogueEventIndex);
    }
}
