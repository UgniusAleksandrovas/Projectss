using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthPotion : Useitems
{ 

    private HealthBarController healthBar;

    private void Awake()
    {
        healthBar = FindObjectOfType<HealthBarController>();
    }

    public override void Initialise()
    {
        base.Initialise();
    }

    public override void Use()
    {
        base.Use();
        FindObjectOfType<audioManager>().Play("drinkPotion");
        healthBar.TakeDamage(-20);
    }
}
