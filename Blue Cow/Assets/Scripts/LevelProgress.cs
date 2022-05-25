using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgress : MonoBehaviour {

    float cowProgress;
    float playerProgress;

    [SerializeField] Slider cowProgressSlider;
    [SerializeField] Slider playerProgressSlider;

    BlueCow bc;
    PlayerController pc;
    Vector2 startPos;
    float totalDistance;

    // Start is called before the first frame update
    void Start() {
        startPos = GameObject.FindWithTag("Spawnpoint").transform.position;
        totalDistance = transform.position.x - startPos.x;
        pc = FindObjectOfType<PlayerController>();
        bc = FindObjectOfType<BlueCow>();
    }

    // Update is called once per frame
    void Update() {
        cowProgress = (bc.transform.position.x - startPos.x) / totalDistance;
        cowProgressSlider.value = cowProgress;

        playerProgress = (pc.transform.position.x - startPos.x) / totalDistance;
        playerProgressSlider.value = playerProgress;
    }
}
