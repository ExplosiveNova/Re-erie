using UnityEngine;

public class SonarSoundSpawn : MonoBehaviour
{
    public GameObject player; // Reference to the player object
    public GameObject sonarSoundPrefab; // Prefab for the SonarSound object
    public float spawnHeightOffset = 2f; // Height offset for the spawn position
    private GameObject currentSonarSound; // Reference to the current SonarSound object

    void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Spawn the SonarSound at the player's position
            SpawnSonarSound();
        }
    }

    void SpawnSonarSound()
    {
        // If there is already a SonarSound object, destroy it
        if (currentSonarSound != null)
        {
            DestroySonar();
        }

        // Calculate the spawn position with an offset above the player
        Vector3 spawnPosition = player.transform.position + Vector3.up * spawnHeightOffset;
        // Instantiate a new SonarSound object at the player's position and set the currentSonarSound reference
        currentSonarSound = Instantiate(sonarSoundPrefab, spawnPosition, Quaternion.identity);
    }

   public void DestroySonar()
    {
        Destroy(currentSonarSound);
    }
}
