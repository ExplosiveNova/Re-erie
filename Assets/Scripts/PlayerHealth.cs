using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 2f;
    private float currentHealth;

    public Transform resetPosition; // The position to reset the player to

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            ResetPlayerPosition();
        }
    }

    private void ResetPlayerPosition()
    {
        currentHealth = maxHealth; // Reset health to max
        transform.position = resetPosition.position; // Relocate player
        transform.rotation = resetPosition.rotation; // Reset player rotation if needed
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
