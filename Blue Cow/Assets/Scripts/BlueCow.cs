using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCow : MonoBehaviour {

    public bool canMove;
    [SerializeField] float moveSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    [SerializeField] float jumpSpeed;
    [SerializeField] float jumpIntervalTime;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] LayerMask whatIsWall;
    [SerializeField] float raycastExtraWidth = 0.01f;
    [SerializeField] float raycastExtraHeight = 0.01f;

    bool facingRight;
    bool isMoving;

    bool isGrounded;
    bool doJump;
    float jumpTimer;

    CapsuleCollider2D col;
    Rigidbody2D rb;

    [Header("Animation")]
    public float FPS;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] List<Sprite> idle;
    [SerializeField] List<Sprite> walk;
    [SerializeField] List<Sprite> jump;

    private List<Sprite> previousClip;
    private int n;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        InvokeRepeating("Animation", 0f, 1 / FPS);
    }

    // Update is called once per frame
    void Update() {
        if (canMove) {
            //Horizontal Movement
            float hor = 1;
            if (hor > 0) facingRight = true;
            if (hor < 0) facingRight = false;
            isMoving = (hor != 0) && canMove && (rb.velocity.x != 0);

            if (Mathf.Abs(rb.velocity.x) < moveSpeed) {
                rb.velocity += Vector2.right * hor * moveSpeed * acceleration * Time.deltaTime;
            }

            if (jumpTimer <= 0) {
                doJump = CheckWall(hor);
            }
            jumpTimer -= Time.deltaTime;
            jumpTimer = Mathf.Clamp(jumpTimer, 0, jumpIntervalTime);

            //Vertical Movement
            isGrounded = CheckGrounded();

            if (isGrounded) {
                if (doJump) {
                    doJump = false;
                    jumpTimer = jumpIntervalTime;
                    rb.velocity += Vector2.up * jumpSpeed;
                }
                if (hor == 0) {
                    rb.velocity -= Vector2.right * (rb.velocity.x * deceleration) * Time.deltaTime;
                }
            }
            else {
                doJump = false;
                jumpTimer = jumpIntervalTime;
            }
        }
    }

    private bool CheckGrounded() {
        return Physics2D.CircleCast(col.bounds.center, col.bounds.extents.x, Vector2.down, col.bounds.extents.y + raycastExtraHeight, whatIsGround);
    }

    private bool CheckWall(float dir) {
        return Physics2D.CircleCast(col.bounds.center, col.bounds.extents.y / 2, Vector2.right * dir, col.bounds.extents.x + raycastExtraWidth, whatIsWall);
    }

    void Animation() {
        sr.flipX = !facingRight;

        if (!isGrounded) {
            LoopFrames(jump);
            return;
        }
        if (isMoving) {
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
