using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField] float mainThrust = 0;
    [SerializeField] float rotationThrust = 0;
    [SerializeField] AudioClip rocketThrustSFX;
    [SerializeField] AudioClip thrusterHissSFX;
    [SerializeField] ParticleSystem mainThrusterPFX;
    [SerializeField] ParticleSystem leftThrusterPFX;
    [SerializeField] ParticleSystem rightThrusterPFX;
    [SerializeField] AudioSource thrusterAudioSource;
    [SerializeField] AudioSource rotationAudioSource;

    private Rigidbody _rigidbody;
    private CollisionHandler _collisionHandler;

    void Start() {
        _rigidbody = GetComponent<Rigidbody>();
        _collisionHandler = GetComponent<CollisionHandler>();
    }

    void Update() {
        ProcessDevModes();
        ProcessThrust();
        ProcessRotation();
    }

    public void cancelMovementEffects() {
        thrusterAudioSource.Stop();
        rotationAudioSource.Stop();
        mainThrusterPFX.Stop();
        leftThrusterPFX.Stop();
        rightThrusterPFX.Stop();
    }

    private void ProcessDevModes() {
        if (Input.GetKeyDown(KeyCode.L)) {
            _collisionHandler.LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            _collisionHandler.ToggleBypassCollisionMode();
        }
    }

    private void ProcessThrust() {
        if (Input.GetKey(KeyCode.W)) {
            EngageMainThruster();
        } else {
            DisengageMainThruster();
        }
    }

    private void ProcessRotation() {
        if (Input.GetKey(KeyCode.A)) {
            RotateLeft();
        } else if (Input.GetKey(KeyCode.D)) {
            RotateRight();
        } else {
            StopRotating();
        }
    }

    private void DisengageMainThruster() {
        thrusterAudioSource.Stop();
        mainThrusterPFX.Stop();
    }

    private void EngageMainThruster() {
        _rigidbody.AddRelativeForce(mainThrust * Time.deltaTime * Vector3.up);
        if (!thrusterAudioSource.isPlaying){
            thrusterAudioSource.PlayOneShot(rocketThrustSFX);
        }
        if (!mainThrusterPFX.isPlaying) {
            mainThrusterPFX.Play();
        }
    }

    private void RotateRight() {
        ApplyRotation(-rotationThrust);
        if (!rotationAudioSource.isPlaying) {
            rotationAudioSource.PlayOneShot(thrusterHissSFX);
        }
        if (!leftThrusterPFX.isPlaying) {
            leftThrusterPFX.Play();
        }
    }

    private void RotateLeft() {
        ApplyRotation(rotationThrust);
        if (!rotationAudioSource.isPlaying) {
            rotationAudioSource.PlayOneShot(thrusterHissSFX);
        }
        if (!rightThrusterPFX.isPlaying) {
            rightThrusterPFX.Play();
        }
    }

    private void StopRotating() {
        rotationAudioSource.Stop();
        leftThrusterPFX.Stop();
        rightThrusterPFX.Stop();
    }

    private void ApplyRotation(float rotation) {
        // disable rotation applied from the physics system
        _rigidbody.freezeRotation = true;

        // apply rotation from the controller manually 
        transform.Rotate(rotation * Time.deltaTime * Vector3.forward);

        // reenable rotation applied from the physics system
        _rigidbody.freezeRotation = false;
    }
}