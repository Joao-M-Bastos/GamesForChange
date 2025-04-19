using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueSeer : MonoBehaviour, IHitable
{
    public void TakePlayerHit()
    {
        //Esse ponto � importante para n�o acumular varios QuandoDialogoComecar
        DialogueManager.dialogueStart -= QuandoDialogoComecar;


        //Chama a fun��o QuandoDialogoComecar quando dialogo comecar
        DialogueManager.dialogueStart += QuandoDialogoComecar;
        Debug.Log("Estou esperando Dialogo");
    }

    private void QuandoDialogoComecar()
    {
        //Esse ponto � importante para n�o acumular varios QuandoDialogoTerminar
        DialogueManager.dialogueEnd -= QuandoDialogoTerminar;

        //Chama a fun��o QuandoDialogoTerminar quando dialogo terminar
        DialogueManager.dialogueEnd += QuandoDialogoTerminar;
        RealizarAcaoComeco();
        //Debug.Log("Dialogo Come�ou, eagora esperarei ele terminar");

        //OBRIGATORIO!!!! sempre retirar o QuandoDialogoComecar do "dialogueStart" desse jeito, apos usar
        DialogueManager.dialogueStart -= QuandoDialogoComecar;
    }

    public abstract void RealizarAcaoComeco();
    public abstract void RealizarAcaoFim();

    private void QuandoDialogoTerminar()
    {
        RealizarAcaoFim();
        //Debug.Log("Dialogo Terminou");

        //OBRIGATORIO!!!! sempre retirar o QuandoDialogoTerminar do "dialogueEnd" desse jeito, apos usar
        DialogueManager.dialogueEnd -= QuandoDialogoTerminar;
    }
}
