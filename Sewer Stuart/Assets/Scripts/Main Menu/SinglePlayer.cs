using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SinglePlayer : MonoBehaviour
{
    public GameObject singlePlayer;

    void OnMouseOver()
    {
        singlePlayer.SetActive(true);
    }

    void OnMouseExit()
    {
        singlePlayer.SetActive(false);
    }
}
