using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using Unity.Collections;

namespace Multiplayer
{
    public class PlayerConnectToLobby : NetworkBehaviour
    {
        public NetworkVariable<FixedString64Bytes> playerNetworkName = new NetworkVariable<FixedString64Bytes>();

        private InputField username;

        void Awake()
        {
            username = FindObjectOfType<PasswordNetworkManager>().usernameInputField;
        }

        public override void OnNetworkSpawn()
        {
            if (IsClient && IsOwner)
            {
                if (username.text != "")
                {
                    FixedString64Bytes customName = username.text;
                    ChangePlayerNameServerRpc(customName);
                }
                else
                {
                    ChangePlayerNameServerRpc($"Player {OwnerClientId}");
                }
                StartCoroutine(AddPlayerToLobby());
            }
        }

        public override void OnNetworkDespawn()
        {
            Lobby lobby = FindObjectOfType<Lobby>();
            if (lobby != null)
            {
                FindObjectOfType<Lobby>().DisconnectPlayerFromLobby(OwnerClientId);
            }
        }

        [ServerRpc]
        public void ChangePlayerNameServerRpc(FixedString64Bytes newName)
        {
            playerNetworkName.Value = newName;
        }

        IEnumerator AddPlayerToLobby()
        {
            yield return new WaitForSeconds(0.1f);
            FindObjectOfType<Lobby>().ConnectPlayerToLobby(OwnerClientId, playerNetworkName.Value);
            yield return new WaitForSeconds(0.1f);
            GetComponent<PlayerHud>().SetUI();
        }
    }
}
