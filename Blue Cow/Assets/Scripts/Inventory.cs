using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Boosts {
    public int count;
    public GameObject ItemUI;
    public Text countUI;
    //public FillTimerUI timerUI;
    public Image lockUI;
    public PowerUp powerUpControl;
    public bool hasSeen;
}

public class Inventory : MonoBehaviour {

    public Boosts speedBoost;
    public Boosts jumpBoost;
    public Boosts healthBoost;
    public Boosts immortalBoost;
    public Boosts superBoost;

    [Header("ItemInfo")]
    public GameObject itemInfoUI;
    public TextMeshProUGUI itemTitle;
    public Image itemSprite;
    public TextMeshProUGUI itemInfo;

    AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        itemInfoUI.SetActive(false);
        UpdateUI();
        speedBoost.lockUI.enabled = true;
        jumpBoost.lockUI.enabled = true;
        healthBoost.lockUI.enabled = true;
        immortalBoost.lockUI.enabled = true;
        superBoost.lockUI.enabled = true;
    }

    public void UpdateUI() {
        speedBoost.ItemUI.SetActive(speedBoost.count > 0);
        if (speedBoost.count > 0) speedBoost.lockUI.enabled = false;
        speedBoost.countUI.text = "" + speedBoost.count;

        jumpBoost.ItemUI.SetActive(jumpBoost.count > 0);
        if (jumpBoost.count > 0) jumpBoost.lockUI.enabled = false;
        jumpBoost.countUI.text = "" + jumpBoost.count;

        healthBoost.ItemUI.SetActive(healthBoost.count > 0);
        if (healthBoost.count > 0) healthBoost.lockUI.enabled = false;
        healthBoost.countUI.text = "" + healthBoost.count;

        immortalBoost.ItemUI.SetActive(immortalBoost.count > 0);
        if (immortalBoost.count > 0) immortalBoost.lockUI.enabled = false;
        immortalBoost.countUI.text = "" + immortalBoost.count;

        superBoost.ItemUI.SetActive(superBoost.count > 0);
        if (superBoost.count > 0) superBoost.lockUI.enabled = false;
        superBoost.countUI.text = "" + superBoost.count;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            ActivateSpeedBoost();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            ActivateJumpBoost();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            ActivateHealthBoost();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            ActivateImmortalBoost();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            ActivateSuperBoost();
        }
    }

    public void ActivateSpeedBoost() {
        if (speedBoost.count > 0) {
            if (!speedBoost.powerUpControl.active) {
                //speedBoost.timerUI.StartTimer(speedBoost.powerUpControl.duration);
                speedBoost.powerUpControl.EnablePowerUp();
                //if (speedBoost.count == 1) speedBoost.powerUpControl = null;
                speedBoost.count--;
                UpdateUI();
                audioSource.Play();
            }
        }
    }

    public void ActivateJumpBoost() {
        if (jumpBoost.count > 0) {
            if (!jumpBoost.powerUpControl.active) {
                //jumpBoost.timerUI.StartTimer(jumpBoost.powerUpControl.duration);
                jumpBoost.powerUpControl.EnablePowerUp();
                //if (jumpBoost.count == 1) jumpBoost.powerUpControl = null;
                jumpBoost.count--;
                UpdateUI();
                audioSource.Play();
            }
        }
    }

    public void ActivateHealthBoost() {
        if (healthBoost.count > 0) {
            if (!healthBoost.powerUpControl.active) {
                //healthBoost.timerUI.StartTimer(healthBoost.powerUpControl.duration);
                healthBoost.powerUpControl.EnablePowerUp();
                //if (healthBoost.count == 1) healthBoost.powerUpControl = null;
                healthBoost.count--;
                UpdateUI();
                audioSource.Play();
            }
        }
    }

    public void ActivateImmortalBoost() {
        if (immortalBoost.count > 0) {
            if (!immortalBoost.powerUpControl.active) {
                //immortalBoost.timerUI.StartTimer(immortalBoost.powerUpControl.duration);
                immortalBoost.powerUpControl.EnablePowerUp();
                //if (immortalBoost.count == 1) immortalBoost.powerUpControl = null;
                immortalBoost.count--;
                UpdateUI();
                audioSource.Play();
            }
        }
    }

    public void ActivateSuperBoost() {
        if (superBoost.count > 0) {
            if (!superBoost.powerUpControl.active) {
                //superBoost.timerUI.StartTimer(superBoost.powerUpControl.duration);
                superBoost.powerUpControl.EnablePowerUp();
                //if (superBoost.count == 1) superBoost.powerUpControl = null;
                superBoost.count--;
                UpdateUI();
                audioSource.Play();
            }
        }
    }

    public void HideItemInfo() {
        itemInfoUI.SetActive(false);
        Time.timeScale = 1;
    }
}
