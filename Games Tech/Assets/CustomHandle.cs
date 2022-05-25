using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]

public class HandleOffsets
{
    public float size = 1;
    public float offset = 1;
}

public enum HandleTypes
{
    Arrow, circle, cone, cube, dot, rectangle, sphere
}

public class CustomHandle : MonoBehaviour
{
    [SerializeField]
    public HandleOffsets handleOffsets;
    public HandleTypes handleType;
}
