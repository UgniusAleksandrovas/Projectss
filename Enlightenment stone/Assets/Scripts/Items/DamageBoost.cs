using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageBoost : Useitems
{
    [SerializeField] float damageMultiplier = 2f;
    [SerializeField] float duration = 2f;
    PlayerScript ps;

    public override void Initialise()
    {
        ps = FindObjectOfType<PlayerScript>();
        base.Initialise();
    }

    public override void Use()
    {
        StartCoroutine(StartUse());
        inventory.inventorySlots[i].slot.transform.GetChild(1).GetComponent<Button>().enabled = false;
        inventory.inventorySlots[i].isFull = false;
    }

    IEnumerator StartUse()
    {
        int oldDamage = ps.damage;
        ps.damage = Mathf.RoundToInt(ps.damage * damageMultiplier);
        yield return new WaitForSeconds(duration);
        ps.damage = oldDamage;
        Destroy(gameObject);
    }
}
