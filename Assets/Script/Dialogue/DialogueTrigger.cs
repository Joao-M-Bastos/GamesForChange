using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DialogueTrigger : MonoBehaviour
{
    //[SerializeField] private int dialogueEventIndex;
    [field: SerializeField] public List<DialogueEvent> DialogueEvents { get; private set; }
    int currentDialogue;

    public DialogueManager DialogueManagerToTrigger;

    public void TriggerDialogueEvent()
    {
            DialogueManagerToTrigger.StartDialogue(DialogueEvents[currentDialogue].Dialogues);
            currentDialogue++;

            if(currentDialogue == DialogueEvents.Count)
                currentDialogue = 0;
        
    }
}
