using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectButton : MainMenuButton, IMenuButton
{
    [Header("Camera Settings")]
    [SerializeField] float moveCameraDelay = 2f;
    [SerializeField] GameObject camera;
    [SerializeField] Transform originalCamTransform;
    [SerializeField] Transform newCamTransform;
    [SerializeField] float moveTime = 1f;

    [Header("GameObject Settings")]
    [SerializeField] float enableDelay = 1f;
    [SerializeField] GameObject objectToEnable;

    public void OnButtonUse()
    {
        StartCoroutine(MoveCamera());
        StartCoroutine(EnableObject());
    }

    IEnumerator MoveCamera()
    {
        yield return new WaitForSeconds(moveCameraDelay);
        StartCoroutine(LerpObjectPosition(camera, newCamTransform.position, moveTime));
        StartCoroutine(LerpObjectRotation(camera, newCamTransform.rotation, moveTime));
    }

    public void ReturnCamera()
    {
        StartCoroutine(LerpObjectPosition(camera, originalCamTransform.position, moveTime));
        StartCoroutine(LerpObjectRotation(camera, originalCamTransform.rotation, moveTime));
        StartCoroutine(ReEnableButtons(moveTime));
    }

    IEnumerator ReEnableButtons(float delay)
    {
        yield return new WaitForSeconds(delay);
        manager.SetAllButtonsUsable(true);
    }

    IEnumerator EnableObject()
    {
        yield return new WaitForSeconds(enableDelay);
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }
    }

    IEnumerator LerpObjectPosition(GameObject targetObject, Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = targetObject.transform.position;
        while (time < duration)
        {
            targetObject.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        targetObject.transform.position = targetPosition;
    }

    IEnumerator LerpObjectRotation(GameObject targetObject, Quaternion targetRotation, float duration)
    {
        float time = 0;
        Quaternion startRotation = targetObject.transform.rotation;
        while (time < duration)
        {
            targetObject.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        targetObject.transform.rotation = targetRotation;
    }
}
