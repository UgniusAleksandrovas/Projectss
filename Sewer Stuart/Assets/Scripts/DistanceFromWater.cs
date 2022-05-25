using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceFromWater : MonoBehaviour
{
    [SerializeField] RatController rat;

    // Update is called once per frame
    void Update()
    {
        rat.PlayerDistanceFromWater();
    }
}
