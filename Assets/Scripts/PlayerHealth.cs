using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 2f;
    private float currentHealth;
    private SpawnPlayers spawnPlayers;
    private BatteryHandler batteryHandler;

    public Transform resetPosition; // The position to reset the player to

    void Start()
    {   
        spawnPlayers = GameObject.FindGameObjectWithTag("SpawnPlayers").GetComponent<SpawnPlayers>();
        batteryHandler = GameObject.FindGameObjectWithTag("BatteryHandler").GetComponent<BatteryHandler>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {   
            Debug.Log("Morri");
            ResetPlayerPosition();
        }
    }

    private void ResetPlayerPosition()
    {
        currentHealth = maxHealth; // Reset health to max
        spawnPlayers.Respawn();
        batteryHandler.Reset();
        Debug.Log("Player position reset.");
    }

    private void OnDrawGizmosSelected()
    {
        if (resetPosition != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(resetPosition.position, 1f);
        }
    }
}
