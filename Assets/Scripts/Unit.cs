using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public Transform target;

    private Animator animator;
    private NavMeshAgent agent;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

 
    void Update()
    {
        
        if(target != null)
        {
            agent.SetDestination(target.position);
        }
        Animate();
    }

    protected virtual void Animate()
    {
        var speedVecor = agent.velocity;
        speedVecor.y = 0;
        float speed = speedVecor.magnitude;
        animator.SetFloat("Speed", speed);
    }

}
