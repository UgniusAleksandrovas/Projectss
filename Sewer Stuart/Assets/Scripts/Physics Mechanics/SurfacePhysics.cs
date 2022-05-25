using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfacePhysics : MonoBehaviour
{
    [Range(1f, 100f)] public float friction = 100f;
    [Range(0f, 2f)] public float jumpMultiplier = 1f;
}
