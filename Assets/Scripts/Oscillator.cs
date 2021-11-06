using UnityEngine;

public class Oscillator : MonoBehaviour {

    Vector3 startingPosition;
    private float tau;
    [SerializeField] private float period;
    [SerializeField] Vector3 movementVector;

    void Start() {
        startingPosition = transform.position;
        tau = Mathf.PI * 2;
    }

    void Update() {
        HandleOscillation();
    }

    void HandleOscillation() {
        if (period <= Mathf.Epsilon) return;
        var cycles = Time.time  * tau / period; // input scaled properly for the period
        var sineOutput = Mathf.Sin(cycles); // from -1 to 1
        var movementFactor = (sineOutput + 1f) / 2f; // rescaled from 0 to 1 to calculate offset
        var offset = movementFactor * movementVector; 
        transform.position = startingPosition + offset;
    }
}