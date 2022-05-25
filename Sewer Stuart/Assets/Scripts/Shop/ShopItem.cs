using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public int ID;
    public int cost;
    [SerializeField] Text[] costText;
    [SerializeField] GameObject equipButton;
    [SerializeField] GameObject purchaseButton;
    [SerializeField] GameObject transactionFailed;

    public void UpdateCost()
    {
        for (int i = 0; i < costText.Length; i++)
        {
            costText[i].text = cost.ToString();
            if (cost <= 0f)
            {
                costText[i].text = "Free";
            }
        }
    }

    public void Purchased()
    {
        purchaseButton.SetActive(false);
    }

    public void Equip(bool equipped)
    {
        equipButton.SetActive(!equipped);
    }

    public void TransactionFailed()
    {
        transactionFailed.SetActive(true);
    }
}
