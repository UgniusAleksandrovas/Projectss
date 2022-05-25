using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField]
    bool screenSpace = false;

    [SerializeField]
    private bool x, y, z;

    Transform cam;
    Vector3 lookDir;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        lookDir = cam.position - transform.position;
        if (!x) lookDir.x = 0;
        if (!y) lookDir.y = 0;
        if (!z) lookDir.z = 0;
        
        if (screenSpace)
        {
            transform.rotation = cam.rotation;
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(lookDir);
        }
    }
}
