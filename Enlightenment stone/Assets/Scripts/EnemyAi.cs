using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public Transform target;

    public int enemyCount;

    public Spawner spawnScript;


    //patroling

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;


    //Attacking


    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public void Awake()
    {
        player = FindObjectOfType<PlayerScript>().transform;
        target = FindObjectOfType<PlayerScript>().transform;
        agent = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        //check for sight
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    public void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    public void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        walkPointSet = true;
    }

    public void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    public void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(target);
    }
    public void OnDestroy()
    {
        spawnScript.enemyCount--;
    }

}
