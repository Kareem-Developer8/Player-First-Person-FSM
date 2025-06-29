using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseIdleState : State
{
    public BaseIdleState(Sophia Sofia, StateMachine stateMachine, string animBoolName) : base(Sofia, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (GameManager.instance.IsPanelState() || GameManager.instance.IsPauseState())
            return;
        if (xInput != 0 || zInput != 0)
            stateMachine.ChangeState(rose.walkState);

        if (Input.GetKey(KeyCode.LeftShift) && zInput > 0)
            stateMachine.ChangeState(rose.runState);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            rose.hasDrawTheSwrod = true;
            stateMachine.ChangeState(rose.DrawSwordState);
        }
    }
}
