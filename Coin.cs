using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Collider))]
public class Coin : MonoBehaviour 
{
    // Needed Information from Scene
    public GameObject coinObject;           // Coin GameObject
    public Key keyObject;                   // Key GameObject
    public MeshRenderer coinMesh;           // MeshRenderer component
    public Collider coinCollider;           // Collider component
    public GameObject glowObject;           // Coin Glow GameObject
    public ParticleSystem particleObject;   // Particle GameObject
    public AudioSource soundSource;         // AudioSource to use

    // Unity Modifiable Variables
    public int coinDestroyDelay;            // Delay before Destroying coin

    // This method is called when the coin is clicked
    public void OnCoinClicked() {
        // Play the coin sound
        soundSource.Play();

        // Add to coin count on key
        keyObject.FoundCoin();

        // Destroy the glow
        Destroy(glowObject);

        // Hide the coin
        coinMesh.enabled = false;

        // Disable the particle system
        particleObject.Stop();

        // Destroy the coin
        Destroy(coinObject, coinDestroyDelay);
    }
}
