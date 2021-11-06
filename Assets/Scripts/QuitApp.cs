using UnityEngine;

public class QuitApp : MonoBehaviour {
    
    void Update() {
        ReadInput();
    }

    void ReadInput() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Debug.Log("Quitting application");
            Application.Quit();
        }
    }
}
