using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

namespace Multiplayer
{
    public class Football : NetworkBehaviour
    {
        [Header("Team Settings")]
        NetworkVariable<int> redScore;
        [SerializeField] Text redScoreText;
        [Space(10)]
        NetworkVariable<int> blueScore;
        [SerializeField] Text blueScoreText;

        [Header("Gameplay Settings")]
        [SerializeField] Transform ballRespawnPos;
        Rigidbody rb;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void AddScore(Teams team)
        {
            AddScoreServerRpc(team);
            ResetBallPosition();
        }

        [ServerRpc]
        void AddScoreServerRpc(Teams team)
        {
            if (team == Teams.red)
            {
                redScore.Value++;
            }
            else if (team == Teams.blue)
            {
                blueScore.Value++;
            }
            UpdateScoreboard();
        }

        void UpdateScoreboard()
        {
            redScoreText.text = redScore.Value.ToString();
            blueScoreText.text = blueScore.Value.ToString();
        }

        void ResetBallPosition()
        {
            transform.position = ballRespawnPos.position;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        public void ResetGame()
        {
            ResetGameServerRpc();
            ResetBallPosition();
        }

        [ServerRpc]
        void ResetGameServerRpc()
        {
            redScore.Value = 0;
            blueScore.Value = 0;
            UpdateScoreboard();
        }
    }

    [System.Serializable]
    public enum Teams
    {
        red,
        blue
    }
}
