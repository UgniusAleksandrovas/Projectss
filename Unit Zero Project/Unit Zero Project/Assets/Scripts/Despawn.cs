using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour {

    public bool deactivate;
    public bool outOfSight;
    public float lifetime;

    // Use this for initialization
    void OnEnable() {
        StartCoroutine(LifeTimer());
    }

    public IEnumerator LifeTimer() {
        yield return new WaitForSeconds(lifetime);
        if (deactivate != true && outOfSight != true) {
            Destroy(gameObject);
        }
        if (deactivate == true) {
            gameObject.SetActive(false);
        }
        if (outOfSight == true) {
            transform.position = new Vector3(0, -100, 0);
        }
    }
}
