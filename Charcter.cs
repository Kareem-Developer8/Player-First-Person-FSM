using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Charcter : MonoBehaviour
{
    public Rigidbody rb;
    public Animator animator;
    public NavMeshAgent agent;
    public float runSpeed;
    public float walkSpeed;
    public float rotationSpeed;
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }
}
