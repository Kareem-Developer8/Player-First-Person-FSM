using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State 
{
    protected Sophia rose;
    protected StateMachine stateMachine;
    protected string boolName;
    protected Rigidbody rb;
    protected float timer;
    protected float xInput;
    protected float zInput;
    protected bool triggerCalled;
    public State(Sophia Sofia, StateMachine stateMachine, string animBoolName)
    {
        this.rose = Sofia;
        this.stateMachine = stateMachine;
        this.boolName = animBoolName;
    }
    public virtual void Enter()
    {
        rose.animator.SetBool(boolName, true);
        rb = rose.rb;
    }
    public virtual void Update()
    {
        timer -= Time.deltaTime;
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");
    }
    public virtual void Exit()
    {
        rose.animator.SetBool(boolName, false);
    }
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }


}
