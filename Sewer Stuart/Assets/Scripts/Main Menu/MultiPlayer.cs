using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayer : MonoBehaviour
{
    public GameObject multiPlayer;

    void OnMouseOver()
    {
        multiPlayer.SetActive(true);
    }

    void OnMouseExit()
    {
        multiPlayer.SetActive(false);
    }
}
