using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void GameOver();
    public static event GameOver OnGameOver;

    public static void SetGameOver()
    {
        if (OnGameOver != null)
        {
            OnGameOver();
        }
    }
}
