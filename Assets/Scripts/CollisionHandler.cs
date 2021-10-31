using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour {
    private void OnCollisionEnter(Collision other) {
        switch (other.gameObject.tag) {
            case "Friendly":
                Debug.Log("Collision detected: Friendly");
                break;
            case "Finish":
                Debug.Log("Collision detected: Finish");
                LoadNextLevel();
                break;
            case "Fuel":
                Debug.Log("Collision detected: Fuel");
                break;
            default:
                Debug.Log("Collision detected: Obstacle");
                ReloadLevel();
                break;
        }
    }

    private void LoadNextLevel() {
        var nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextLevelIndex >= SceneManager.sceneCountInBuildSettings) {
            nextLevelIndex = 0;
        }

        SceneManager.LoadScene(nextLevelIndex);
    }

    private void ReloadLevel() {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}