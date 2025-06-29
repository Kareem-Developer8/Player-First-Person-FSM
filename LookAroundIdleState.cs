using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAroundIdleState : BaseIdleState
{
    public LookAroundIdleState(Sophia Sofia, StateMachine stateMachine, string animBoolName) : base(Sofia, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timer = 18;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(timer<0)
            stateMachine.ChangeState(rose.idleState);
    }
}
