using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFight : MonoBehaviour {

    public AudioClip bossFightMusic;
    public AudioSource musicSource;

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<FPSController>()) {
            musicSource.clip = bossFightMusic;
            if (musicSource.isPlaying != true) musicSource.Play();
        }
    }

}
