using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    private Transform sonarSound;

    public LayerMask whatIsGround, whatIsPlayer, sound, whatIsObstacle;

    public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange, hearingRange;
    public bool playerInSightRange, playerInAttackRange, soundInRange;

    public bool onSight = false;
    private SonarSoundSpawn sonarSoundSpawn;

    private void Start()
    {
        sonarSoundSpawn = player.gameObject.GetComponent<SonarSoundSpawn>();
    }
    private void Awake()
    {
        player = GameObject.Find("PlayerCapsule").transform;
        agent = GetComponent<NavMeshAgent>();
        
    }

    private void FixedUpdate()
    {
        onSight = HasLineOfSightToPlayer();
    }

    private void Update()
    {
        
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        
        if (GameObject.Find("SonarSound(Clone)")) sonarSound = GameObject.Find("SonarSound(Clone)").transform;
            
        soundInRange = Physics.CheckSphere(transform.position, hearingRange, sound);

        Debug.Log("SoundInRange:" + soundInRange);

        if (!playerInSightRange && !playerInAttackRange && !soundInRange) Patroling();
        
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();

        if (!onSight && soundInRange) ChaseSound();

        if (playerInAttackRange && playerInSightRange) AttackPlayer();
        
       
    }



    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void GoToSpot()
    {
        //TO DO
        //agent.SetDestination();
    }
    private void ChasePlayer()
    {

       if (onSight)
        {
            agent.SetDestination(player.position);
            sonarSoundSpawn.DestroySonar();
        }
        
    }

    private void ChaseSound()
    {
        agent.SetDestination(sonarSound.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hearingRange);
    }

    private bool HasLineOfSightToPlayer()
    {
        
        RaycastHit hit;
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        // Adjust the raycast start position to the enemy's eye level or a suitable point

        Vector3 startPosition = transform.position + Vector3.up * 1.5f; // Assuming 1.5 units above the ground

        // Debug line to visualize the ray
        Debug.DrawRay(startPosition, directionToPlayer * sightRange, Color.red);

        // Cast a ray from the enemy to the player
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, sightRange))
        {
            // Check if the ray hit the player
            //Debug.Log($"Ray hit: {hit.transform.name}" + " " + hit.transform.gameObject);
            //Debug.Log(hit.transform.gameObject.tag);
            
            if (hit.transform.CompareTag("Player") || hit.transform.root.CompareTag("Player"))
            {
                //Debug.Log("Player is in line of sight");
                return true;
            }
            // Check if the ray hit an obstacle
            else 
            {
                //Debug.Log("Hit an obstacle");
                return false;
            }
        }
        return false;
    }
}
