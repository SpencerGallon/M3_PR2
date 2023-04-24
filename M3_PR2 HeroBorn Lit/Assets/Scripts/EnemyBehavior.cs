using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform patrolRoute;
    [SerializeField] private int lives = 3;

    private readonly List<Transform> locations = new List<Transform>();
    private int locationIndex;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        InitializePatrolRoute();
    }

    private void InitializePatrolRoute()
    {
        foreach (Transform child in patrolRoute)
        {
            locations.Add(child);
        }
    }

    private void MoveToNextPatrolLocation()
    {
        if (locations.Count == 0) return;
        agent.SetDestination(locations[locationIndex].position);
        locationIndex = (locationIndex + 1) % locations.Count;
    }

    private void Update()
    {
        if (agent.remainingDistance < 0.2f && !agent.pathPending)
        {
            MoveToNextPatrolLocation();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            agent.SetDestination(player.position);
            Debug.Log("Player detected - attack!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player out of range, resume patrol");
            MoveToNextPatrolLocation();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            lives--;
            Debug.Log("Critical hit!");
            if (lives <= 0)
            {
                Debug.Log("Enemy down.");
                Destroy(gameObject);
            }
        }
    }
}
