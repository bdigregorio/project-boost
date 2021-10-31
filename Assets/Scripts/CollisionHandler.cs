using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour {
    private Movement _movementComponent;
    private AudioSource _audioSource;

    [SerializeField] private float endOfLevelDelay = 0;
    
    private void Start() {
        _movementComponent = GetComponent<Movement>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other) {
        switch (other.gameObject.tag) {
            case "Friendly":
                Debug.Log("Collision detected: Friendly");
                break;
            case "Finish":
                Debug.Log("Collision detected: Finish");
                FinishLevel();
                break;
            default:
                Debug.Log("Collision detected: Obstacle");
                DestroyPlayer();
                break;
        }
    }

    private void DestroyPlayer() {
        // TODO add SFX and particle FX
        DisablePlayerController();
        Invoke(nameof(ReloadLevel), endOfLevelDelay);
    }
    
    private void FinishLevel() {
        // TODO add SFX and particle FX
        DisablePlayerController();
        Invoke(nameof(LoadNextLevel), endOfLevelDelay);
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

    private void DisablePlayerController() {
        _audioSource.enabled = false;
        _movementComponent.enabled = false;
    }

}