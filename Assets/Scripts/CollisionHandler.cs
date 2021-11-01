using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour {
    [SerializeField] float endOfLevelDelay = 0;
    [SerializeField] AudioClip collisionSFX;
    [SerializeField] AudioClip missionSuccessSFX;

    Movement _movementComponent;
    AudioSource _audioSource;

    bool isTransitioning = false;

    private void Start() {
        _movementComponent = GetComponent<Movement>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other) {
        if (isTransitioning) return;

        switch (other.gameObject.tag) {
            case "Friendly":
                Debug.Log("Collision detected: Friendly");
                break;
            case "Finish":
                FinishLevel();
                break;
            default:
                DestroyPlayer();
                break;
        }
    }

    private void DestroyPlayer() {
        // TODO particle FX
        TriggerExplosion();
        DisablePlayerController();
        Invoke(nameof(ReloadLevel), endOfLevelDelay);
    }

    private void FinishLevel() {
        // TODO add particle FX
        TriggerSuccessSequence();
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

    private void TriggerExplosion() {
        isTransitioning = !isTransitioning;
        _audioSource.Stop();
        _audioSource.PlayOneShot(collisionSFX);
    }

    private void TriggerSuccessSequence() {
        isTransitioning = !isTransitioning;
        _audioSource.Stop();
        _audioSource.PlayOneShot(missionSuccessSFX);
    }

    private void ReloadLevel() {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void DisablePlayerController() {
        _movementComponent.enabled = false;
    }
}