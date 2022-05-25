using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winmode : MonoBehaviour
{
    [SerializeField] GameObject VictoryScreen;
    public void OnTriggerEnter(Collider other)
    {
        StartCoroutine(clickvolume());
        VictoryScreen.SetActive(true);
    }

    IEnumerator clickvolume()
    {
        yield return new WaitForSeconds(0.5f);
        AudioListener.volume = 0;
    }
}
