using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Spin : Obstacle
{
    [Header("Spin Settings")]
    [SerializeField] Vector3Random randomStartRotation = new Vector3Random(new Vector3(0, 0, -180), new Vector3(0, 0, 180));
    [SerializeField] Vector3Random rotateSpeedRange = new Vector3Random(new Vector3(0, 0, -70), new Vector3(0, 0, -50));

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

    public override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
    }

    public override void OnTriggerEnter(Collider other) { }
}
