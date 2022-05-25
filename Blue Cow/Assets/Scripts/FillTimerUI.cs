using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillTimerUI : MonoBehaviour {

    [SerializeField] bool loop;
    [SerializeField] bool destroyOnEnd;
    bool animate;
    float timer;
    float timerLength;

    Image imageFill;

    void Update() {
        if (animate) {
            if (timer > 0) {
                float fillAmount = timer / timerLength;
                imageFill.fillAmount = fillAmount;
                timer -= Time.deltaTime;
            }
            else {
                if (loop) {
                    timer = timerLength;
                }
                else {
                    if (destroyOnEnd) {
                        Destroy(transform.parent.gameObject);
                    }
                    else {
                        timer = 0;
                        animate = false;
                        imageFill.enabled = false;
                    }
                }
            }
        }
    }

    public void StartTimer(float duration) {
        imageFill = GetComponent<Image>();
        imageFill.enabled = false;

        timerLength = duration;
        timer = duration;
        animate = true;
        imageFill.enabled = true;
    }
}
