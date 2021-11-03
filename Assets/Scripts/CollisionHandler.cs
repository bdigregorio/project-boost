using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour {
    [SerializeField] float endOfLevelDelay = 0;
    [SerializeField] AudioClip collisionSFX;
    [SerializeField] AudioClip missionSuccessSFX;
    [SerializeField] ParticleSystem successPFX;
    [SerializeField] ParticleSystem crashPFX;

    Movement _movementComponent;
    AudioSource _audioSource;

    bool sceneIsTransitioning = false;
    bool shouldBypassCollisions = false;

    private void Start() {
        _movementComponent = GetComponent<Movement>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other) {
        if (sceneIsTransitioning) return;

        switch (other.gameObject.tag) {
            case "Friendly":
                Debug.Log("Collision detected: Friendly");
                break;
            case "Finish":
                FinishLevel();
                break;
            default:
                HandleCollision();
                break;
        }
    }

    public void LoadNextLevel() {
        var nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextLevelIndex >= SceneManager.sceneCountInBuildSettings) {
            nextLevelIndex = 0;
        }

        SceneManager.LoadScene(nextLevelIndex);
    }

    public void ToggleBypassCollisionMode() {
        shouldBypassCollisions = !shouldBypassCollisions;
    }

    private void ReloadLevel() {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void HandleCollision() {
        if (!shouldBypassCollisions) {
            TriggerCrashSequence();
            DisablePlayerController();
            Invoke(nameof(ReloadLevel), endOfLevelDelay);
        }
    }

    private void FinishLevel() {
        TriggerSuccessSequence();
        DisablePlayerController();
        Invoke(nameof(LoadNextLevel), endOfLevelDelay);
    }

    private void TriggerCrashSequence() {
        sceneIsTransitioning = !sceneIsTransitioning;
        crashPFX.Play();
        _movementComponent.cancelMovementEffects();
        _audioSource.PlayOneShot(collisionSFX);
        
    }

    private void TriggerSuccessSequence() {
        sceneIsTransitioning = !sceneIsTransitioning;
        successPFX.Play();
        _movementComponent.cancelMovementEffects();
        _audioSource.PlayOneShot(missionSuccessSFX);
    }

    private void DisablePlayerController() {
        _movementComponent.enabled = false;
    }
}