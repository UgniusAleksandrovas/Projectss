using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilNeutralss : MonoBehaviour
{
    public GameObject DevilNeutral;
    public bool activateme;

    private void OnTriggerEnter2D(Collider2D other)
    {

        {
            if (activateme == true)
            {
                DevilNeutral.SetActive(true); ;
                Destroy(gameObject);
            }
        }
    }
}
