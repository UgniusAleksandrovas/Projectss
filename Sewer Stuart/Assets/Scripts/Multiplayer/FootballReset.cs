using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Multiplayer
{
    public class FootballReset : MonoBehaviour
    {
        [SerializeField] Football footballManager;
        [SerializeField] float standDuration = 2f;
        [SerializeField] GameObject UI;
        [SerializeField] Image standDurationUI;
        float standTime;

        void OnTriggerStay(Collider other)
        {
            PlayerControllerLobby player = other.GetComponent<PlayerControllerLobby>();
            if (player != null)
            {
                if (standTime < standDuration)
                {
                    standTime += Time.deltaTime;
                    UI.SetActive(true);
                    standDurationUI.fillAmount = standTime / standDuration;
                }
                else
                {
                    UI.SetActive(false);
                    footballManager.ResetGame();
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            PlayerControllerLobby player = other.GetComponent<PlayerControllerLobby>();
            if (player != null)
            {
                standTime = 0f;
                UI.SetActive(false);
                standDurationUI.fillAmount = 0f;
            }
        }
    }
}
