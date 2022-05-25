using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseScript : MonoBehaviour
{
   public GameObject footsteps;
    public void OnMouseDragInventoryOpen()
    {
        footsteps.SetActive(false);
        Time.timeScale = 0f;
    }
    public void OnMouseDragInventoryClose()
    {
        Time.timeScale = 1f;
        footsteps.SetActive(true);
    }

}
