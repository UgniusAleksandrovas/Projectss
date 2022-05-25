using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCamera : MonoBehaviour
{
    public Transform playerTransform;

    private Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;

    public bool LookAtPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        _cameraOffset = transform.position - playerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = playerTransform.position + _cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);

        if (LookAtPlayer)
            transform.LookAt(playerTransform);
    }
}
