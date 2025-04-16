using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screamer : MonoBehaviour, IHitable
{
    public void TakePlayerHit()
    {
        Debug.Log("OUCH!");
    }
}
