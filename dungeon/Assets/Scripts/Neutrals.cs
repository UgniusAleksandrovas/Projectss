using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neutrals : MonoBehaviour
{
    public GameObject GoblinNeutral;

    public bool activateme;

    private void OnTriggerEnter2D(Collider2D other)
    {

        {
            if (activateme == true)
            {
                GoblinNeutral.SetActive(true);;
                Destroy(gameObject);
            }
        }
    }
}
