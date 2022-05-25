using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
    void PickUp();
    void Drop(Vector3 location);
}
