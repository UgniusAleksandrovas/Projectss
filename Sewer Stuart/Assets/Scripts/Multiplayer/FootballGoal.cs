using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Multiplayer
{
    public class FootballGoal : MonoBehaviour
    {
        [SerializeField] Teams team;

        void OnTriggerEnter(Collider other)
        {
            Football ball = other.GetComponent<Football>();
            if (ball != null)
            {
                ball.AddScore(team);
            }
        }
    }
}
