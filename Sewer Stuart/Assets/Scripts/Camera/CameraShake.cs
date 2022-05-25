using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] Vector3 positionInfluence = new Vector3(1, 1, 0);
    [SerializeField] Vector3 rotationInfluence = Vector3.one;

    IEnumerator cameraShake;

    public void ShakeOnce(float magnitude, float roughness, float fadeInTime, float fadeOutTime)
    {
        if (cameraShake != null)
        {
            StopCoroutine(cameraShake);
        }
        cameraShake = Shake(magnitude, roughness, fadeInTime, fadeOutTime);
        StartCoroutine(cameraShake);
    }

    IEnumerator Shake(float magnitude, float roughness, float fadeInTime, float fadeOutTime)
    {
        float duration = fadeInTime + fadeOutTime;
        float elapsedTime = 0f;

        float fadeMultiplier = 0f;

        while (elapsedTime < duration)
        {
            if (elapsedTime < fadeInTime)
            {
                fadeMultiplier = elapsedTime / fadeInTime;
            }
            else
            {
                fadeMultiplier = 1 - (elapsedTime - fadeInTime) / fadeOutTime;
            }

            float x = elapsedTime * roughness;
            float y = elapsedTime * roughness + roughness;
            float z = elapsedTime * roughness + elapsedTime;

            float shakeValueX = NoiseSample(x, x, magnitude * fadeMultiplier);
            float shakeValueY = NoiseSample(y, y, magnitude * fadeMultiplier);
            float shakeValueZ = NoiseSample(z, z, magnitude * fadeMultiplier);

            transform.localPosition = new Vector3(shakeValueX * positionInfluence.x, shakeValueY * positionInfluence.y, shakeValueZ * positionInfluence.z);
            transform.localEulerAngles = new Vector3(shakeValueX * rotationInfluence.x, shakeValueY * rotationInfluence.y, shakeValueZ * rotationInfluence.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
    }

    float NoiseSample(float x, float y, float strength)
    {
        float shakeValue = (Mathf.PerlinNoise(x, y) - 0.5f) * strength;

        return shakeValue;
    }
}
