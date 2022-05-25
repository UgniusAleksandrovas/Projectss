using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Floater : MonoBehaviour 
{
    [SerializeField] List<Transform> floaterPoints;
    [SerializeField] bool ignoreFloaterPointGravity = false;
    [SerializeField] float depthBeforeSubmerged = 1f;
    [SerializeField] float displacementAmount = 3f;
    [SerializeField] float waterDrag = 0.99f;
    [SerializeField] float waterAngularDrag = 0.5f;
    WaterBody waterObject;
    float waveHeight;
    [HideInInspector] public bool inWater;

    Rigidbody rb;

    void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        if (floaterPoints.Count == 0) 
        {
            floaterPoints.Add(transform);
        }
    }

    void FixedUpdate() {
        for (int i = 0; i < floaterPoints.Count; i++) 
        {
            if (waterObject == null) 
            {
                waveHeight = -100;
            }
            else 
            {
                //waveHeight = waveObject.GetWaveHeight(floaterPoints[i].position.z);
                waveHeight = waterObject.GetWaveHeight(floaterPoints[i].position);
            }
            if (!ignoreFloaterPointGravity)
            {
                rb.AddForceAtPosition(Physics.gravity / floaterPoints.Count, floaterPoints[i].position, ForceMode.Acceleration);
            }
            if (floaterPoints[i].position.y < waveHeight) 
            {
                float displacementMultiplier = Mathf.Clamp01(waveHeight - floaterPoints[i].position.y / depthBeforeSubmerged) * displacementAmount;
                rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), floaterPoints[i].position, ForceMode.Acceleration);
                rb.AddForce(displacementMultiplier * -rb.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
                rb.AddTorque(displacementMultiplier * -rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);

                //Add water current
                rb.AddForceAtPosition(new Vector3(waterObject.direction.x, 0f, waterObject.direction.z).normalized * waterObject.waterCurrentSpeed, floaterPoints[i].position, ForceMode.Acceleration);
            }
        }
    }

    void OnTriggerStay(Collider other) 
    {
        WaterBody waveInstance = other.GetComponent<WaterBody>();
        if (waveInstance != null) 
        {
            waterObject = waveInstance;
            inWater = true;
        }
    }

    void OnTriggerExit(Collider other) 
    {
        WaterBody waveInstance = other.GetComponent<WaterBody>();
        if (waveInstance != null) 
        {
            waterObject = null;
            inWater = false;
        }
    }
}
