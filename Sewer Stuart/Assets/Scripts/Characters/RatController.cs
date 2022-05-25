using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
public class RatController : MonoBehaviour
{
    public bool gameOver;
    [HideInInspector] public bool canMove = true;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    float moveSpeedOriginal;
    [SerializeField] [Range(1f, 100f)] float acceleration = 8f;
    public float turnSpeed = 6f;
    float turnAmount;
    [SerializeField] float jumpForce = 100f;
    [SerializeField] int jumpCount = 1;
    int jumpAmount;
    int jumpCountOriginal;
    float jumpMultiplier = 1f;
    bool isJumping;
    [SerializeField] LayerMask whatIsGround;
    [HideInInspector] public bool isGrounded;
    bool justGrounded;
    [SerializeField] float gravityMultiplier = 2f;
    float horizontalInput;
    Vector3 gravity;
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

    [Header("Audio")]
    public RatAudio audioController;

    [Header("Visual Effects")]
    [SerializeField] VisualEffect waterDrops;
    [SerializeField] VisualEffect fecesSplat;
    [SerializeField] GameObject nightVision;
    [SerializeField] VisualEffect flames;
    float burnDuration = 0f;
    [SerializeField] Image screenFrostRenderer;
    Material screenFrost;
    float screenFrostScale = 0f;
    [SerializeField] float defrostSpeed = 2f;
    [SerializeField] SkinnedMeshRenderer playerMesh;
    Material[] playerMats;
    [SerializeField] GameObject skateboard;
    [SerializeField] VisualEffect speedLines;
    [SerializeField] float speedLineEmissionRate = 50;
    [SerializeField] GameObject[] doubleJump;
    [SerializeField] GameObject[] iceSkates;
    bool onIce;
    [SerializeField] float collisionScaleY = 0.8f;
    [SerializeField] float bootsCollisionScaleY = 0.9f;

