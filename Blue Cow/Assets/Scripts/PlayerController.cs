using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] bool canMove;
    [HideInInspector] public float moveSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    [HideInInspector] public float jumpSpeed;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] float raycastExtraHeight = 0.01f;

    [HideInInspector] public bool facingRight;
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool isGrounded;

    [SerializeField] AudioSource footsteps;
    CapsuleCollider2D col;
    Rigidbody2D rb;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject CanvasUI;
    [SerializeField] GameObject AudioManager;
    [SerializeField] GameObject FootSteps;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        GetComponent<PlayerStats>().UpdatePlayerStats();
    }

    // Update is called once per frame
    void Update() {
        if (canMove) {
            //Horizontal Movement
            float hor = Input.GetAxisRaw("Horizontal");
            if (hor > 0) facingRight = true;
            if (hor < 0) facingRight = false;
            isMoving = (hor != 0) && canMove && (rb.velocity.x != 0);

            if (Mathf.Abs(rb.velocity.x) < moveSpeed) { 
                rb.velocity += Vector2.right * hor * moveSpeed * acceleration * Time.deltaTime;
            }
            if(hor != 0)
            {
                footsteps.volume = 1;
            }
            else
            {
                footsteps.volume = 0;
            }

            //Vertical Movement
            isGrounded = CheckGrounded();

            if (isGrounded) {
                if (Input.GetKeyDown(KeyCode.Space)) {
                    rb.velocity += Vector2.up * jumpSpeed;
                }
                if (hor == 0) {
                    rb.velocity -= Vector2.right * (rb.velocity.x * deceleration) * Time.deltaTime;
                }
            }
        }
    }

    private bool CheckGrounded() {
        return Physics2D.CircleCast(col.bounds.center, col.bounds.extents.x, Vector2.down, col.bounds.extents.y + raycastExtraHeight, whatIsGround);
    }

    void Win()
    {
        winMenu.SetActive(true);
        CanvasUI.SetActive(false);
        AudioManager.SetActive(false);
        FootSteps.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "BlueCow")
        {
            Win();
            Time.timeScale = 0;
        }
    }
}
