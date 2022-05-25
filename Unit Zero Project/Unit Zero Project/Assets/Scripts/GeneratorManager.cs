using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorManager : MonoBehaviour {

    public GameObject lightning;
    public GameObject shield;
    public List<GeneratorHealth> generators;

    public void UpdateGenerators() {
        int destroyed = 0;
        for (int i = 0; i < generators.Count; i++) {
            if (generators[i].health <= 0) {
                destroyed += 1;
            }
        }
        if (destroyed == generators.Count) {
            lightning.SetActive(false);
            shield.SetActive(false);
        }
    }
}
