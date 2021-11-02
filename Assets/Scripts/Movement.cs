using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField] float mainThrust = 0;
    [SerializeField] float rotationThrust = 0;
    [SerializeField] AudioClip rocketThrustSfx;
    [SerializeField] ParticleSystem mainThrusterPFX;
    [SerializeField] ParticleSystem leftThrusterPFX;
    [SerializeField] ParticleSystem rightThrusterPFX;

    private Rigidbody _rigidbody;
    private AudioSource _audioSource;


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
                _audioSource.PlayOneShot(rocketThrustSfx);
                if (!mainThrusterPFX.isPlaying) {
                    mainThrusterPFX.Play();
                }
            }
        } else {
            _audioSource.Stop();
            mainThrusterPFX.Stop();
        }
    }

    private void ProcessRotation() {
        if (Input.GetKey(KeyCode.A)) {
            ApplyRotation(rotationThrust);
            if (!rightThrusterPFX.isPlaying) {
                rightThrusterPFX.Play();
            }
        } else if (Input.GetKey(KeyCode.D)) {
            ApplyRotation(-rotationThrust);
            if (!leftThrusterPFX.isPlaying) {
                leftThrusterPFX.Play();
            }
        } else {
            leftThrusterPFX.Stop();
            rightThrusterPFX.Stop();
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