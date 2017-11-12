using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Door : MonoBehaviour 
{
    // Needed Information from Scene
    public GameObject viewObject;           // Player's view object
    public GameObject doorObject;           // Door GameObject
    public Navigation waypointsNavigation;  // Object used to navigate
    public Collider doorCollider;           // Collider component
    public AudioSource soundSource;         // AudioSource to use
    public AudioClip soundDoorLocked;       // AudioClip to play if locked
    public AudioClip soundDoorOpenned;      // AudioClip to play if openned
    
    // Unity Modifiable Variables
    public float doorHeight;                // Height to raise the door
    public float walkingSpeed = 0.05f;      // Speed to move the player

    // Method Variables
    bool locked;
    bool openning;

    void Start() {

        // Set up door parameters
        locked = true;
        openning = false;
    }

    void Update() {

        // If the door is set to the openning status...
        if (openning && !locked) {

            // ...if the door is not fully raised...
            if (doorObject.transform.position.y < doorHeight) {

                // ...raise the door
                doorObject.transform.Translate(0, 2.5f * Time.deltaTime, 0,
                Space.World);
            } else {

                // ...else turn off the openning status               
                openning = false;

                // Move player into temple
                waypointsNavigation.doorOpenned = true;
            }
        }
    }

    // This method is called when the player clicks on the door
    public void OnDoorClicked() {

        // If the door is unlocked...
        if (!locked) {

            // Play the unlocked sound
            soundSource.clip = soundDoorOpenned;
            soundSource.Play();

            // ...set the openning status to 'true'
            openning = true;
        } else {

            // ...else play locked sound
            soundSource.clip = soundDoorLocked;
            soundSource.Play();
        }
    }

    // This method is called by the Key script when key is found
    public void Unlock() {

        // Set the door to unlocked
        locked = false;
    }
}
