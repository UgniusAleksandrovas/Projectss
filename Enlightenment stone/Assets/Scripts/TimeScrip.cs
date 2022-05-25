using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScrip : MonoBehaviour
{

    public int isRunning = 1;
    public int numberOfSeconds;
    void Update()
    {
        if(isRunning ==1)
        {
            StartCoroutine(Wait());
        }
    }

    public IEnumerator Wait()
    {
        isRunning = 0;
        yield return new WaitForSeconds(numberOfSeconds);
        GetComponent<Spawner>().enabled = true;
        isRunning = 1;
    }
}
