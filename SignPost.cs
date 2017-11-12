using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class SignPost : MonoBehaviour
{
    // Needed Information from Scene
    public Collider signCollider;       // Collider component
    public Scene mazeScene;             // Maze Scene, for reset


    public void ResetScene() {

        // Get the current Scene
        mazeScene = SceneManager.GetActiveScene();

        // Restart the Scene
        SceneManager.LoadScene(mazeScene.name);
	}
}