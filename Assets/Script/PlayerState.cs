using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerState
{
    public void EnterState(PlayerStateMachineController stateMachine, PlayerScpt player);
    public void ExitState(PlayerStateMachineController stateMachine, PlayerScpt player);
    public void UpdateState(PlayerStateMachineController stateMachine, PlayerScpt player);
    public void FixedState(PlayerStateMachineController stateMachine, PlayerScpt player);

}
