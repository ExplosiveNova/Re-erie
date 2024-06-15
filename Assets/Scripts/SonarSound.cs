using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarSound : MonoBehaviour
{

    
    void OnTriggerEnter(Collider other)
    {
        // Check if the object collided with has the "Enemy" tag
        if (other.CompareTag("Enemy"))
        {
            // Destroy this SonarSound object
            Destroy(gameObject);
        }
    }
}

