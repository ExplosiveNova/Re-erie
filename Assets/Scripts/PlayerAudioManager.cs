using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public AudioSource normalMusicAudioSource;
    public AudioSource chasePlayerAudioSource;
    public AudioClip normalMusic;
    public AudioClip chasePlayerMusic;

    private float chaseMusicTimer = 0f;
    private float chaseMusicCooldown = 5f; // Cooldown in seconds before changing the music

    private bool isChaseMusicPlaying = false;

    public AudioSource footstepAudioSource; // Audio source for footsteps
    public AudioClip[] footstepClips; // Array of footstep sounds
    public float footstepInterval = 0.5f; // Time between footstep sounds
    private float footstepTimer;
    private CharacterController characterController; // Reference to the CharacterController

    public AudioSource sonarSource;
    public AudioClip sonarsound;
    public AudioSource batterySource;
    public AudioClip batterySound;

    private void Start()
    {
        // Set up the audio sources with the appropriate clips
        normalMusicAudioSource.clip = normalMusic;
        chasePlayerAudioSource.clip = chasePlayerMusic;

        sonarSource.clip = sonarsound;
        batterySource.clip = batterySound;

        // Start playing the normal music by default
        normalMusicAudioSource.Play();

        // Initialize the footstep timer
        footstepTimer = footstepInterval;
        // Get the CharacterController component
        characterController = GetComponent<CharacterController>();


    }

    private void Update()
    {
        // Update the music timer

        if (chaseMusicTimer > 0)
        {
            chaseMusicTimer -= Time.deltaTime;
        }

        if (chaseMusicTimer<= 0){
            PlayNormalMusic();
        }

        ManageFootstepSounds();
    }

    public void PlaySonarSound()
    {
        if (!sonarSource.isPlaying)
        {
            sonarSource.Play();
        }
    }

    public void PlayBatterySound()
    {
        if (!sonarSource.isPlaying)
        {
            batterySource.Play();
        }
    }


    public void PlayChasePlayerMusic()
    {
        if (isChaseMusicPlaying && chasePlayerAudioSource.isPlaying)
        {
            chaseMusicTimer = chaseMusicCooldown; // Reset the cooldown timer
            return;// If chase music is already playing, do nothing
        }
        else
        {
            
            if (chaseMusicTimer <= 0)
            {
                // Otherwise, switch to chase music 
                chaseMusicTimer = chaseMusicCooldown; // Reset the cooldown timer
                StopCurrentMusic();
                chasePlayerAudioSource.Play();
                isChaseMusicPlaying = true;
                Debug.Log("Chase music started.");

            }
        }

    }
    public void PlayNormalMusic()
    {
        if (chaseMusicTimer <= 0 && isChaseMusicPlaying)
        {
            StopCurrentMusic();
            normalMusicAudioSource.Play();
            isChaseMusicPlaying = false;
            //musicTimer = musicCooldown; // Reset the cooldown timer
            Debug.Log("Normal music started.");
        }
    }

    private void StopCurrentMusic()
    {
        if (normalMusicAudioSource.isPlaying)
        {
            normalMusicAudioSource.Stop();
        }
        if (chasePlayerAudioSource.isPlaying)
        {
            chasePlayerAudioSource.Stop();
        }
    }

    private void ManageFootstepSounds()
    {
        if (characterController == null)
            return;

        // Check if the player is moving and is grounded
        if (characterController.isGrounded && characterController.velocity.magnitude > 0.1f)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0)
            {
                PlayFootstepSound();
                footstepTimer = footstepInterval; // Reset the timer
            }
        }
        else
        {
            footstepTimer = footstepInterval; // Reset the timer if not moving
        }
    }

    private void PlayFootstepSound()
    {
        if (footstepClips.Length > 0)
        {
            int clipIndex = Random.Range(0, footstepClips.Length);
            footstepAudioSource.PlayOneShot(footstepClips[clipIndex]);
        }
    }
}
