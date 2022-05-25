using System;
using System.Text;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode.Transports.UNET;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Multiplayer
{
    public class PasswordNetworkManager : MonoBehaviour
    {
        public string ipAddress = "127.0.0.1";
        UNetTransport transport;

        public InputField usernameInputField;
        [SerializeField] InputField passwordInputField;
        [SerializeField] GameObject passwordEntryUI;
        [SerializeField] GameObject leaveButton;
        [SerializeField] GameObject readyButton;
        [SerializeField] GameObject startButton;

        [SerializeField] PlayerSpawner playerSpawner;

        private void Start()
        {
            NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
        }

        private void OnDestroy()
        {
            if (NetworkManager.Singleton == null)
            {
                return;
            }

            NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
        }

        public void Host()
        {
            //NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.StartHost();
        }

        public void Client()
        {
            transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
            transport.ConnectAddress = ipAddress;
            NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(passwordInputField.text);
            NetworkManager.Singleton.StartClient();
        }

        public void Leave()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
                StartCoroutine(WaitToDisconnectHost());
            }
            else if (NetworkManager.Singleton.IsClient)
            {
                NetworkManager.Singleton.Shutdown();
            }/*
            passwordEntryUI.SetActive(true);
            leaveButton.SetActive(false);
            readyButton.SetActive(false);
            startButton.SetActive(false);*/
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private IEnumerator WaitToDisconnectHost()
        {
            FindObjectOfType<Lobby>().HostDisconnectServerRpc();
            yield return new WaitForSeconds(0.5f);
            NetworkManager.Singleton.Shutdown();
        }

        private void HandleServerStarted()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                HandleClientConnected(NetworkManager.Singleton.LocalClientId);
            }
        }

        private void HandleClientConnected(ulong clientId)
        {
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                passwordEntryUI.SetActive(false);
                leaveButton.SetActive(true);
                playerSpawner.StartCoroutine(playerSpawner.SpawnAllPlayerObjects());
            }
        }

        private void HandleClientDisconnect(ulong clientId)
        {
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                passwordEntryUI.SetActive(true);
                leaveButton.SetActive(false);
            }
        }

        private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
        {
            string password = Encoding.ASCII.GetString(connectionData);

            bool approveConnection = password == passwordInputField.text;

            callback(true, null, approveConnection, null, null);
        }

        public void IPAddressChanged(string newAddress)
        {
            ipAddress = newAddress;
        }
    }
}
