using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class victoryScript : MonoBehaviour
{
    public EnemyHealth boss;
    public EnemyHealth boss2;
    public EnemyHealth boss3;
    public GameObject Win;


    void Update()
    {
        if (boss == null && boss2 == null && boss3 == null)
        {
            Win.SetActive(true);
        }
    }
}