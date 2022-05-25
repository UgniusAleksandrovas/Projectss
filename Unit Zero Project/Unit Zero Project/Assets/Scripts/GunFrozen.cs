using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFrozen : MonoBehaviour {

    public List<ParticleSystem> particles;

    private MainMenu menuScript;

    // Start is called before the first frame update
    void Start() {
        menuScript = FindObjectOfType<MainMenu>();
        foreach(ParticleSystem part in particles) {
            part.Emit(1);
        }
        StartCoroutine(StopParticles());
    }

    private IEnumerator StopParticles() {
        yield return new WaitForSeconds(menuScript.waitTillFreezeTime);
        foreach (ParticleSystem part in particles) {
            part.Pause();
        }
    }
}
