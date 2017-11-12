using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Collider))]
public class Key : MonoBehaviour 
{
    // Needed Information from Scene
    public MeshRenderer keyMeshRenderer;    // MeshRenderer component
    public Collider keyCollider;            // Collider component
    public Animator keyAnimator;           // Animator component
    public AudioSource soundSource;         // AudioSource to use
    public Door keyDoor;                    // Door to unlock
    public GameObject keyObject;            // Key GameObject
    public GameObject keyGlow;              // Key Glow GameObject
    public GameObject keySpotLight;         // Ket Spotlight GameObject
    public ParticleSystem keyParticles;     // Key Poof

    // Unity Modifiable Variables
    public int keyDestroDelay;              // Delay before destroying key

    // Method Variables
    int coinCount;
    bool hasCoins;
    bool triggerKey;

    void Start() {
        // Start coin count
        coinCount = 0;
        hasCoins = false;

        // Set up the key paramenters
        triggerKey = false;
        keyMeshRenderer.enabled = false;
        keyCollider.enabled = false;
        keyAnimator.StopPlayback();
        keyGlow.SetActive(false);
        keySpotLight.SetActive(false);
        keyParticles.Stop();
    }

    void Update() {

        // If all coins have been found...
        if (hasCoins && triggerKey) {

            // ...show the key
            ShowKey();
        }
	}

    // This method is called when a coin is clicked
    public void FoundCoin() {

        // Increment the number of coins found
        coinCount++;

        // If all 5 coins have been found...
        if (coinCount == 5) {

            // ...change the 'hasCoins' variable to 'true'
            hasCoins = true;
            triggerKey = true;
        }
    }

    // This method is called when the key is clicked
	public void OnKeyClicked() {

        // Play the key sound
        soundSource.Play();

        // Destroy the glow and spot light
        Destroy(keyGlow);
        Destroy(keySpotLight);

        // Hide the key
        keyMeshRenderer.enabled = false;

        // Disable the particle system
        keyParticles.Stop();

        // Unlock the door
        keyDoor.Unlock();
        
        // Destroy the key
        Destroy(keyObject, keyDestroDelay);
    }

    // This method shows the key
    public void ShowKey() {

        // Make Visible
        keyMeshRenderer.enabled = true;
        keyCollider.enabled = true;

        // Light the key
        keyGlow.SetActive(true);
        keySpotLight.SetActive(true);

        // Turn on the Poof and animation
        keyParticles.Play();
        keyAnimator.StartPlayback();

        // Turn off trigger
        triggerKey = false;
    }
}