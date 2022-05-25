using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeManager : MonoBehaviour {

    public int grenadeCount;
    public Image[] grenadeIcons = new Image[3];
    public GameObject[] grenadeModels = new GameObject[3];
    public GameObject grenadePrefab;
    public Transform grenadeSpawn;

    public float grenadeThrowRate;
    public float grenadeThrowForce;

    private float waitTillThrow;
    private bool thrown;

    // Start is called before the first frame update
    void Start() {
        UpdateSlots();
    }

    // Update is called once per frame
    void Update() {
        if (waitTillThrow <= 0 && grenadeCount > 0) {
            if (Input.GetAxis("Grenade") != 0) {
                if (thrown == false) {
                    GameObject grenade = Instantiate(grenadePrefab, grenadeSpawn.position, grenadeSpawn.rotation);
                    grenade.GetComponent<Rigidbody>().AddForce((grenade.transform.forward * grenadeThrowForce) + (Vector3.up * grenadeThrowForce / 4));
                    grenade.GetComponent<Rigidbody>().AddTorque(grenade.transform.right * grenadeThrowForce / 2);
                    waitTillThrow = 1;
                    grenadeCount -= 1;
                    UpdateSlots();
                }
                thrown = true;
            }
            else {
                thrown = false;
            }
        }
        waitTillThrow -= Time.deltaTime * grenadeThrowRate;
        waitTillThrow = Mathf.Clamp(waitTillThrow, 0, 1);
    }

    public void UpdateSlots() {
        for (int i = 0; i < grenadeIcons.Length; i++) {
            if (grenadeCount - 1 >= i) {
                grenadeIcons[i].enabled = true;
                grenadeModels[i].SetActive(true);
            }
            else {
                grenadeIcons[i].enabled = false;
                grenadeModels[i].SetActive(false);
            }
        }
    }
}
