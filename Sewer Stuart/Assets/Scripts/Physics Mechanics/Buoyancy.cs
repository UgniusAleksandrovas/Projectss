using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Buoyancy : MonoBehaviour
{
    public Transform[] floatingObjects;

    public float underwaterDrag = 3f;
    public float underwaterAngularDrag = 1f;

    public float airDrag = 0f;
    public float airAngularDrag = 0.05f;

    public float floatingStrength;
    public float heightOfWater = 0f;

    Rigidbody myRigidbody;

    bool isUnderwater;

    int objectsUnderwater;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        objectsUnderwater = 0;
        for (int i = 0; i < floatingObjects.Length; i++)
        {
            float distanceUnderwater = floatingObjects[i].position.y - heightOfWater;

            if (distanceUnderwater < 0)
            {
                myRigidbody.AddForceAtPosition(Vector3.up * floatingStrength * Mathf.Abs(distanceUnderwater), floatingObjects[i].position, ForceMode.Force);
                objectsUnderwater += 1;

                if (isUnderwater == false)
                {
                    isUnderwater = true;
                    ChangeState(true);
                }
            }
        }


        if (isUnderwater && objectsUnderwater == 0)
        {
            isUnderwater = false;
        }
    }

    void ChangeState(bool isUnderwater)
    {
        if (isUnderwater)
        {
            myRigidbody.drag = underwaterDrag;
            myRigidbody.angularDrag = underwaterAngularDrag;
        }
        else
        {
            myRigidbody.drag = airDrag;
            myRigidbody.angularDrag = airAngularDrag;
        }
    }
}