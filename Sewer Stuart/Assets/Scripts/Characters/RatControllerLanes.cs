using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
public class RatControllerLanes : MonoBehaviour
{
    [HideInInspector] public bool canMove = true;
    [SerializeField] int tunnelLanes = 12;
    [SerializeField] float tunnelRadius = 5f;
    Vector2[] lanes;
    int currentLane = 0;

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 3f;
    float moveSpeedOriginal;
    [SerializeField] [Range(1f, 100f)] float acceleration = 8f;
    [SerializeField] float changeLaneSpeed = 6f;
    [SerializeField] float jumpForce = 100f;
    float jumpMultiplier = 1f;
    bool isJumping;
    [SerializeField] LayerMask whatIsGround;
    [HideInInspector] public bool isGrounded;
    [SerializeField] float gravityMultiplier = 2f;
    float horizontalInput;
    Vector3 gravity;

    Vector3 newPos;
    Quaternion startRot;
    Quaternion endRot;

    Rigidbody rb;


    public enum PlayerState
    {
        Idle,
        Walk,
        Run,
        Air
    }
   public PlayerState state;

    [Header("Model Animations")]
    [SerializeField] GameObject model;
    [SerializeField] float modelTurnAngle;
    [SerializeField] float modelTurnspeed;
    //[SerializeField] public RatAnimations animationScript;
    [SerializeField] Animator anim;

    [Header("Screen Effects")]
    [SerializeField] VisualEffect waterDrops;
    [SerializeField] VisualEffect fecesSplat;
    [SerializeField] GameObject nightVision;
    float nightVisionDuration = 0f;

    [Header("Gameplay")]
    [SerializeField] float waterMaxDist = 10f;
    [SerializeField] GameObject water;

    [Header("Audio")]
    public RatAudio audioController;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        moveSpeedOriginal = moveSpeed;
        canMove = true;

        float tunnelLaneAngle = 360f / tunnelLanes;
        lanes = new Vector2[tunnelLanes];
        for (int i = 0; i < tunnelLanes; i++)
        {
            lanes[i] = new Vector2(tunnelRadius * Mathf.Cos(tunnelLaneAngle * i * Mathf.Deg2Rad), tunnelRadius * Mathf.Sin(tunnelLaneAngle * i * Mathf.Deg2Rad));
        }
        UpdateLanePosition(0);
    }

    void Update()
    {
        if (canMove)
        {
            isGrounded = CheckGrounded();

            /*
            horizontalInput = Input.GetAxisRaw("Horizontal");
            UpdateLanePosition(horizontalInput);
            */
            if (Input.GetKeyDown(KeyCode.A))
            {
                UpdateLanePosition(-1);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                UpdateLanePosition(1);
            }
            transform.position = Vector3.Lerp(transform.position, newPos, changeLaneSpeed * Time.deltaTime);

            if (isGrounded)
            {
                state = PlayerState.Run;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isJumping = true;
                }
            }
            else
            {
                //state = PlayerState.Air;
            }

            startRot = model.transform.localRotation;

            endRot = Quaternion.Euler(0, modelTurnAngle * horizontalInput, 0);
            model.transform.localRotation = Quaternion.Slerp(startRot, endRot, modelTurnspeed * Time.deltaTime);
        }
        anim.SetTrigger($"{state}");

        //Screen Effects
        nightVision.SetActive(nightVisionDuration > 0f);
        nightVisionDuration -= Time.deltaTime;
        nightVisionDuration = Mathf.Clamp(nightVisionDuration, 0f, 100f);

        float dist = (transform.position.z - water.transform.position.z);
        float waterDropCount = 1 - (dist / waterMaxDist);
        waterDrops.SetFloat("Spawn Rate", waterDropCount * 200f);

        if (dist <= 0)
        {
            print("dead");
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            Vector3 movePos = transform.forward * moveSpeed;
            rb.MovePosition(transform.position + movePos * Time.fixedDeltaTime);

            if (isJumping)
            {
                rb.AddForce(transform.up * jumpForce * jumpMultiplier);
                isJumping = false;
            }

            rb.AddForce(gravity * 9.81f * gravityMultiplier, ForceMode.Acceleration);
        }
    }

    void LateUpdate()
    {
        if (canMove)
        {
            Vector3 newRot = new Vector3(
                transform.eulerAngles.x, 
                transform.eulerAngles.y, 
                Vector3.SignedAngle(Vector3.up, new Vector3(0, 0, transform.position.z) - transform.position, Vector3.forward)
            );
            transform.rotation = Quaternion.Euler(newRot);
            gravity = -transform.up;
        }
    }

    bool CheckGrounded()
    {
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        RaycastHit hit;
        bool grounded = Physics.Raycast(transform.position + transform.up * col.center.y, -transform.up, out hit, col.height / 2f + 0.05f, whatIsGround);
        if (grounded)
        {
            SurfacePhysics surfaceSettings = hit.transform.GetComponent<SurfacePhysics>();
            if (surfaceSettings != null)
            {
                acceleration = surfaceSettings.friction;
                jumpMultiplier = surfaceSettings.jumpMultiplier;
            }
        }
        return grounded;
    }

    void UpdateLanePosition(float dir)
    {
        if (dir < 0)
        {
            if (currentLane == 0)
            {
                currentLane = tunnelLanes - 1;
            }
            else
            {
                currentLane--;
            }
        }
        else if (dir > 0)
        {
            if (currentLane == tunnelLanes - 1)
            {
                currentLane = 0;
            }
            else
            {
                currentLane++;
            }
        }
        newPos = new Vector3(lanes[currentLane].x, lanes[currentLane].y, transform.position.z);
    }

    /*
    void OnTriggerEnter(Collider other)
    {
        IPickup pickup = other.GetComponent<IPickup>();
        if (pickup != null)
        {
            pickup.Pickup();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Obstacle obstacle = other.transform.GetComponent<Obstacle>();
        if (obstacle != null)
        {
            anim.SetTrigger("Stumble");
            anim.SetBool("Getup", false);
        }
    }
    */
    public IEnumerator ChangeSpeed(float duration, float speedMultiplier)
    {
        float moveSpeedChange = speedMultiplier - 1;
        float tunnelSpeed = FindObjectOfType<TunnelSpawner>().moveSpeed;

        moveSpeed += moveSpeedChange * tunnelSpeed;
        anim.SetFloat("Movespeed", speedMultiplier);

        yield return new WaitForSeconds(duration);

        moveSpeed -= moveSpeedChange * tunnelSpeed;
        float originalSpeed = moveSpeed / tunnelSpeed + 1f;
        anim.SetFloat("Movespeed", originalSpeed);
    }

    public void SetSpeed(float newMoveSpeed)
    {
        moveSpeed = newMoveSpeed;
        //animationScript.anim.SetFloat("MoveSpeed", moveSpeed / 4.6f);
    }

    public void ResetSpeed()
    {
        moveSpeed = moveSpeedOriginal;
        //animationScript.anim.SetFloat("MoveSpeed", moveSpeed / 4.6f);
    }

    public void AddNightVisiontime(float duration)
    {
        nightVisionDuration += duration;
    }

    public IEnumerator FecesSplat(float duration, float amount)
    {
        fecesSplat.SetFloat("Spawn Rate", amount);
        fecesSplat.SetFloat("Lifetime", duration);
        fecesSplat.Play();
        yield return new WaitForSeconds(duration);
        fecesSplat.SetFloat("Spawn Rate", 0);
    }

    public IEnumerator IgnoreObstacles(float duration)
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Obstacle");
        yield return new WaitForSeconds(duration);
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}