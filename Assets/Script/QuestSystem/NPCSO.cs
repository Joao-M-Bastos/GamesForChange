using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC_SO")]
public class NPCSO : ScriptableObject
{
    [SerializeField] bool needs1;
    [SerializeField] bool needs2;

    [SerializeField] bool reward;
}
