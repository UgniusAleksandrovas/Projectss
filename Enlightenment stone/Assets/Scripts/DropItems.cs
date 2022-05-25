using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItems : MonoBehaviour
{
    public GameObject Drop;


    public void Die()
    {
        Destroy(gameObject);
        Instantiate(Drop, transform.position, transform.rotation);
    }
}
