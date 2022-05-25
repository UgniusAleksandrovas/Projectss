using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowShrink : MonoBehaviour
{
    [SerializeField] Vector3 minSize;
    [SerializeField] Vector3 originalSize;
    [SerializeField] Vector3 maxSize;
    [SerializeField] float changeSizeSpeed = 2f;

    Vector3 newSize;

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, originalSize, changeSizeSpeed * Time.deltaTime);
    }

    public void Burst(int amount)
    {
        if (amount > 0)
        {
            transform.localScale = maxSize;
        }
        else if (amount < 0)
        {
            transform.localScale = minSize;
        }
    }
}
