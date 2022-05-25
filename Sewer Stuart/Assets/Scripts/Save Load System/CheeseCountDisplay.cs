using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheeseCountDisplay : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] Text display;

    void Start()
    {
        DisplayCountToUI();
    }

    void OnEnable()
    {
        Inventory.OnCheeseCountChanged += DisplayCountToUI;
    }

    void OnDisable()
    {
        Inventory.OnCheeseCountChanged -= DisplayCountToUI;
    }

    void DisplayCountToUI()
    {
        display.text = inventory.cheeseCount.ToString();
    }
}
