using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    public override void UpdateHealth() {
        healthBarUI.fillAmount = GetHealthPercent();
        if (health <= 0) {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
