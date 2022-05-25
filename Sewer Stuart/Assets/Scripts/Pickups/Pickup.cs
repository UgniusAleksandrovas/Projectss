using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;

public class Pickup : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] GameObject model;
    [SerializeField] Vector3 rotateAngle = new Vector3(0f, 90f, 0f);
    [SerializeField] float bobAmplitude = 0.1f;
    [SerializeField] float bobSpeed = 2f;
    Vector3 bobPosition;
    Vector3 startPos;

    void Start()
    {
        if (model != null)
        {
            startPos = model.transform.localPosition;
        }
        else
        {
            startPos = transform.position;
        }
        Initialize();
    }

    public virtual void Initialize()
    {
        //initilizating variables goes here...
    }
    
    void Update()
    {
        if (model != null)
        {
            Animate();
        }
    }
    
    public virtual void Animate()
    {
        transform.Rotate(rotateAngle * Time.deltaTime);
        bobPosition.y = bobAmplitude * Mathf.Sin(bobSpeed * Time.time);
        model.transform.localPosition = startPos + bobPosition;
    }

    public virtual void PickupItem(RatController player)
    {
        //do something to the player (i.e. increase movement speed)
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        RatController player = other.GetComponent<RatController>();
        if (player != null)
        {
            PickupItem(player);
            player.audioController.Pickup();
        }
        Destroy(gameObject);
    }
}

public struct PickupAnimateJob : IJobParallelFor 
{
    [ReadOnly]
    public float rotateSpeed;

    [WriteOnly]
    NativeArray<float3> positions;

    public void Execute(int index)
    {/*
        positions[index].y = bobAmplitude * Mathf.Sin(bobSpeed * Time.time);
        model.transform.localPosition = startPos + bobPosition;
        */
    }
}
