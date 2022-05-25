using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    public float FPS;
    public List<Sprite> idle;
    public List<Sprite> walk;
    public List<Sprite> jump;

    private List<Sprite> previousClip;
    private int n;
    private SpriteRenderer sr;
    private PlayerController pc;

    // Start is called before the first frame update
    void Start() {
        sr = GetComponent<SpriteRenderer>();
        pc = transform.root.GetComponent<PlayerController>();
        InvokeRepeating("Animation", 0f, 1 / FPS);
    }

    void Animation() {
        sr.flipX = !pc.facingRight;

        if (!pc.isGrounded) {
            LoopFrames(jump);
            return;
        }
        if (pc.isMoving) {
            LoopFrames(walk);
            return;
        }
        LoopFrames(idle);
    }

    void LoopFrames(List<Sprite> frames) {
        if (frames != previousClip) n = 0;
        sr.sprite = frames[n];
        if (n < frames.Count - 1) {
            n += 1;
        }
        else {
            n = 0;
        }
        previousClip = frames;
    }
}
