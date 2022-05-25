using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBody : MonoBehaviour 
{
    [Header("Water Body Settings")]
    public Vector3 direction = new Vector3(1, 0, 0);
    [SerializeField] float startHeightOffset = 0f;
    public float waterCurrentSpeed = 1f;

    [Header("Wave Settings")]
    [SerializeField] float waveAmplitude = 1f;
    [SerializeField] float waveLength = 2f;
    [SerializeField] float waveSpeed = 1f;
    [SerializeField] float startWaveOffset = 0f;

    float offset = 0f;

    void Start()
    {
        offset = startWaveOffset;
    }

    void Update() 
    {
        offset += Time.deltaTime * waveSpeed;
    }
    public float GetWaveHeight(Vector3 pos) 
    {
        float x = direction.x * pos.x;
        float z = direction.z * pos.z;
        float average = (x + z) / 2;
        float height = transform.position.y + startHeightOffset + waveAmplitude * Mathf.Sin(average / waveLength + offset);
        return height;
    }
}
