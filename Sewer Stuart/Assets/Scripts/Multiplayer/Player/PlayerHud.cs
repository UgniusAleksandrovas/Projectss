using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Multiplayer
{
    public class PlayerHud : NetworkBehaviour
    {
        private string playerName;

        private GameObject localPlayerOverlay;
        private Canvas canvas;
        private RectTransform canvasRect;
        private Vector2 uiCanvasOffset;
        [SerializeField] bool autoPositionHUD = true;
        private Vector3 uiPlayerOffset = new Vector3(0f, 0.9f, 0f);

        private void Awake()
        {
            canvas = gameObject.GetComponentInChildren<Canvas>();
            canvasRect = canvas.GetComponent<RectTransform>();

            localPlayerOverlay = canvas.transform.Find("PlayerName").gameObject;
        }

        private void Start()
        {
            if (!autoPositionHUD)
            {
                SetUI();
            }
        }

        public void SetUI()
        {
            playerName = FindObjectOfType<Lobby>().GetPlayerNameFromId(OwnerClientId).ToString();

            Text localPlayerName = gameObject.GetComponentInChildren<Text>();
            localPlayerName.text = playerName;
        }

        public void Update()
        {
            if (autoPositionHUD)
            {
                Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(transform.position + uiPlayerOffset);
                Vector2 proportionalPosition = new Vector2(ViewportPosition.x * canvasRect.sizeDelta.x, ViewportPosition.y * canvasRect.sizeDelta.y);
                uiCanvasOffset = new Vector2(canvasRect.sizeDelta.x / 2f, canvasRect.sizeDelta.y / 2f);

                localPlayerOverlay.transform.localPosition = proportionalPosition - uiCanvasOffset;
            }
        }
    }
}
