using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverDisplay : MonoBehaviour
{
    [SerializeField] GameObject display;

    void OnMouseEnter()
    {
        display.SetActive(true);
    }

    void OnMouseExit()
    {
        display.SetActive(false);
    }
}
