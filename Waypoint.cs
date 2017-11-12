using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Collider))]
public class Waypoint: MonoBehaviour {

    // Needed information from Scene
    public Rigidbody waypointRigidBody;         // RigidBody component
    public MeshRenderer waypointMeshRenderer;   // MeshRenderer component
    public Collider waypointCollider;           // Collider component
    public AudioSource soundSource;             // AudioSource to use
    public AudioClip soundFile;                 // AudioClip to play
    public Waypoint[] neighborhood;             // Neighbor Waypoints

    // Unity Modifiable Variables
    [Header("Colors")]
    public Color colorActive    = new Color(0.0f, 1.0f, 0.0f, 0.5f);
    public Color colorHilight   = new Color(0.8f, 0.8f, 1.0f, 0.125f);
    public Color colorDisabled  = new Color(0.125f, 0.125f, 0.125f, 0.0f);
    [Header("Animation")]
    public float animationScale = 3.0f;
    public float animationSpeed = 3.0f;
    [Header("States")]
    public bool occupied;
    public bool active;
    public bool triggered;
    public bool focused;
    
    // Method Variables
    private Material waypointMaterial;  // MeshRenderer Material property
    private Vector3 originalScale;      // Scale of Waypoint at Instantiate
    private float hilight           = 0.0f;     // Degree of color change
    private float hilightFadeSpeed  = 0.25f;    // Speed of color change

    void Awake() {

        // Get material property from mesh renderer
        waypointMaterial = waypointMeshRenderer.material;

        // Get original scale
        originalScale = transform.localScale;

        // Keep sound from automatically playing and set sound
        soundSource.playOnAwake = false;
        soundSource.clip = soundFile;

        // Reset all state parameters
        occupied = false;
        active = false;
        triggered = false;
        focused = false;

        // Update activation status
        UpdateActivation();
    }

    void LateUpdate() {

        // Animate the active waypoints
        if (active && !occupied) {

            // Animate Waypoint
            Animate();

        } else {

            // Deactivate Waypoint
            Deactivate();
        }
    }

    // This method activates the Waypoint if its neighbor is occupied
    public void UpdateActivation() {

        // Deactivate the Waypoint
        Deactivate();

        // Check all neighbors...
        for (int i = 0; i < neighborhood.Length; i++) {

            // ...if neighbor is occupied...
            if (neighborhood[i].occupied) {

                // ...activate this Waypoint
                Activate();
            }
        }
    }

    // This method activates the Waypoint
    public void Activate() {

        // Reset the color and local scale values
        waypointMaterial.color = colorActive;
        transform.localScale = originalScale;

        // Set parameters for an active Waypoint
        occupied = false;
        active = true;
        triggered = false;

        // Make visable
        waypointMeshRenderer.enabled = true;
        waypointCollider.enabled = true;
    }

    // This method deactivates the Waypoint
    public void Deactivate() {

        // Set the disabled color and change the scale to half original
        waypointMaterial.color = colorDisabled;
        transform.localScale = originalScale * 0.5f;

        // Set parameters for deactivation of Waypoint
        active = false;
        triggered = false;

        // Make invisible
        waypointMeshRenderer.enabled = false;
        waypointCollider.enabled = false;
    }

    // This method sets the properties when Waypoint is occupied
    public void Occupy() {
        occupied = true;
        active = false;
    }

    // This method sets the properties when Waypoint is active
    public void Depart() {
        occupied = false;
    }

    // This method is called when the pointer hovers over the Waypoint
    public void Enter() {

        if (!focused && active) {

            focused = true;
            hilight = 0.5f;
        }
    }

    // This method is called when the pointer leaves the Waypoint
    public void Exit() {

        focused = false;
        hilight = 1.0f;
    }

    // This method is called when the pointer clicks the Waypoint
    public void Trigger() {

        // If the Waypoint is accessible
        if (focused && active && !occupied) {

            focused = false;
            triggered = true;
            hilight = 1.0f;

            // Play sound
            soundSource.Play();
        }
    }

    // This method animates the Waypoint
    public void Animate() {

        // Define the pulse rate
        float pulseAnimation = Mathf.Abs(Mathf.Cos(Time.time * animationSpeed));

        // Define the color lerp
        waypointMaterial.color = Color.Lerp(colorActive, colorHilight, hilight);

        // Define the degree of color change
        hilight = Mathf.Max(hilight - hilightFadeSpeed, 0.0f);

        // Define the scale of the hilight
        Vector3 hilightScale = Vector3.one * (hilight + (focused ? 0.5f : 0.0f));

        // Define the local scale lerp
        transform.localScale = Vector3.Lerp(originalScale + hilightScale,
            originalScale * animationScale + hilightScale, pulseAnimation);
    }
}