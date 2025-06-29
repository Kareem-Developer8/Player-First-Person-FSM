using UnityEngine;

public class DrawSwordState : State
{
    public DrawSwordState(Sophia Sofia, StateMachine stateMachine, string animBoolName) : base(Sofia, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        triggerCalled = false;
        if (rose.sword != null)
        {
            rose.sword.SetActive(true);
            rose.sword.transform.localPosition = new Vector3(0.0006f, .0002f, 0.00035f);
            rose.sword.transform.localRotation = Quaternion.Euler(-50f, -60f, -10f);
        }
        if (rose.weaponRig != null) rose.weaponRig.weight = 1;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            stateMachine.ChangeState(rose.FightIdle);
        }
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        triggerCalled = true;
    }

}