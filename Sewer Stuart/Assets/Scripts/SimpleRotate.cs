using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    [SerializeField] Vector3Random randomStartRotation;
    [SerializeField] Vector3Random rotateSpeedRange;
    Vector3 rotateSpeed;

    void OnEnable()
    {
        transform.Rotate(
            Random.Range(randomStartRotation.min.x, randomStartRotation.max.x),
            Random.Range(randomStartRotation.min.y, randomStartRotation.max.y),
            Random.Range(randomStartRotation.min.z, randomStartRotation.max.z)
        );

        rotateSpeed = new Vector3(
            Random.Range(rotateSpeedRange.min.x, rotateSpeedRange.max.x),
            Random.Range(rotateSpeedRange.min.y, rotateSpeedRange.max.y),
            Random.Range(rotateSpeedRange.min.z, rotateSpeedRange.max.z)
        );
    }

    void FixedUpdate()
    {
        transform.Rotate(rotateSpeed * Time.fixedDeltaTime);
    }
}

[System.Serializable]
public struct Vector3Random
{
    public Vector3 min;
    public Vector3 max;

    public Vector3Random(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }
}
