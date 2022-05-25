using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour {

    PauseMenu pm;

    // Start is called before the first frame update
    void Start() {
        pm = FindObjectOfType<PauseMenu>();
        pm.ResumeGame();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!pm.paused) {
                pm.PauseGame();
            }
            else {
                pm.ResumeGame();
            }
        }
    }
}
