using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private Text characterNameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Animator dialogueBoxAnimator;

    public Text CharacterNameText => characterNameText;
    public TMP_Text DialogueText => dialogueText;
    public Animator DialogueBoxAnimator => dialogueBoxAnimator;
}
