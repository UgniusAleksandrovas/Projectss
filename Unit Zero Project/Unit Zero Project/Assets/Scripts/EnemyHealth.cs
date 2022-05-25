using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health {

    public GameObject ragdoll;
    public GameObject ItemDrop;
    private bool spawned;

    private SoldierAI soldierScript;
    private KillCounter killCountScript;

    // Start is called before the first frame update
    void Start() {
        maxHealth = health;
        soldierScript = GetComponent<SoldierAI>();
        killCountScript = FindObjectOfType<KillCounter>();
        spawned = false;
    }

    public override void UpdateHealth() {
        if (health <= 0) {
            soldierScript.canShoot = false;
            //StartCoroutine(StartDeathSequence());
            StartDeathSequence();
        }
    }

    public IEnumerator Burn(float burntime) {
        soldierScript.fire.SetActive(true);
        yield return new WaitForSeconds(burntime);
        if (this != null) {
            soldierScript.fire.SetActive(false);
        }
    }

    /*
    private IEnumerator StartDeathSequence() {
        if (ragdoll != null) {
            GameObject rd = Instantiate(ragdoll, transform.position, transform.rotation);
            FindObjectOfType<RagdollManager>().AddRagdoll(rd.GetComponent<Ragdoll>());
        }
        if (ItemDrop != null) Instantiate(ItemDrop, transform.position + Vector3.up, Quaternion.identity);
        yield return null;
        Destroy(gameObject);
    }
    */
    public void StartDeathSequence() {
        if (spawned == false) {
            if (ragdoll != null) {
                GameObject rd = Instantiate(ragdoll, transform.position, transform.rotation);
                FindObjectOfType<RagdollManager>().AddRagdoll(rd.GetComponent<Ragdoll>());
            }
            if (ItemDrop != null) Instantiate(ItemDrop, transform.position + Vector3.up, Quaternion.identity);
            spawned = true;
        }
        Destroy(gameObject);
    }
    private void OnDestroy() {
        killCountScript.kills++;
        killCountScript.UpdateKills();
    }
}
