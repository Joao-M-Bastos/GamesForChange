using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private float textSpeed;
    private DialogueBox _dialogueBox;
    //[field: SerializeField] public List<DialogueEvent> DialogueEvents { get; private set; }

    private Queue<Dialogue> dialoguesQueue;
    private Queue<string> sentencesQueue;
    //[SerializeField] private CharactersData[] characters;
    private Coroutine sentenceCoroutine;
    private string lastSentence;

    bool inDialog;

    public bool InDialog => inDialog;

    public delegate void DialogueEnd();
    public static DialogueEnd dialogueEnd;

    public delegate void DialogueStart();
    public static DialogueStart dialogueStart;

    public static DialogueManager instance;

    private void Awake()
    {
        instance = this;
        //dialogueBoxImage = dialogueBoxAnimator.GetComponent<Image>();
    }
    void Start()
    {
        dialoguesQueue = new Queue<Dialogue>();
        sentencesQueue = new Queue<string>();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TryDisplayNextSentence();
        }
    }

    public void StartDialogue(int dialogueID)
    {
        //StartDialogue(DialogueEvents[dialogueID].Dialogues);
    }

    public void StartDialogue(List<Dialogue> dialogues)
    {
        dialoguesQueue.Clear();
        sentencesQueue.Clear();

        foreach (Dialogue dialogue in dialogues)
        {
            dialoguesQueue.Enqueue(dialogue);
        }

        inDialog = true;

        dialogueStart?.Invoke();


        TryDisplayNextDialogue();
    }
     
    public void DisplayNextDialogue()
    {
        bool hasDialog = dialoguesQueue.TryPeek(out Dialogue nextDialogue);
        if (!hasDialog) return;

        foreach (string sentence in nextDialogue.Sentences) 
        {
            sentencesQueue.Enqueue(sentence);
        }

        SetDialogueConfig(nextDialogue);

        _dialogueBox.CharacterNameText.text = nextDialogue.CharacterData.Name;

        dialoguesQueue.Dequeue();
        TryDisplayNextSentence();
    }

    private void SetDialogueConfig(Dialogue dialogue)
    {
        if(_dialogueBox != null)
            _dialogueBox.DialogueText.text = "";
        
        _dialogueBox = dialogue.DialogueBox;

        _dialogueBox.DialogueText.font = dialogue.Font;
        _dialogueBox.DialogueText.fontStyle = dialogue.FontStyle;
        _dialogueBox.DialogueText.color = dialogue.FontColor;
        //dialogueBoxImage.sprite = dialogue.BackgroundImage;

        _dialogueBox.DialogueBoxAnimator.SetBool("IsOpen", true);
    }

    public void TryDisplayNextSentence()
    {
        if (!inDialog)
            return;

        if (sentencesQueue.Count == 0 && sentenceCoroutine == null)
        {
            TryDisplayNextDialogue();
            return;
        }

        if (sentenceCoroutine != null)
        {
            StopAllCoroutines();
            _dialogueBox.DialogueText.text = lastSentence;
            sentenceCoroutine = null;
            return;
        }

        string sentence = sentencesQueue.Dequeue();
        lastSentence = sentence;
        DisplayNextSentence(sentence);
    }
    public void DisplayNextSentence(string sentence)
    {
        StopAllCoroutines();
        sentenceCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    public void TryDisplayNextDialogue()
    {
        if (dialoguesQueue.Count != 0)
        {
            DisplayNextDialogue();
        }

        else EndDialogue();
    }

    private IEnumerator TypeSentence(string sentence)
    {
        _dialogueBox.DialogueText.text = "";

        foreach (char letter in  sentence.ToCharArray())
        {
            _dialogueBox.DialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        sentenceCoroutine = null;
    }

    public void EndDialogue()
    {
        dialogueEnd?.Invoke();
        _dialogueBox.DialogueBoxAnimator.SetBool("IsOpen", false);
        inDialog = false;
    }
}