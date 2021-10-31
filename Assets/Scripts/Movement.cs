using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEngine;

public class Movement : MonoBehaviour {
    private Rigidbody _rigidbody;
    private AudioSource _audioSource;

    [SerializeField] private float mainThrust = 0;
    [SerializeField] private float rotationThrust = 0;

    void Start() {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust() {
        if (Input.GetKey(KeyCode.W)) {
            _rigidbody.AddRelativeForce(mainThrust * Time.deltaTime * Vector3.up);
            if (!_audioSource.isPlaying) {
                _audioSource.Play();
            }
        } else {
            _audioSource.Stop();
        }
    }

    private void ProcessRotation() {
        if (Input.GetKey(KeyCode.A)) {
            ApplyRotation(rotationThrust);
        } else if (Input.GetKey(KeyCode.D)) {
            ApplyRotation(-rotationThrust);
        }
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