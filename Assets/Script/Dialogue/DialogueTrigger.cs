using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DialogueTrigger : MonoBehaviour
{
    //[SerializeField] private int dialogueEventIndex;
    [field: SerializeField] public List<DialogueEvent> DialogueEvents { get; private set; }
    int currentDialogue;

    public void TriggerDialogueEvent()
    {
            DialogueManager.instance.StartDialogue(DialogueEvents[currentDialogue].Dialogues);
            currentDialogue++;

            if(currentDialogue == DialogueEvents.Count)
                currentDialogue = 0;
        
    }

    public void TriggerDialogueEvent(int dialogueID)
    {
        DialogueManager.instance.StartDialogue(DialogueEvents[dialogueID].Dialogues);
    }
}
