using UnityEngine;

public class Collectable : MonoBehaviour {

    [SerializeField] GameObject pickupEffect;

    [Header("Information UI")]
    [SerializeField] string itemName;
    [SerializeField] Sprite itemSprite;
    [SerializeField] [TextArea(1, 5)] string itemInfo;

    [HideInInspector] public Inventory i;

    [SerializeField] AudioClip pickupSFX;

    void Start() {
        i = FindObjectOfType<Inventory>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<PlayerController>()) {
            if (pickupEffect != null) {
                Instantiate(pickupEffect, transform.position, transform.rotation);
            }
            i.GetComponent<AudioSource>().PlayOneShot(pickupSFX);
            Pickup();
            Destroy(gameObject);
        }
    }

    public virtual void Pickup() {
        DisplayInformation();
        i.UpdateUI();
    }

    public void DisplayInformation() {
        i.itemTitle.text = itemName;
        i.itemSprite.sprite = itemSprite;
        i.itemInfo.text = itemInfo;
        i.itemInfoUI.SetActive(true);
        Time.timeScale = 0;
    }
}