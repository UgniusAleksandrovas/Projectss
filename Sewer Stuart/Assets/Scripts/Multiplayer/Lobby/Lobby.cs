using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lobby : NetworkBehaviour
{    
    [Header("References")]
    [SerializeField] private PlayerCardLobby[] lobbyPlayerCards;
    [SerializeField] private Button startGameButton;

    private NetworkList<LobbyPlayerState> lobbyPlayers;

    private void Awake()
    {
        lobbyPlayers = new NetworkList<LobbyPlayerState>();
        DontDestroyOnLoad(gameObject);
    }

    public override void OnNetworkSpawn()
    {
        
        if (IsClient)
        {
            lobbyPlayers.OnListChanged += HandleLobbyPlayersStateChanged;
        }
        /*
        if (IsServer)
        {
            startGameButton.gameObject.SetActive(true);

            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;

            foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                HandleClientConnected(client.ClientId);
            }
        }*/

        if (IsHost)
        {
            ShowStartGameButton();
        }
    }

    private bool IsEveryoneReady()
    {
        if (lobbyPlayers.Count < 2)
        {
            return false;
        }

        foreach (var player in lobbyPlayers)
        {
            if (!player.IsReady)
            {
                return false;
            }
        }

        return true;
    }

    private void HandleClientDisconnect(ulong clientId)
    {
        for (int i = 0; i < lobbyPlayers.Count; i++)
        {
            if (lobbyPlayers[i].ClientId == clientId)
            {
                lobbyPlayers.RemoveAt(i);
                break;
            }
        }
    }

    public void ConnectPlayerToLobby(ulong clientId, FixedString64Bytes name)
    {
        ConnectPlayerToLobbyServerRpc(clientId, name);
        //UpdatePlayerCardsServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ConnectPlayerToLobbyServerRpc(ulong clientId, FixedString64Bytes name)
    {
        for (int i = 0; i < lobbyPlayers.Count; i++)
        {
            if (lobbyPlayers[i].ClientId == clientId)
            {
                return;
            }
        }
        lobbyPlayers.Add(
            new LobbyPlayerState(clientId, name, false)
        );
    }

    public void DisconnectPlayerFromLobby(ulong clientId)
    {
        /*
        if (IsHost)
        {
            Debug.Log("i am the host");
            //HostDisconnectServerRpc();
        }
        */
        DisconnectPlayerFromLobbyServerRpc(clientId);
        //UpdatePlayerCardsServerRpc();

        //HostDisconnectServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DisconnectPlayerFromLobbyServerRpc(ulong clientId)
    {
        for (int i = 0; i < lobbyPlayers.Count; i++)
        {
            if (lobbyPlayers[i].ClientId == clientId)
            {
                lobbyPlayers.RemoveAt(i);
                break;
            }
        }
    }

    [ServerRpc]
    public void HostDisconnectServerRpc()
    {
        HostDisconnectClientRpc();
    }

    [ClientRpc]
    private void HostDisconnectClientRpc()
    {
        //FindObjectOfType<PasswordNetworkManager>().Leave();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnReadyClicked()
    {
        ToggleReadyServerRpc();
        UpdatePlayerCardsServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ToggleReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        for (int i = 0; i < lobbyPlayers.Count; i++)
        {
            if (lobbyPlayers[i].ClientId == serverRpcParams.Receive.SenderClientId)
            {
                lobbyPlayers[i] = new LobbyPlayerState(
                    lobbyPlayers[i].ClientId,
                    lobbyPlayers[i].PlayerName,
                    !lobbyPlayers[i].IsReady
                );
            }
        }
    }

    public void StartGame()
    {
        StartGameServerRpc();
    }

    [ServerRpc]
    private void StartGameServerRpc()
    {
        if (!IsEveryoneReady()) { return; }

        //SceneManager.LoadScene("MultiplayerScene");
        NetworkManager.Singleton.SceneManager.LoadScene("MultiplayerScene", LoadSceneMode.Single);
    }

    public void ShowStartGameButton()
    {
        startGameButton.interactable = false;
        startGameButton.gameObject.SetActive(true);
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdatePlayerCardsServerRpc()
    {
        UpdatePlayerCardsClientRpc();
    }

    [ClientRpc]
    private void UpdatePlayerCardsClientRpc()
    {
        UpdatePlayerCards();
    }

    private void UpdatePlayerCards()
    {
        for (int i = 0; i < lobbyPlayerCards.Length; i++)
        {
            if (lobbyPlayers.Count > i)
            {
                lobbyPlayerCards[i].UpdateDisplay(lobbyPlayers[i]);
            }
            else
            {
                lobbyPlayerCards[i].DisableDisplay();
            }
        }
        
        if (IsHost)
        {
            startGameButton.interactable = IsEveryoneReady();
        }
    }

    private void HandleLobbyPlayersStateChanged(NetworkListEvent<LobbyPlayerState> lobbyState)
    {
        UpdatePlayerCards();
    }

    public FixedString64Bytes GetPlayerNameFromId(ulong clientId)
    {
        if (lobbyPlayers.Count <= 0)
        {
            return new FixedString64Bytes("No players found");
        }
        for (int i = 0; i < lobbyPlayers.Count; i++)
        {
            if (lobbyPlayers[i].ClientId == clientId)
            {
                return lobbyPlayers[i].PlayerName;
            }
        }
        return new FixedString64Bytes("Missing Name");
    }
}
