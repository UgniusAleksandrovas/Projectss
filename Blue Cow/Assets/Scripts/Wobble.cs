using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobble : MonoBehaviour {

    [Header("Position")]
    [SerializeField] Vector2 moveAmount;
    [SerializeField] float moveSpeed;

    [Header("Rotation")]
    [SerializeField] float rotateAmount;
    [SerializeField] float rotateSpeed;

    Vector2 oldPosition;

    // Start is called before the first frame update
    void Start() {
        oldPosition = transform.position;

        transform.position = oldPosition + Mathf.Sin(Random.Range(0f, 1f)) * moveAmount;
        transform.rotation = Quaternion.Euler(Vector3.forward * Mathf.Sin(Random.Range(0f, 1f)) * rotateAmount);
    }

    // Update is called once per frame
    void Update() {
        transform.position = oldPosition + Mathf.Sin(Time.deltaTime * moveSpeed) * moveAmount;

        transform.rotation = Quaternion.Euler(Vector3.forward * Mathf.Sin(Time.deltaTime * rotateSpeed) * rotateAmount);
    }
}
