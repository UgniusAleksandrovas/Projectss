using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public Transform player;

    public float MouseSens = 5;

    private float x = 0;
    private float y = 0;

    private void Update()
    {
        x += -Input.GetAxis("Mouse Y") * MouseSens;
        y += Input.GetAxis("Mouse X") * MouseSens;

        x = Mathf.Clamp(x, -90, 90);

        transform.localRotation = Quaternion.Euler(x, 0, 0);
        player.transform.localRotation = Quaternion.Euler(0, y,0);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
        }
    }
}
