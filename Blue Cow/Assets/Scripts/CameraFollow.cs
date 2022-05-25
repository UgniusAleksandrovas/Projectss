using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField] Transform target;
    [SerializeField] float positionSmooth = 0.3f;
    [SerializeField] bool fixedUpdate;

    Vector3 _velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (!fixedUpdate) {
            Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y, -10);
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, positionSmooth);
        }
    }

    void FixedUpdate() {
        if (fixedUpdate) {
            Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y, -10);
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, positionSmooth);
        }
    }
}
