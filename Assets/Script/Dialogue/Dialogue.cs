using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Dialogue 
{
    [field:SerializeField] public CharactersData CharacterData { get; private set; }

    //[field:SerializeField] public Sprite BackgroundImage { get; private set; }

    [field: SerializeField] public DialogueBox DialogueBox { get; private set; }
    [field:SerializeField] public TMP_FontAsset Font { get; private set; }
    [field:SerializeField] public Color FontColor { get; private set; }
    [field:SerializeField] public FontStyles FontStyle { get; private set; }

    [field:TextArea(1, 2)]
    [field: SerializeField] public string[] Sentences { get; private set; }
}
