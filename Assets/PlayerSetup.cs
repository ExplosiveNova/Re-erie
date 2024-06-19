using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSetup : MonoBehaviour
{
    public GameObject secondPlayerPrefab; // Assign the second player prefab in the inspector
    public Key addPlayerKey = Key.M; // Default key is 'M', can be changed in the inspector

    void Update()
    {
        // Check if the configured key is pressed
        if (Keyboard.current[addPlayerKey].wasPressedThisFrame)
        {
            AddSecondPlayer();
        }
    }

    void AddSecondPlayer()
    {
        if (secondPlayerPrefab == null)
        {
            Debug.LogError("Second player prefab is not assigned.");
            return;
        }

        // Instantiate the second player as a child of this game object
        GameObject secondPlayer = Instantiate(secondPlayerPrefab, transform);

        // Ensure the second player has a Camera and PlayerInput component
        PlayerInput playerInput = secondPlayer.GetComponent<PlayerInput>();
        Camera playerCamera = secondPlayer.GetComponentInChildren<Camera>();

        if (playerInput == null || playerCamera == null)
        {
            Debug.LogError("Second player prefab must have PlayerInput and Camera components.");
            return;
        }

        // Set the PlayerInput camera
        playerInput.camera = playerCamera;
    }
}