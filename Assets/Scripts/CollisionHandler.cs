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
                DestroyPlayer();
                break;
        }
    }

    private void DestroyPlayer() {
        TriggerCrashSequence();
        DisablePlayerController();
        Invoke(nameof(ReloadLevel), endOfLevelDelay);
    }

    private void FinishLevel() {
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

    private void TriggerCrashSequence() {
        sceneIsTransitioning = !sceneIsTransitioning;
        crashPFX.Play();
        _audioSource.Stop();
        _audioSource.PlayOneShot(collisionSFX);
        _movementComponent.cancelParticleEffects();
        
    }

    private void TriggerSuccessSequence() {
        sceneIsTransitioning = !sceneIsTransitioning;
        successPFX.Play();
        _audioSource.Stop();
        _audioSource.PlayOneShot(missionSuccessSFX);
        _movementComponent.cancelParticleEffects();
    }

    private void ReloadLevel() {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void DisablePlayerController() {
        _movementComponent.enabled = false;
    }
}