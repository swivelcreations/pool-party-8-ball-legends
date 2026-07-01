using UnityEngine;

public class WanderNPC : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform[] waypoints;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float rotationSpeed = 5f;
    public float stoppingDistance = 0.5f;
    public float minWaitTime = 2f;
    public float maxWaitTime = 5f;

    private Transform currentTarget;
    private float waitTimer;
    private bool waiting;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

        if (anim != null)
            anim.SetFloat("Speed", 1f);

        SetNewDestination();
    }

    private void Update()
    {
        if (currentTarget == null)
            return;

        if (waiting)
        {
            if (anim != null)
                anim.SetFloat("Speed", 0f);

            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0)
            {
                waiting = false;

                if (anim != null)
                    anim.SetFloat("Speed", 1f);

                SetNewDestination();
            }

            return;
        }

        Vector3 direction = currentTarget.position - transform.position;
        direction.y = 0;

        if (direction.magnitude <= stoppingDistance)
        {
            waiting = true;
            waitTimer = Random.Range(minWaitTime, maxWaitTime);
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime);

        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private void SetNewDestination()
    {
        if (waypoints.Length == 0)
            return;

        Transform nextTarget;

        do
        {
            nextTarget = waypoints[Random.Range(0, waypoints.Length)];
        }
        while (nextTarget == currentTarget && waypoints.Length > 1);

        currentTarget = nextTarget;
    }
}