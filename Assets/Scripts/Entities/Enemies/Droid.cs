using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Droid : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Enemy>().Model.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.velocity.magnitude > 1f)
            animator.SetBool("Walking", true);
        else
            animator.SetBool("Walking", false);
    }
}
