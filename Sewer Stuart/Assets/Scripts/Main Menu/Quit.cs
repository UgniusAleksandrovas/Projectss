using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    public GameObject quit;
    void OnMouseOver()
    {
        quit.SetActive(true);
    }

    void OnMouseExit()
    { 
        quit.SetActive(false);
    }
}
