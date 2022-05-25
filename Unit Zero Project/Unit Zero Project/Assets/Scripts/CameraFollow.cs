using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform pivot; 
    public Vector3 offset;
    public float followSmoothTime;
    public float rotationSpeed;

    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        transform.position = Vector3.SmoothDamp(transform.position, pivot.position + offset, ref velocity, followSmoothTime);

        float rotationLerp = rotationSpeed * Time.deltaTime;
        Quaternion rot = transform.rotation;
        rot = Quaternion.Slerp(transform.rotation, pivot.rotation, rotationLerp);
        transform.rotation = rot;
    }
}
