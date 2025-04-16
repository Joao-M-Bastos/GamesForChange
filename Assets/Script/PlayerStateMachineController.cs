using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachineController : MonoBehaviour
{
    PlayerState currentState;

    PlayerState freeState = new PlayerFreeMoveState();

    PlayerScpt player;

    private void Awake()
    {
        player = GetComponent<PlayerScpt>();
    }

    private void Start()
    {
        ChangeState(freeState);
    }

    public void ChangeState(PlayerState newState)
    {
        if (currentState != null)
            currentState.ExitState(this, player);

        currentState = newState;

        currentState.EnterState(this, player);
    }

    private void Update()
    {
        currentState.UpdateState(this, player);
    }

    void FixedUpdate()
    {
        currentState.FixedState(this, player);
    }
}
