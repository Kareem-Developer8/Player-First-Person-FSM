using System.Collections;
using UnityEngine;

public class WalkState : State
{
    public WalkState(Sophia Sofia, StateMachine stateMachine, string animBoolName) : base(Sofia, stateMachine, animBoolName)
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
        if (rose.canMove)
        {
            // Calculate camera-relative movement like Rose does
            Vector3 moveDirection = GetCameraRelativeMovement();

            if (moveDirection.magnitude >= 0.1f)
            {
                rose.agent.Move(moveDirection * rose.walkSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.LeftShift) && zInput > 0)
                stateMachine.ChangeState(rose.runState);

            if (Mathf.Approximately(xInput, 0) && Mathf.Approximately(zInput, 0))
            {
                if (rose.hasDrawTheSwrod)
                    stateMachine.ChangeState(rose.FightIdle);
                else
                    stateMachine.ChangeState(rose.idleState);
            }
        }
    }
    private Vector3 GetCameraRelativeMovement()
    {
        if (rose.cameraTransform == null)
            return Vector3.zero;

        Vector3 forward = rose.cameraTransform.forward;
        Vector3 right = rose.cameraTransform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        return (forward * zInput + right * xInput).normalized;
    }
}

