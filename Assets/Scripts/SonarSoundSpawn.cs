using UnityEngine;

public class SonarSoundSpawn : MonoBehaviour
{

    public GameObject sonarSoundPrefab; // Prefab for the SonarSound object
    public float spawnHeightOffset = 2f; // Height offset for the spawn position
    private GameObject currentSonarSound; // Reference to the current SonarSound object
    private GameObject player1;

    public void SpawnSonarSound()
    {   
        player1 = GameObject.FindGameObjectWithTag("Player");

        if (currentSonarSound != null)
        {
            DestroySonar();
        }

        // Calculate the spawn position with an offset above the player
        Vector3 spawnPosition = player1.transform.position + Vector3.up * spawnHeightOffset;
        // Instantiate a new SonarSound object at the player's position and set the currentSonarSound reference
        currentSonarSound = Instantiate(sonarSoundPrefab, spawnPosition, Quaternion.identity);
        GetComponent<PlayerAudioManager>().PlaySonarSound();

        Debug.Log("Emitting Sonar Sound");
    }

   public void DestroySonar()
    {
        Destroy(currentSonarSound);
    }



}
