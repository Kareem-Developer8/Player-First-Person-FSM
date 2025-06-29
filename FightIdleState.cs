using UnityEngine;

public class FightIdleState : State
{
    public FightIdleState(Sophia Sofia, StateMachine stateMachine, string animBoolName) : base(Sofia, stateMachine, animBoolName)
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
            State randomAttack = rose.GetRandomAttackState();
            stateMachine.ChangeState(randomAttack);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (rose.sword != null) rose.sword.SetActive(false);
            if (rose.weaponRig != null) rose.weaponRig.weight = 0;

            rose.hasDrawTheSwrod = false;
            stateMachine.ChangeState(rose.idleState);
        }
    }
}
