using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private Transform player1;
    private Transform player2;
    private NavMeshAgent navMeshAgent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        // Enable obstacle avoidance for better dynamic obstacle handling
        navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        //dumb way of doing this
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        player1 = players[0].transform;
        player2 = players[1].transform;
    }

    // Update is called once per frame
    void Update()
    {
        //find closest player and move towards them
        float player1Dist = Vector3.Distance(transform.position, player1.position);
        float player2Dist = Vector3.Distance(transform.position, player2.position);
        if (player1 != null && player2 != null)
        {
            navMeshAgent.SetDestination(player1Dist < player2Dist ? player1.position : player2.position);
        }
    }
}
