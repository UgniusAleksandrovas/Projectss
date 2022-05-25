using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissapear : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {

    }
    public void OnMouseDown()
    {
        gameObject.SetActive(false); // destroys object on click
    }
}

