using UnityEngine;
using System.Collections;


public class Navigation: MonoBehaviour {

    // Needed Information from scene
    public GameObject viewObject;       // Emulator or Main Camera
    
    // Unity Modifiable Variables
    public bool doorOpenned = false;
    public float speed = 0.05f;         
    public string waypointTag = "Waypoint";
    [Header("Temple Position")]
    public float posX = 0f;
    public float posY = 0f;
    public float posZ = 0f;

    // Method variables
    private Waypoint[] waypointArray;   // Waypoint objects of the scene
    private Waypoint currentWaypoint;   // Current location Waypoint
    private Waypoint[] neighborArray;   // Neighbor Waypoints of current Waypoint


    private void Start() {

        // If the view object is null...
        if (viewObject == null) {
            // ...use the main camera
            viewObject = Camera.main.gameObject;
        }

        // Get an array of Waypoints in the scene
        waypointArray = GetWaypoints();

        // Find the closest Waypoint to the player, set as current
        currentWaypoint = GetClosestWaypoint();

        // Set the current Waypoint as 'occupied'
        currentWaypoint.Occupy();

        // Start player at the current Waypoint
        viewObject.transform.position = currentWaypoint.transform.position;

        // Update all Waypoints
        UpdateAll();
    }

    private void Update() {

        // If there are Waypoints in the scene...
        if (waypointArray.Length > 0) {

            // ...check to see if any have been triggered
            for (int i = 0; i < waypointArray.Length; i++) {

                // If Waypoint has been triggered...
                if (waypointArray[i].triggered) {

                    // ...leave the current Waypoint
                    currentWaypoint.Depart();

                    // ...set as current and occupied
                    currentWaypoint = waypointArray[i];
                    currentWaypoint.Occupy();
                }
            }
        }

        // Update all Waypoints
        UpdateAll();
    }

    // This method updates every Waypoint's status and player location
    public void UpdateAll() {

        // For every Waypoint...
        for (int i = 0; i < waypointArray.Length; i++) {

            // ...update the Waypoint activation status
            waypointArray[i].UpdateActivation();
        }

        // If the door is closed...
        if (!doorOpenned) {

            // ...and the player location and current Waypoint differ...
            if (viewObject.transform.position != currentWaypoint.transform.position) {

                // ...move player
                MovePlayer(currentWaypoint);
            }
        }

        // If door is open...
        if (doorOpenned) {

            // Go into Temple
            GoIntoTemple();
        }
    }

    // This method relocates the player to the given Waypoint
    public void MovePlayer(Waypoint waypoint) {

        // Get the distance between the Waypoint and the player
        float distance = Vector3.Distance(viewObject.transform.position, 
            waypoint.transform.position);

        // If the distance is significant...
        if (distance > 0.5f) {

            // ...walk the player to the waypoint
            viewObject.transform.position = Vector3.Lerp(
                viewObject.transform.position, 
                waypoint.transform.position, speed);
        } else {

            // ....shift player to the Waypoint
            viewObject.transform.position = waypoint.transform.position;
        }
    }

    // This method determines the closest Wyapoint to the player's location
    public Waypoint GetClosestWaypoint() {
        int closestIndex = 0;
        float closestDistance = float.PositiveInfinity;

        // For each Waypoint in the array...
        for (int i = 0; i < waypointArray.Length; i++) {

            // ...get the distance between the player and the waypoint
            float waypointDistance = Vector3.Distance(
                viewObject.transform.position, 
                waypointArray[i].transform.position);

            // If this distance is closer than the current closest distance...
            if (waypointDistance < closestDistance) {

                // ...set the closest Waypoint properties to this Waypoint
                closestDistance = waypointDistance;
                closestIndex = i;
            }
        }

        return waypointArray[closestIndex];
    }

    // This method gets all the scene Waypoint prefabs
    public Waypoint[] GetWaypoints() {

        // Find all game objects with a 'Waypoint' tag
        GameObject[] waypointGameObjects = 
            GameObject.FindGameObjectsWithTag(waypointTag);

        // Create a new Waypoint array
        Waypoint[] waypoints = new Waypoint[waypointGameObjects.Length];

        // For each Waypoint game object...
        for (int i = 0; i < waypointGameObjects.Length; i++) {

            // ...pull over the Waypoint component at the current index
            waypoints[i] = waypointGameObjects[i].GetComponent<Waypoint>();
        }

        return waypoints;
    }

    // This method moves the player into the temple
    public void GoIntoTemple() {

        // Define temple position
        Vector3 templePosition = new Vector3(posX, posY, posZ);

        // Walk player into temple
        viewObject.transform.position = Vector3.Lerp(
                viewObject.transform.position, templePosition, speed);
    }
}