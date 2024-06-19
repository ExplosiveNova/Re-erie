using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class EnemyAI : MonoBehaviourPunCallbacks
{
    public NavMeshAgent agent;

    public Transform pointA;
    public Transform pointB;
    public Animator animator;

    private Transform sonarSound;
    private Transform currentDestination;
    public float waitTime = 10f;
    private float waitTimer;

    public LayerMask whatIsGround, whatIsPlayer, sound, whatIsObstacle;

    public float health;

    //Roam
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Patrolling
    public bool betweenPoints = false;
    

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange, hearingRange;
    public bool playerInSightRange, playerInAttackRange, soundInRange;

    public bool onSight = false;
    private SonarSoundSpawn sonarSoundSpawn;
    private Transform player;
    private bool fetchPlayer1 = false;
    private bool isWalking = true;
    private bool isAttacking = false;


    private SpawnPlayers spawnPlayers;

    public AudioSource audioSource;
    public AudioClip[] footstepClips;
    public float footstepInterval = 0.5f; // Time between footstep sounds
    private float footstepTimer;

    public AudioSource attackAudioSource; // Separate AudioSource for attack sounds
    public AudioSource idleAudioSource;   // Separate AudioSource for idle sounds
    public AudioClip attackSound;         // Audio clip for attack sound
    public AudioClip[] idleSounds;        // Array of idle sounds

    private float idleSoundTimer;         // Timer for playing idle sounds
    private float idleSoundInterval = 5f; // Interval between idle sounds in seconds



private PlayerAudioManager playerAudioManager;

    private void Start()
    {   
        
        spawnPlayers = GameObject.Find("SpawnPlayers").GetComponent<SpawnPlayers>();
        isWalking = true;

        footstepTimer = footstepInterval;
        idleSoundTimer = idleSoundInterval;
        
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        currentDestination = pointA;
        waitTimer = 0f; // Initialize the wait timer

        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {   
        if (spawnPlayers.player1Join){
            onSight = HasLineOfSightToPlayer();
            Debug.Log("On sight activated!" + onSight);
        }
    }

    private void OnEnteredRoom(){
        player = GameObject.FindWithTag("Player").transform;
        Debug.Log("found player transform!");
        sonarSoundSpawn = player.gameObject.GetComponent<SonarSoundSpawn>();
        playerAudioManager = player.gameObject.GetComponent<PlayerAudioManager>(); 
    }


    private void Update()
    {   

        if (spawnPlayers.player1Join){
            if(!fetchPlayer1){
                OnEnteredRoom();
                fetchPlayer1 = true;
            }

            Debug.Log("eles andem aí");
            //Check for sight and attack range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            
            if (GameObject.Find("SonarSound(Clone)")) sonarSound = GameObject.Find("SonarSound(Clone)").transform;
                
            soundInRange = Physics.CheckSphere(transform.position, hearingRange, sound);

            Debug.Log("SoundInRange:" + soundInRange);

            if (playerInSightRange && !playerInAttackRange) {
                ChasePlayer();
                switchAnimation();
            }

            if (!playerInSightRange && soundInRange) ChaseSound();

            if (playerInAttackRange && playerInSightRange){
                AttackPlayer();
                switchAnimation();
            }

            if (!playerInSightRange && !playerInAttackRange && !soundInRange)
            {
                if (playerInSightRange) agent.speed = 2f;
                else agent.speed = 4f;

                if (betweenPoints)
                {
                    MoveBetweenPoints();
                }
                else
                {   
                    Debug.Log("Is patrolling!");
                    Patroling();
                    PlayIdleSound();
                    switchAnimation();
                }
                
            }

            else{
                idleSoundTimer = idleSoundInterval;
            }
            
       }

       ManageFootstepSounds();

    }

    //METODOS SOM

    private void ManageFootstepSounds()
    {
        // Check if the agent is moving
        if (agent.velocity.sqrMagnitude > 0.1f && agent.remainingDistance > agent.stoppingDistance)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0)
            {
                PlayFootstepSound();
                footstepTimer = footstepInterval; // Reset timer
            }
        }
        else
        {
            footstepTimer = footstepInterval; // Reset timer if not moving
        }
    }

    private void PlayIdleSound()
    {
        // Countdown the idle sound timer
        idleSoundTimer -= Time.deltaTime;

        if (idleSoundTimer <= 0)
        {
            // Play a random idle sound if available
            if (idleSounds.Length > 0 && idleAudioSource != null)
            {
                int randomIndex = Random.Range(0, idleSounds.Length);
                idleAudioSource.PlayOneShot(idleSounds[randomIndex]);
            }

            // Reset the idle sound timer with a random interval to add variety
            idleSoundTimer = Random.Range(idleSoundInterval / 2, idleSoundInterval * 1.5f);
        }
    }

    private void PlayFootstepSound()
    {
        if (footstepClips.Length > 0)
        {
            int clipIndex = Random.Range(0, footstepClips.Length);
            audioSource.PlayOneShot(footstepClips[clipIndex]);
        }
    }

    private void switchAnimation(){

        if (isWalking && !isAttacking){ //walking
            animator.SetBool("isWalking", true);
        }

        if (!isWalking && !isAttacking ){ //chasing player
            animator.SetBool("isWalking", false);
        }

        else { // attacking
            animator.SetBool("isAttacking", true);
        }
    }

    private void SwitchDestination()
    {
        if (currentDestination == pointA)
            currentDestination = pointB;
        else
            currentDestination = pointA;
        Debug.Log("Switching destination to: " + currentDestination.name);
    }
    private void Patroling()
    {   
        isAttacking = false;
        isWalking = true;

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

    private void MoveBetweenPoints()
    {
        if (currentDestination == null)
        {
            Debug.Log("wtf");
            return;
            
        }
            


        Debug.Log(currentDestination);
        agent.SetDestination(currentDestination.position);

        
        if (Vector3.Distance(transform.position, currentDestination.position) <= agent.stoppingDistance)
        {
            // Reached the destination point
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                if (waitTimer <= 0f)
                {
                    waitTimer = waitTime; // Reset the wait timer
                    SwitchDestination();  // Switch to the next destination
                }
                else
                {
                    waitTimer -= Time.deltaTime; // Count down the wait timer
                    agent.SetDestination(transform.position); // Stop moving during the wait
                }
            }
        } 
    }


    private void ChasePlayer()
    {
        Debug.Log("is chasing player");
        agent.speed = 4.5f;

        if (playerAudioManager != null)
        {
            playerAudioManager.PlayChasePlayerMusic();
        }

        if (playerInSightRange)
        {   
            isWalking = false; // is Chasing/ Running
            isAttacking = false;
            agent.SetDestination(player.position);
            sonarSoundSpawn.DestroySonar();
        }
        
    }

    private void ChaseSound()
    {   
        if (playerAudioManager != null)
        {
            playerAudioManager.PlayNormalMusic();
        }

        agent.speed = 4f;
        agent.SetDestination(sonarSound.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            if (attackAudioSource != null && attackSound != null)
            {
                attackAudioSource.PlayOneShot(attackSound);
            }

            isAttacking = true;
            isWalking = false;
            // Damage the player
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(20f); // Specify the damage amount
            }

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
        Debug.Log("Line of sight to player!");
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
