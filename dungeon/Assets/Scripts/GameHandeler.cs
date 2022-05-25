using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameHandeler : MonoBehaviour
{
    public HealthBar healthBar;
    private void Start()
    {
        HealthSystem healthSystem = new HealthSystem(100);

        healthBar.Setup(healthSystem);
        Debug.Log("Health: " + healthSystem.GetHealthPercent());
        healthSystem.Damage(10);
        Debug.Log("Health: " + healthSystem.GetHealthPercent());
        healthSystem.Heal(5);
        Debug.Log("Health: " + healthSystem.GetHealthPercent());
    }
}
