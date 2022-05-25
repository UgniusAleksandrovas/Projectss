using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorHealth : Health {

    public GameObject lightning;
    public GameObject smoke;
    public AudioClip electricity;
    public AudioClip sparks;
    public AudioClip explosion;

    private bool oneShot;
    private AudioSource m_AudioSource;
    private GeneratorManager gm;

    private void Start() {
        gm = FindObjectOfType<GeneratorManager>();
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.clip = electricity;
        m_AudioSource.Play();
        m_AudioSource.time = Random.Range(0f, 20f);
    }

    public override void UpdateHealth() {
        if (health <= 0) {
            if (oneShot == false) {
                oneShot = true;
                gm.UpdateGenerators();
                lightning.SetActive(false);
                smoke.SetActive(true);
                if (GetComponent<EventTrigger>()) {
                    GetComponent<EventTrigger>().CallOtherFunctions();
                }
                m_AudioSource.PlayOneShot(explosion);
                m_AudioSource.clip = sparks;
                m_AudioSource.Play();
            }
        }
    }
}
