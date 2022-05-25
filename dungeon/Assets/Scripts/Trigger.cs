using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public GameObject Trigeris;
    public GameObject Trigeris1;
    public GameObject Trigeris2;
    public GameObject Trigeris3;
    public GameObject Trigeris4;

    public bool activateme;

    private void OnTriggerEnter2D(Collider2D other)
    {

        {
            if (activateme == true)
            {
                Trigeris.SetActive(true);
                Trigeris1.SetActive(true);
                Trigeris2.SetActive(true);
                Trigeris3.SetActive(true);
                Trigeris4.SetActive(true);
                Destroy(gameObject);
            }
        }
    }
}