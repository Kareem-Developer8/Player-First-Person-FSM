using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseIdleState
{
    public IdleState(Sophia Sofia, StateMachine stateMachine, string animBoolName) : base(Sofia, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timer = 8;
        rose.hasDrawTheSwrod = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update(); 
        if (timer < 0) 
            stateMachine.ChangeState(rose.breathingIdleState);

    }
}
