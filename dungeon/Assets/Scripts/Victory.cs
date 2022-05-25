using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    public Health health1;
    public Health health2;
    public GameObject Win;


    void Update()
    {
        if(health1 == null && health2 == null)
        {
            Win.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
