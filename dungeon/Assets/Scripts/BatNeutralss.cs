using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatNeutralss : MonoBehaviour
{
    public GameObject BatNeutral;

    public bool activateme;

    private void OnTriggerEnter2D(Collider2D other)
    {

        {
            if (activateme == true)
            {
                BatNeutral.SetActive(true); ;
                Destroy(gameObject);
            }
        }
    }
}
