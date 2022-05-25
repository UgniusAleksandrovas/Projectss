using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomScale : MonoBehaviour
{
    [SerializeField] bool preserveAspectRatio;
    [SerializeField] bool multiplyScale;
    [SerializeField] Vector3 scaleMin;
    [SerializeField] Vector3 scaleMax;

    Vector3 newScale;

    void Start()
    {
        if (multiplyScale)
        {
            newScale = transform.localScale;

            if (preserveAspectRatio)
            {
                newScale *= Random.Range(scaleMin.x, scaleMax.x);
            }

            newScale.x *= Random.Range(scaleMin.x, scaleMax.x);
            newScale.y *= Random.Range(scaleMin.y, scaleMax.y);
            newScale.z *= Random.Range(scaleMin.z, scaleMax.z);
        }
        else
        {
            if (preserveAspectRatio)
            {
                newScale = Vector3.one * Random.Range(scaleMin.x, scaleMax.x);
            }

            newScale = new Vector3(
                Random.Range(scaleMin.x, scaleMax.x),
                Random.Range(scaleMin.y, scaleMax.y),
                Random.Range(scaleMin.z, scaleMax.z)
            );
        }

        transform.localScale = newScale;
    }
}
