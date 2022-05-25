using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Unity.Netcode;

namespace Multiplayer
{
    public class PlayerControllerMultiplayer : NetworkBehaviour
    {
        [HideInInspector] public bool canMove = true;

        [Header("Movement Settings")]
        [SerializeField] float moveSpeed = 3f;
        float moveSpeedOriginal;
        [SerializeField] [Range(1f, 100f)] float acceleration = 8f;
        public float turnSpeed = 6f;
        float turnAmount;
        [SerializeField] float jumpForce = 400f;
        [SerializeField] int jumpCount = 1;
        int jumpAmount;
        int jumpCountOriginal;
        float jumpMultiplier = 1f;
        Vector3 gravity;
        [SerializeField] LayerMask whatIsGround;
        [HideInInspector] public bool isGrounded;
        bool justGrounded;
        [SerializeField] float gravityMultiplier = 2f;
        float horizontalInput;
        Vector3 velocity;
        Quaternion startRot;
        Quaternion endRot;
        Rigidbody rb;

        private NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>();
        private NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>();

        public enum PlayerState
        {
            Idle,
            Walk,
            Run,
            Air
        }
        private NetworkVariable<PlayerState> networkPlayerState = new NetworkVariable<PlayerState>();
        //client caches positions
        private Vector3 oldPosition = Vector3.zero;
        private Quaternion oldRotation = Quaternion.identity;
        private PlayerState oldPlayerState = PlayerState.Idle;

        [Header("Model Animations")]
        [SerializeField] GameObject model;
        [SerializeField] float modelTurnAngle;
        [SerializeField] float modelTurnspeed;
        [SerializeField] Animator anim;

        [Header("Screen Effects")]
        [SerializeField] VisualEffect waterDrops;
        [SerializeField] VisualEffect fecesSplat;
        [SerializeField] GameObject nightVision;
        float nightVisionDuration = 0f;
        [SerializeField] VisualEffect flames;
        [SerializeField] Material screenFrost;
        float screenFrostScale = 0f;
        [SerializeField] float defrostSpeed = 2f;

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
            jumpCountOriginal = jumpCount;
        }

        void Start()
        {
            screenFrostScale = 0;
            screenFrost.SetFloat("_VignetteSoftness", screenFrostScale);
        }

        public override void OnNetworkSpawn()
        {
            if (IsClient && IsOwner)
            {
                transform.rotation = Quaternion.identity;
                UpdateClientPositionAndRotationServerRpc(transform.position, transform.rotation);
                FindObjectOfType<CameraControllerMultiplayer>().SetTarget(gameObject);
            }
        }

        void Update()
        {
            if (IsClient && IsOwner)
            {
                ClientInput();
            }

            //ClientLagCompensation();
            ClientAnimations();
        }

        void FixedUpdate()
        {
            if (IsClient && IsOwner)
            {
                ClientMovement();
            }
        }

        void LateUpdate()
        {
            if (IsClient && IsOwner)
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
        /*
        private void ClientLagCompensation()
        {
            transform.position = Vector3.Lerp(transform.position, networkPosition.Value, 4f * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation.Value, 4f * Time.deltaTime);
        }
        */
        private void ClientAnimations()
        {
            if (oldPlayerState != networkPlayerState.Value)
            {
                oldPlayerState = networkPlayerState.Value;
                anim.SetTrigger($"{networkPlayerState.Value}");
            }
        }

        private void ClientInput()
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
                    UpdatePlayerStateServerRpc(PlayerState.Run);
                }
                else
                {
                    UpdatePlayerStateServerRpc(PlayerState.Air);
                    justGrounded = false;
                }
                if (Input.GetButtonDown("Jump"))
                {
                    if (jumpCount > 0)
                    {
                        rb.velocity = Vector3.zero;
                        rb.AddForce(transform.up * jumpForce);

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

            /*
            //move player
            turnAmount += horizontalInput * acceleration * Time.fixedDeltaTime;
            if (horizontalInput == 0f)
            {
                turnAmount = Mathf.Lerp(turnAmount, 0, acceleration * 0.25f * Time.fixedDeltaTime);
            }
            turnAmount = Mathf.Clamp(turnAmount, -turnSpeed, turnSpeed);

            Vector3 movePos = (transform.right * turnAmount) + (transform.forward * moveSpeed);
            if (!isGrounded) velocity -= gravity * Physics.gravity.y * Time.deltaTime;
            transform.position += (movePos + velocity) * Time.deltaTime;

            //let server know about position and rotation client changes
            if (oldPosition != transform.position || oldRotation != transform.rotation)
            {
                oldPosition = transform.position;
                oldRotation = transform.rotation;
                UpdateClientPositionAndRotationServerRpc(transform.position, transform.rotation);
            }*/
        }

        void ClientMovement()
        {
            turnAmount += horizontalInput * acceleration * Time.fixedDeltaTime;
            if (horizontalInput == 0f)
            {
                turnAmount = Mathf.Lerp(turnAmount, 0, acceleration * 0.25f * Time.fixedDeltaTime);
            }
            turnAmount = Mathf.Clamp(turnAmount, -turnSpeed, turnSpeed);

            Vector3 movePos = (transform.right * turnAmount) + (transform.forward * moveSpeed);
            rb.MovePosition(transform.position + movePos * Time.fixedDeltaTime);

            rb.AddForce(gravity * 9.81f * gravityMultiplier, ForceMode.Acceleration);
            /*
            //let server know about position and rotation client changes
            if (oldPosition != transform.position || oldRotation != transform.rotation)
            {
                oldPosition = transform.position;
                oldRotation = transform.rotation;
                UpdateClientPositionAndRotationServerRpc(transform.position, transform.rotation);
            }*/
        }

        private bool CheckGrounded()
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

                    anim.SetBool("IceSkate", acceleration < 10f);
                }
            }
            return grounded;
        }

        [ServerRpc]
        public void UpdateClientPositionAndRotationServerRpc(Vector3 newPosition, Quaternion newRotation)
        {
            networkPosition.Value = newPosition;
            networkRotation.Value = newRotation;
        }

        [ServerRpc]
        public void UpdatePlayerStateServerRpc(PlayerState state)
        {
            networkPlayerState.Value = state;
        }
    }
}
