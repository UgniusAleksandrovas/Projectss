using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceUI : MonoBehaviour
{
    [SerializeField] Vector3 positionOffset;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject overlay;

    private RectTransform canvasRect;
    Vector2 uiCanvasOffset;

    void Start()
    {
        canvasRect = canvas.GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(transform.position + positionOffset);
        Vector2 proportionalPosition = new Vector2(ViewportPosition.x * canvasRect.sizeDelta.x, ViewportPosition.y * canvasRect.sizeDelta.y);
        uiCanvasOffset = new Vector2(canvasRect.sizeDelta.x / 2f, canvasRect.sizeDelta.y / 2f);

        overlay.transform.localPosition = proportionalPosition - uiCanvasOffset;
    }
}