    [Header("Gameplay")]
    [SerializeField] GameObject magnetPowerUp;
    public Jetpack jetpack;
    float magnetDuration;
    public GameObject trappedUI;
    public Image trappedUIFillAmount;
    [SerializeField] float waterMaxDist = 10f;
    [SerializeField] GameObject water;
    [SerializeField] Image waterVignetteRenderer;
    Material waterVignette;
    [SerializeField] GameObject ratSplat;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] Text distanceFromWaterText;
    [SerializeField] GameObject waveImage;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        moveSpeedOriginal = moveSpeed;
        canMove = true;
        jumpCountOriginal = jumpCount;
    }

    void Start()
    {
        screenFrost = screenFrostRenderer.materialForRendering;
        screenFrostScale = 0;
        screenFrost.SetFloat("_VignetteSoftness", screenFrostScale);

        waterVignette = waterVignetteRenderer.materialForRendering;
        waterVignette.SetFloat("_VignetteSoftness", 0f);

        playerMats = playerMesh.materials;
    }

    void Update()
    {
        if (canMove)
        {
            isGrounded = CheckGrounded();

            horizontalInput = Input.GetAxisRaw("Horizontal");

            if (isGrounded)
            {
                if (!justGrounded)
                {
                    justGrounded = true;
                    jumpCount = jumpCountOriginal;
                    jumpAmount = 0;
                }
                state = PlayerState.Run;
            }
            else
            {
                state = PlayerState.Air;
                justGrounded = false;
            }
            if (Input.GetButtonDown("Jump"))
            {
                if (jumpCount > 0)
                {
                    isJumping = true;
                    //rb.velocity = Vector3.zero;
                    //rb.AddForce(transform.up * jumpForce * jumpMultiplier);

                    audioController.Jump();

                    jumpCount--;
                    jumpAmount++;

                    if (jumpAmount > 1)
                    {
                        anim.SetTrigger("JumpAgain");
                    }
                }
            }

            startRot = model.transform.localRotation;

            endRot = Quaternion.Euler(0, modelTurnAngle * horizontalInput, 0);
            model.transform.localRotation = Quaternion.Slerp(startRot, endRot, modelTurnspeed * Time.deltaTime);
        }
        //anim.SetTrigger($"{state}");
        anim.SetBool("Jumping", !isGrounded);

        //magnet
        magnetPowerUp.SetActive(magnetDuration > 0);
        if (magnetDuration > 0) magnetDuration -= Time.deltaTime;

        //Screen Effects
        //flames effect
        flames.SetFloat("Spawn Rate", burnDuration > 0f ? 30 : 0);
        burnDuration -= Time.deltaTime;
        burnDuration = Mathf.Clamp(burnDuration, 0f, 100f);

        //frost effect
        screenFrost.SetFloat("_VignetteSoftness", screenFrostScale);
        screenFrostScale = Mathf.Lerp(screenFrostScale, 0f, defrostSpeed * Time.deltaTime);

        //water effect
        float dist = transform.position.z - water.transform.position.z;
        float waterPerc = 1 - Mathf.Clamp(dist / waterMaxDist, 0, 1);
        waterDrops.SetFloat("Spawn Rate", waterPerc * 200f);

        if (!gameOver) waterVignette.SetFloat("_VignetteSoftness", waterPerc);

        if (dist <= 0 && !gameOver)
        {
            gameOver = true;
            GameOver();
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            turnAmount += horizontalInput * acceleration * Time.fixedDeltaTime;
            if (horizontalInput == 0f)
            {
                turnAmount = Mathf.Lerp(turnAmount, 0, acceleration * 0.25f * Time.fixedDeltaTime);
            }
            turnAmount = Mathf.Clamp(turnAmount, -turnSpeed, turnSpeed);

            Vector3 movePos = (transform.right * turnAmount) + (transform.forward * moveSpeed);
            rb.MovePosition(transform.position + movePos * Time.fixedDeltaTime);
            if (isJumping)
            {
                rb.AddForce(transform.up * jumpForce * jumpMultiplier, ForceMode.Impulse);
                isJumping = false;
            }
            rb.AddForce(gravity * 9.81f * gravityMultiplier, ForceMode.Acceleration);
        }
    }

    void LateUpdate()
    {
        Vector3 newRot = new Vector3(
                transform.eulerAngles.x,
                transform.eulerAngles.y,
                Vector3.SignedAngle(Vector3.up, new Vector3(0, 0, transform.position.z) - transform.position, Vector3.forward)
            );
        transform.rotation = Quaternion.Euler(newRot);
        gravity = -transform.up;
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

                if (acceleration < 20f)
                {
                    if (!onIce)
                    {
                        StopJumpBoost();
                        StopSkateboarding();

                        onIce = true;
                        anim.SetBool("IceSkate", true);
                        for (int i = 0; i < iceSkates.Length; i++)
                        {
                            iceSkates[i].SetActive(true);
                        }
                        GetComponent<CapsuleCollider>().height = bootsCollisionScaleY;
                    }
                }
                else
                {
                    if (onIce)
                    {
                        onIce = false;
                        anim.SetBool("IceSkate", false);

                        for (int i = 0; i < iceSkates.Length; i++)
                        {
                            iceSkates[i].SetActive(false);
                        }
                        GetComponent<CapsuleCollider>().height = collisionScaleY;
                    }
                }
            }
        }
        return grounded;
    }

    public void ChangeSkin(Animator animator, SkinnedMeshRenderer mesh, RatAudio audio, GameObject[] springBoots, GameObject[] iceSkateBoots)
    {
        anim = animator;
        playerMesh = mesh;
        audioController = audio;
        doubleJump[0] = springBoots[0];
        doubleJump[1] = springBoots[1];
        iceSkates[0] = iceSkateBoots[0];
        iceSkates[1] = iceSkateBoots[1];
    }

    public void Stumble()
    {
        anim.SetBool("GetUp", false);
        anim.SetTrigger("Stumble");
        audioController.Stumble();
    }

    public IEnumerator ChangeSpeed(float duration, float speedMultiplier)
    {
        float moveSpeedChange = speedMultiplier - 1;
        float tunnelSpeed = FindObjectOfType<TunnelSpawner>().moveSpeed;

        moveSpeed += moveSpeedChange * tunnelSpeed;
        turnSpeed *= speedMultiplier;
        anim.SetFloat("Movespeed", speedMultiplier);

        if (speedMultiplier > 1)
        {
            speedLines.SetFloat("Emission Rate", speedLineEmissionRate);
        }

        yield return new WaitForSeconds(duration);

        moveSpeed -= moveSpeedChange * tunnelSpeed;
        turnSpeed /= speedMultiplier;
        float originalSpeed = moveSpeed / tunnelSpeed + 1f;
        anim.SetFloat("Movespeed", originalSpeed);

        speedLines.SetFloat("Emission Rate", 0);

        anim.SetBool("GetUp", true);
    }

    public void MultiplySpeed(float speedMultiplier)
    {
        float moveSpeedChange = speedMultiplier - 1;
        float tunnelSpeed = FindObjectOfType<TunnelSpawner>().moveSpeed;

        moveSpeed += moveSpeedChange * tunnelSpeed;
        turnSpeed *= speedMultiplier;
        anim.SetFloat("Movespeed", speedMultiplier);
    }

    public void FreezePosition()
    {
        canMove = false;
        //float tunnelSpeed = FindObjectOfType<TunnelSpawner>().moveSpeed;
        //moveSpeed = -tunnelSpeed;
        rb.velocity = Vector3.zero;
        anim.SetFloat("Movespeed", 0);
    }

    public void ResetSpeed()
    {
        canMove = true;
        float tunnelSpeed = FindObjectOfType<TunnelSpawner>().moveSpeed;
        //moveSpeed = 0;
        float originalSpeed = moveSpeedOriginal / tunnelSpeed + 1f;
        anim.SetFloat("Movespeed", originalSpeed);
        //animationScript.anim.SetFloat("MoveSpeed", moveSpeed / 4.6f);
    }

    public IEnumerator ChangeJumpForce(float duration, float jumpMultiplier)
    {
        jumpForce *= jumpMultiplier;
        yield return new WaitForSeconds(duration);
        jumpForce /= jumpMultiplier;
    }

    Coroutine jumpBoost;
    private IEnumerator JumpBoost(float duration, int jumpAmount)
    {
        jumpCountOriginal = jumpAmount;
        jumpCount = jumpCountOriginal;
        for (int i = 0; i < doubleJump.Length; i++)
        {
            doubleJump[i].SetActive(true);
        }
        GetComponent<CapsuleCollider>().height = bootsCollisionScaleY;

        yield return new WaitForSeconds(duration);
        jumpCountOriginal = 1;
        jumpCount = jumpCountOriginal;
        for (int i = 0; i < doubleJump.Length; i++)
        {
            doubleJump[i].SetActive(false);
        }
        GetComponent<CapsuleCollider>().height = collisionScaleY;
    }

    public void StartJumpBoost(float duration, int jumpCount)
    {
        if (jumpBoost != null)
        {
            StopCoroutine(jumpBoost);
        }
        jumpBoost = StartCoroutine(JumpBoost(duration, jumpCount));
    }

    public void StopJumpBoost()
    {
        if (jumpBoost != null)
        {
            StopCoroutine(jumpBoost);
        }
        jumpCountOriginal = 1;
        jumpCount = jumpCountOriginal;
        for (int i = 0; i < doubleJump.Length; i++)
        {
            doubleJump[i].SetActive(false);
        }
    }

    public void SetJumpCount(int jumpAmount)
    {
        jumpCountOriginal = jumpAmount;
        jumpCount = jumpCountOriginal;
    }

    public bool GetNightVisionStatus()
    {
        return nightVision.activeSelf;
    }

    public void EnableNightVision(bool enabled)
    {
        nightVision.SetActive(enabled);
    }

    public void EnableMagnet(float duration, float pullDistance, float pullStrength)
    {
        magnetDuration = duration;
        magnetPowerUp.GetComponent<MagnetPowerUp>().StartMagnet(pullDistance, pullStrength);
    }

    public void SetFlamesCount(float amount)
    {
        flames.SetFloat("Spawn Rate", amount);
    }

    public void SetBurnDuration(float amount)
    {
        burnDuration = amount;
    }

    public void IncraeseFrostScale(float amount)
    {
        screenFrostScale += amount;
        screenFrostScale = Mathf.Clamp(screenFrostScale, 0f, 1f);
    }

    public void FecesSplat(float duration, float amount)
    {
        fecesSplat.SetFloat("Spawn Rate", amount);
        fecesSplat.SetFloat("Lifetime", duration);
        fecesSplat.Play();
    }

    Coroutine skateboarding;
    private IEnumerator Skateboard(float duration, float speedMultiplier)
    {
        skateboard.SetActive(true);
        anim.SetBool("Skateboard", true);
        yield return new WaitForSeconds(duration);
        skateboard.SetActive(false);
        anim.SetBool("Skateboard", false);
    }

    public void StartSkateboarding(float duration, float speedMultiplier)
    {
        if (skateboarding != null)
        {
            StopCoroutine(skateboarding);
        }
        StartCoroutine(ChangeSpeed(duration, speedMultiplier));
        skateboarding = StartCoroutine(Skateboard(duration, speedMultiplier));
    }

    public void StopSkateboarding()
    {
        if (skateboarding != null)
        {
            StopCoroutine(skateboarding);
        }
        skateboard.SetActive(false);
        anim.SetBool("Skateboard", false);
    }

    Coroutine ignoreObstacles;
    public IEnumerator IgnoreObstacles(float duration, Material mat)
    {
        if (mat != null)
        {
            ChangeMaterial(mat);
        }

        gameObject.layer = LayerMask.NameToLayer("Ignore Obstacle");
        yield return new WaitForSeconds(duration);
        gameObject.layer = LayerMask.NameToLayer("Default");

        ResetMaterials();
    }

    public void SetIgnoreObstacles(float duration, Material playerMat)
    {
        if (ignoreObstacles != null)
        {
            StopCoroutine(ignoreObstacles);
        }
        ignoreObstacles = StartCoroutine(IgnoreObstacles(duration, playerMat));
    }

    private void ChangeMaterial(Material mat)
    {
        Material[] materials = playerMesh.materials;
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = mat;
        }
        playerMesh.materials = materials;
    }

    private void ResetMaterials()
    {
        playerMesh.materials = playerMats;
    }

    public void GameOver()
    {
        StartCoroutine(Die());
        EventManager.SetGameOver();
    }

    public float PlayerDistanceFromWater()
    {
        float distance = Vector3.Distance(water.transform.position, transform.position);

        distanceFromWaterText.text = (distance - 4.6).ToString("f1") + "m";
        waveImage.transform.localScale = new Vector3(Mathf.Sqrt(12f / distance), Mathf.Sqrt(12f / distance), Mathf.Sqrt(12f / distance));
        return distance;
    }
    private IEnumerator Die()
    {
        FindObjectOfType<PlayerScore>().alive = false;
        water.transform.parent = transform;
        audioController.Stumble();
        canMove = false;
        gameObject.layer = LayerMask.NameToLayer("Ignore Obstacle");
        waterVignette.SetFloat("_VignetteSoftness", 0f);
        model.SetActive(false);
        ratSplat.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        gameOverScreen.SetActive(true);

        TunnelSpawner tunnelSpawner = FindObjectOfType<TunnelSpawner>();
        float time = 0;
        float duration = 2f;
        while (time < duration)
        {
            tunnelSpawner.ChangeMoveSpeed(-tunnelSpawner.moveSpeed * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
        tunnelSpawner.SetMoveSpeed(0f);
    }
}