using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingIdleState : BaseIdleState
{
    public BreathingIdleState(Sophia Sofia, StateMachine stateMachine, string animBoolName) : base(Sofia, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timer = 12;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    if(timer<0)
            stateMachine.ChangeState(rose.lookAroundIdleState);
    }
}

