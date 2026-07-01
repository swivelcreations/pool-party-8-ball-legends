using UnityEngine;
using UnityEngine.AI;

public class NPCAnimator : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

    private void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetFloat("Speed", agent.velocity.magnitude);
    }
}