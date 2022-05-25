using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    public class CameraControllerLobby : MonoBehaviour
    {
        [SerializeField] GameObject target;
        [SerializeField] float followSpeed = 1f;
        [SerializeField] Vector3Bool freezeAxis = new Vector3Bool(false, true, true);
        [SerializeField] Float4 edgePercent = new Float4(0.5f, 0.5f, 0.5f, 0.5f);
        [SerializeField] Vector3Range constraintPosition = new Vector3Range(new Vector3(-5f, 0f, -5f), new Vector3(5f, 5f, 5f));

        Camera cam;
        bool move;
        Vector3 startPos;
        Vector3 endPos;

        private void Awake()
        {
            cam = Camera.main;
        }

        void LateUpdate()
        {
            if (target == null)
            {
                return;
            }
            startPos = transform.position;

            Vector2 playerScreenPos = cam.WorldToScreenPoint(target.transform.position);
            Float4 edgePixels = new Float4(
                Screen.height * edgePercent.top,
                Screen.height * edgePercent.bottom,
                Screen.width * edgePercent.left,
                Screen.width * edgePercent.right
            );
            move = playerScreenPos.x < edgePixels.left ||
                playerScreenPos.x > Screen.width - edgePixels.right ||
                playerScreenPos.y < edgePixels.bottom ||
                playerScreenPos.y > Screen.height - edgePixels.top;

            if (move)
            {
                endPos = new Vector3(
                    !freezeAxis.x ? target.transform.position.x : transform.position.x,
                    !freezeAxis.y ? target.transform.position.y : transform.position.y,
                    !freezeAxis.z ? target.transform.position.z : transform.position.z
                );
            }

            endPos.x = Mathf.Clamp(endPos.x, constraintPosition.min.x, constraintPosition.max.x);
            endPos.y = Mathf.Clamp(endPos.y, constraintPosition.min.y, constraintPosition.max.y);
            endPos.z = Mathf.Clamp(endPos.z, constraintPosition.min.z, constraintPosition.max.z);

            transform.position = Vector3.Lerp(startPos, endPos, followSpeed * Time.fixedDeltaTime);
        }

        public void SetTarget(GameObject newTarget)
        {
            target = newTarget;
        }
    }
}

[System.Serializable]
public struct Vector3Bool
{
    public bool x;
    public bool y;
    public bool z;

    public Vector3Bool(bool x, bool y, bool z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

[System.Serializable]
public struct Float4
{
    public float top;
    public float bottom;
    public float left;
    public float right;

    public Float4(float top, float bottom, float left, float right)
    {
        this.top = top;
        this.bottom = bottom;
        this.left = left;
        this.right = right;
    }
}

[System.Serializable]
public struct Vector3Range
{
    public Vector3 min;
    public Vector3 max;

    public Vector3Range(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }
}
