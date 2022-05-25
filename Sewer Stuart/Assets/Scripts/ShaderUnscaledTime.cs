using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderUnscaledTime : MonoBehaviour
{
    [SerializeField] Material mat;

    void Update()
    {
        if (mat == null)
        {
            return;
        }
        mat.SetFloat("_UnscaledTime", Time.unscaledTime);
    }
}
