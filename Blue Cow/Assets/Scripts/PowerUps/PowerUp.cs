using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    public bool active;
    public GameObject[] buffUI;
    Transform buffsCanvas;

    [Header("Attributes")]
    public float duration = 1f;

    void Start() {
        buffsCanvas = FindObjectOfType<Canvas>().transform.Find("Buffs");
    }

    public void EnablePowerUp() {
        StartCoroutine(DoPowerUp());

        foreach (GameObject buffObj in buffUI) {
            GameObject buff = Instantiate(buffObj);
            buff.transform.SetParent(buffsCanvas);
            buff.transform.localScale = Vector3.one;
            buff.transform.GetComponentInChildren<FillTimerUI>().StartTimer(duration);
        }
    }

    public IEnumerator DoPowerUp() {
        PlayerStats stats = FindObjectOfType<PlayerStats>();

        PowerUpStat(stats);
        active = true;

        yield return new WaitForSeconds(duration);

        PowerDownStat(stats);
        active = false;
    }

    public virtual void PowerUpStat(PlayerStats stats) {
        //change a stat for power up
    }

    public virtual void PowerDownStat(PlayerStats stats) {
        //reset stat when power up runs out
    }
}