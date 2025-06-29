using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StateMachine 
{
    public State currentState;
    public void Instatiate(State _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }
    public void ChangeState(State _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
