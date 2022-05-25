using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinNeutralss : MonoBehaviour
{
    public GameObject Neutralss;

    public bool activateme;

    private void OnTriggerEnter2D(Collider2D other)
    {

        {
            if (activateme == true)
            {
                Neutralss.SetActive(true);;
                Destroy(gameObject);
            }
        }
    }
}
