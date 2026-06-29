using UnityEngine;
using UnityEngine.AI;

public class WanderNPC : MonoBehaviour
{
    [Header("Movement")]
    public float wanderRadius = 20f;
    public float minWaitTime = 2f;
    public float maxWaitTime = 5f;

    private NavMeshAgent agent;
    private float waitTimer;
    private bool waiting;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetNewDestination();
    }

    private void Update()
    {
        if (waiting)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0)
            {
                waiting = false;
                SetNewDestination();
            }
        }
        else
        {
            if (!agent.pathPending &&
                agent.remainingDistance <= agent.stoppingDistance)
            {
                waiting = true;
                waitTimer = Random.Range(minWaitTime, maxWaitTime);
            }
        }
    }

    private void SetNewDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection,
                                   out hit,
                                   wanderRadius,
                                   NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}