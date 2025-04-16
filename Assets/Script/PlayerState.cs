using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerState
{
    public void EnterState(PlayerStateMachine stateMachine, PlayerScpt player);
    public void ExitState(PlayerStateMachine stateMachine, PlayerScpt player);
    public void UpdateState(PlayerStateMachine stateMachine, PlayerScpt player);
    public void FixedState(PlayerStateMachine stateMachine, PlayerScpt player);

}
