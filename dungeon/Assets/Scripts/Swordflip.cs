using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordflip : MonoBehaviour
{
    public Vector2 dir;
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (dir.x < 0)
        {
            transform.localPosition = new Vector2(-0.1f, -0.1f);
            GetComponent<SpriteRenderer>().flipY = false;
        }
        else if (dir.x > 0)
        {
            transform.localPosition = new Vector2(0.1f, 0.1f);
            GetComponent<SpriteRenderer>().flipY = true;
        }
    }
}
