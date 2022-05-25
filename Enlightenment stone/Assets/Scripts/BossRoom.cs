using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    public GameObject spawner;
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            spawner.SetActive(true);
        }
    }
}
