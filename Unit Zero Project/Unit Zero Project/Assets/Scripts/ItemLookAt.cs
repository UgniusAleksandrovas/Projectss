using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLookAt : MonoBehaviour {

    public float maxRotationX;
    public float maxRotationY;
    public float speed;

    private Vector3 startRot;

    // Use this for initialization
    void Start() {
        startRot = transform.localEulerAngles;
    }
    void OnEnable() {
        transform.localEulerAngles = startRot;
        //transform.LookAt(FindObjectOfType<FPSController>().transform);
    }

    // Update is called once per frame
    void Update() {
        float movementX = Input.GetAxis("Mouse X") * speed;
        float movementY = -Input.GetAxis("Mouse Y") * speed;
        movementX = Mathf.Clamp(movementX, -maxRotationX, maxRotationX);
        movementY = Mathf.Clamp(movementY, -maxRotationY, maxRotationY);
        transform.Rotate(movementY * Time.deltaTime, movementX * Time.deltaTime, 0);
        //transform.rotation = Quaternion.Euler(movementY * Time.deltaTime, movementX * Time.deltaTime, 0);
        /*
        if (transform.localEulerAngles.x < maxRotationX || transform.localEulerAngles.x > 360 - maxRotationX) {
            if (transform.localEulerAngles.y < maxRotationY || transform.localEulerAngles.y > 360 - maxRotationY) {
                transform.Rotate(movementY * speed * Time.deltaTime, movementX * speed * Time.deltaTime, 0, Space.World);
            }
        }*/
    }
}
