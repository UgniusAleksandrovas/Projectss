using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour {

    [SerializeField] Transform healthBar;
    [SerializeField] GameObject heartPrefab;
    [SerializeField] Sprite heart;
    [SerializeField] Sprite heartHalf;
    [SerializeField] Sprite immortalHeart;
    [SerializeField] Sprite immortalHeartHalf;
    List<GameObject> hearts = new List<GameObject>();


    Health healthScript;

    // Start is called before the first frame update
    void Awake() {
        healthScript = GetComponent<Health>();
    }

    public void UpdateUI() {
        hearts.Clear();
        foreach (Transform child in healthBar) {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < (healthScript.health + 1) / 2; i++) {
            GameObject heartObj = Instantiate(heartPrefab);
            heartObj.transform.SetParent(healthBar);
            heartObj.transform.localScale = Vector3.one;
            hearts.Add(heartObj);
            if (healthScript.immortal) {
                hearts[i].GetComponent<Image>().sprite = immortalHeart;
            }
            else {
                hearts[i].GetComponent<Image>().sprite = heart;
            }
        }
        if (healthScript.health % 2 != 0) {
            if (healthScript.immortal) {
                hearts[hearts.Count - 1].GetComponent<Image>().sprite = immortalHeartHalf;
            }
            else {
                hearts[hearts.Count - 1].GetComponent<Image>().sprite = heartHalf;
            }
        }
    }
}
