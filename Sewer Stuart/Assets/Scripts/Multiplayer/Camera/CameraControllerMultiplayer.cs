using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    public class CameraControllerMultiplayer : MonoBehaviour
    {
        [SerializeField] GameObject target;
        [SerializeField] float rotateSpeed;

        float lerpTime = 1f;
        float currentLerpTime;
        Vector3 startPos;
        Vector3 endPos;

        private bool canMove = true;
        private bool canRotate = true;

        void Start()
        {
            canRotate = true;
        }

        void LateUpdate()
        {
            if (target == null)
            {
                return;
            }
            if (canMove)
            {
                startPos = transform.position;
                endPos = target.transform.position;
                currentLerpTime += Time.deltaTime;
                if (currentLerpTime > lerpTime)
                {
                    currentLerpTime = lerpTime;
                }
                float perc = currentLerpTime / lerpTime;
                transform.position = Vector3.Lerp(startPos, endPos, perc);
            }
            if (canRotate)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, target.transform.rotation, rotateSpeed * Time.deltaTime);
            }
        }

        public void SetTarget(GameObject newTarget)
        {
            target = newTarget;
        }
    }
}
