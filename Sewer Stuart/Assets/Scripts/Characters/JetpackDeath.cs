using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class JetpackDeath : MonoBehaviour
{
    private void OnCollisionEnter(Collision col)
    {
        RatController rat = col.gameObject.GetComponent<RatController>();
        if (rat)
        {
            rat.GameOver();
        }
    }
}
