using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public bool showInventory;
    public GameObject inventoryUI;
    public List<Transform> slots;
    public LayerMask inventoryLayer;

    //public string startingWeapon;
    private Object[] allGuns;
    public List<GameObject> weapons;

    public bool replaceWeapon;
    public GameObject newWeapon;
    public GameObject replaceWeaponItem;
    public List<GameObject> weaponItems;

    public GameObject pickUpHUD;
    public Text displayItemName;

    private Animator prevAnim;

    private FPSController thePlayer;
    private int inventorySize;
    public int selectedSlot;
    public int activeSlot;

    // Start is called before the first frame update
    void Start() {
        thePlayer = FindObjectOfType<FPSController>();
        pickUpHUD.SetActive(false);
        inventorySize = slots.Count;
        UpdateInventory();
    }

    // Update is called once per frame
    void Update() {
        //if (weapons.Count > 0) WeaponSelect(weapons);
        if (Input.GetKeyDown(KeyCode.I)) {
            showInventory = !showInventory;
            selectedSlot = -1;
            WeaponSelect(weapons);
            UpdateInventory();
        }
        if (showInventory) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, inventoryLayer)) {
                SlotID id = hit.collider.GetComponent<SlotID>();
                if (id != null) {
                    activeSlot = id.slotID;
                    if (Input.GetKeyDown(KeyCode.Mouse0)) {
                        //if (replaceWeapon == false) {
                            if (weapons.Count >= activeSlot + 1) {
                                selectedSlot = activeSlot;
                                showInventory = false;
                                WeaponSelect(weapons);
                                UpdateInventory();
                            }
                        /*}
                        else {
                            weapons[selectedSlot] = Instantiate(newWeapon);
                            selectedSlot = activeSlot;
                            showInventory = false;
                            WeaponSelect(weapons);
                            replaceWeapon = false;
                            Instantiate(weaponItems[selectedSlot], transform.position, Quaternion.identity);
                            weaponItems[selectedSlot] = replaceWeaponItem;
                        }*/
                    }
                    if (Input.GetKeyDown(KeyCode.Mouse1)) {
                        if (weaponItems[activeSlot] != null) {
                            Destroy(weapons[activeSlot]);
                            weapons[activeSlot] = null;
                            Instantiate(weaponItems[activeSlot], transform.position, Quaternion.identity);
                            weaponItems[activeSlot] = null;
                            Destroy(slots[activeSlot].transform.GetChild(0).gameObject);
                            showInventory = false;
                            WeaponSelect(weapons);
                            UpdateInventory();
                        }
                    }
                }
                else {
                    activeSlot = -1;
                }
            }
            foreach (Transform slot in slots) {
                if (slot.GetComponent<SlotID>().slotID == activeSlot) {
                    slot.GetComponent<Animator>().SetBool("Highlighted", true);
                }
                else {
                    slot.GetComponent<Animator>().SetBool("Highlighted", false);
                }
            }
        }
    }

    public void UpdateInventory() {
        inventoryUI.SetActive(showInventory);
        thePlayer.freezeRotation = showInventory;
        thePlayer.freezePosition = showInventory;
        thePlayer.lockMouse = !showInventory;
        if (selectedSlot == -1) {
            GameObject.Find("Canvas").transform.Find("WeaponInfo").transform.Find("AmmoCount").GetComponent<Text>().text = "";
            GameObject.Find("Canvas").transform.Find("WeaponInfo").transform.Find("MagCount").GetComponent<Text>().text = "";
            GameObject.Find("Canvas").transform.Find("WeaponInfo").transform.Find("Name").GetComponent<Text>().text = "";
        }
    }

    public void WeaponSelect(List<GameObject> weaponType) {
        for (int i = 0; i < weaponType.Count; i++) {
            if (weaponType[i] != null) weaponType[i].SetActive(false);
        }
        if (selectedSlot != -1) {
            if (weaponType[selectedSlot] != null) weaponType[selectedSlot].SetActive(true);
        }
    }

    public void UpdateWeaponInfo() {
        weapons[selectedSlot].SetActive(false);
        weapons[selectedSlot].SetActive(true);
    }

    public void SelectInventorySlot(int slot) {
        if (slot < weapons.Count) {
            if (weapons[slot] != null) {
                selectedSlot = slot;
            }
        }
    }
}
