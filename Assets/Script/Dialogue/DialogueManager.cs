using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private float textSpeed;
    [SerializeField] private Text characterNameText;
    [SerializeField] private SpriteRenderer characterSpriteRenderer;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Animator dialogueBoxAnimator;
    [field: SerializeField] public List<DialogueEvent> DialogueEvents { get; private set; }

    private Queue<Dialogue> dialoguesQueue;
    private Queue<string> sentencesQueue;
    [SerializeField] private CharactersData[] characters;
    private Image dialogueBoxImage;
    private Coroutine sentenceCoroutine;
    private string lastSentence;

    bool inDialog;

    public bool InDialog => inDialog;

    public delegate void DialogueEnd();
    public static DialogueEnd dialogueEnd;

    public delegate void DialogueStart();
    public static DialogueStart dialogueStart;

    private void Awake()
    {
        dialogueBoxImage = dialogueBoxAnimator.GetComponent<Image>();
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

    public void StartDialogue(int dialogueID, int cameraWork)
    {
        StartDialogue(DialogueEvents[dialogueID].Dialogues, cameraWork);
    }

    public void StartDialogue(List<Dialogue> dialogues, int cameraWork)
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

        characterNameText.text = characterNamesDictionary[nextDialogue.CharacterType];
        characterSpriteRenderer.sprite = characterSpritesDictionary[nextDialogue.CharacterType];

        dialoguesQueue.Dequeue();
        TryDisplayNextSentence();
    }

    private void SetDialogueConfig(Dialogue dialogue)
    {
        dialogueBoxImage.sprite = dialogue.BackgroundImage;
        dialogueText.font = dialogue.Font;
        dialogueText.fontStyle = dialogue.FontStyle;
        dialogueText.color = dialogue.FontColor;
        dialogueBoxImage.sprite = dialogue.BackgroundImage;

        dialogueBoxAnimator.SetBool("IsOpen", true);
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
            dialogueText.text = lastSentence;
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
        dialogueText.text = "";

        foreach (char letter in  sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        sentenceCoroutine = null;
    }

    public void EndDialogue()
    {
        dialogueEnd?.Invoke();
        dialogueBoxAnimator.SetBool("IsOpen", false);
        inDialog = false;
    }
}