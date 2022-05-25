using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHop : MonoBehaviour
{
    [SerializeField] LayerMask buttonLayer;
    [SerializeField] AnimationCurve jumpCurve;
    [SerializeField] float jumpHeight = 1f;
    [SerializeField] float jumpChargeTime = 0.8f;
    [SerializeField] float jumpTime = 1f;
    [SerializeField] float turnSpeed = 4f;

    Vector3 endPos;
    Quaternion endRot;

    public Animator anim;

    void Update()
    {
        Vector3 lookPos = endPos - transform.position;
        lookPos.y = 0f;
        if (lookPos.magnitude <= 0)
        {
            endRot = transform.rotation;
        }
        else
        {
            endRot = Quaternion.LookRotation(lookPos);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, endRot, turnSpeed * Time.deltaTime);
    }

    public void StartJump()
    {
        anim.SetTrigger("Jump");
        StartCoroutine(WaitToMove());
    }

    public void SetTargetPosition(Vector3 pos)
    {
        endPos = pos;
    }

    IEnumerator WaitToMove()
    {
        yield return new WaitForSeconds(jumpChargeTime);
        StartCoroutine(LerpPosition(endPos, jumpTime));
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            Vector3 newPos = Vector3.Lerp(startPosition, targetPosition, time / duration);
            float yPos = jumpCurve.Evaluate(time / duration) * jumpHeight;
            newPos.y = startPosition.y + yPos;
            transform.position = newPos;
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
}
