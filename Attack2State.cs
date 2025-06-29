using UnityEngine;

public class Attack2State : State
{
    private float hitTime = 0.4f; // Adjust per animation
    private bool hasPerformedAttack;
    Vector3 originalCameraPosition; 
    public Attack2State(Sophia Sofia, StateMachine stateMachine, string animBoolName) : base(Sofia, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rose.agent.isStopped = true; // Add this
        triggerCalled = false;
        hasPerformedAttack = false;
       
            originalCameraPosition = rose.cameraTransform.localPosition;
            rose.MoveCameraForward();
        
    }

    public override void Exit()
    {
        rose.MoveCameraBack();

        rose.agent.isStopped = false;
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        AnimatorStateInfo stateInfo = rose.animator.GetCurrentAnimatorStateInfo(0);
        if (!hasPerformedAttack &&
            stateInfo.normalizedTime >= hitTime &&
            stateInfo.IsName(boolName))
        {
            rose.PerformAttack();
            hasPerformedAttack = true;
        }
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