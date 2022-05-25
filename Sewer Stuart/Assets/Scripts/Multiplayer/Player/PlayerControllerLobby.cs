using UnityEngine;
using Unity.Netcode;

namespace Multiplayer
{
    public class PlayerControllerLobby : NetworkBehaviour
    {
        private float deltaTime = 0f;

        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private float moveSpeed = 0f;
        [SerializeField] private float walkSpeed = 1.0f;
        [SerializeField] private float runSpeed = 2.0f;
        [SerializeField] private float rotationSpeed = 3.5f;
        [SerializeField] private float jumpHeight = 1f;
        [SerializeField] [Range(0f, 2f)] private float airControl = 1f;
        private bool isGrounded;
        private bool canJump;

        [SerializeField] private Vector2 randomSpawnPos = new Vector2(-4f, 4f);
        [SerializeField] private Vector2 randomSpawnRot = new Vector2(-180f, 180f);

        private NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>();
        private NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>();

        public enum PlayerState
        {
            Idle,
            Walk,
            Run,
            Air,
            Jump,
            Land,
            Emote
        }

        private NetworkVariable<PlayerState> networkPlayerState = new NetworkVariable<PlayerState>();
        private NetworkVariable<int> networkEmoteChoice = new NetworkVariable<int>(0);
        private int oldEmoteChoice = 0;
        private bool isEmoting = false;
        [SerializeField] GameObject emoteWheel;

        //client caches positions
        private Vector3 oldPosition = Vector3.zero;
        private Quaternion oldRotation = Quaternion.identity;
        private PlayerState oldPlayerState = PlayerState.Idle;

        Vector3 moveDir = Vector3.zero;
        Vector3 velocity = Vector3.zero;
        Quaternion moveRot = Quaternion.identity;

        public Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        void Start()
        {
            deltaTime = 1f / NetworkManager.Singleton.NetworkTickSystem.TickRate;
            moveSpeed = walkSpeed;
        }

        public override void OnNetworkSpawn()
        {
            if (IsClient && IsOwner)
            {
                //transform.position = new Vector3(Random.Range(randomSpawnPos.x, randomSpawnPos.y), 0.01f, Random.Range(randomSpawnPos.x, randomSpawnPos.y));
                //transform.rotation = Quaternion.Euler(0f, Random.Range(randomSpawnRot.x, randomSpawnRot.y), 0f);
                moveRot = transform.rotation;
                UpdateClientPositionAndRotationServerRpc(transform.position, transform.rotation);
                FindObjectOfType<CameraControllerLobby>().SetTarget(gameObject);
            }
        }

        void Update()
        {
            if (IsClient && IsOwner)
            {
                ClientInput();
            }

            ClientLagCompensation();
            ClientAnimations();
        }

        private void ClientLagCompensation()
        {
            transform.position = Vector3.Lerp(transform.position, networkPosition.Value, 4f * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation.Value, rotationSpeed * Time.deltaTime);
        }

        private void ClientAnimations()
        {
            if (oldPlayerState != networkPlayerState.Value || oldEmoteChoice != networkEmoteChoice.Value)
            {
                oldPlayerState = networkPlayerState.Value;
                anim.SetTrigger($"{networkPlayerState.Value}");

                oldEmoteChoice = networkEmoteChoice.Value;
                anim.SetInteger("EmoteChoice", networkEmoteChoice.Value);
            }
        }

        private void ClientInput()
        {
            isGrounded = CheckGrounded();

            Vector3 inputDir = (Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal")).normalized;
            if (isGrounded)
            {
                moveDir = inputDir;
            }
            else
            {
                //add air control
                moveDir += inputDir * airControl;
            }
            if (moveDir != Vector3.zero)
            {
                moveRot = Quaternion.LookRotation(moveDir, Vector3.up);
            }

            //change animation states
            if (isGrounded)
            {
                if (moveDir.magnitude <= 0f)
                {
                    if (!isEmoting)
                    {
                        UpdatePlayerStateServerRpc(PlayerState.Idle);
                    }
                }
                else
                {
                    StopEmoting();
                    if (!ActiveRunningActionKey())
                    {
                        UpdatePlayerStateServerRpc(PlayerState.Walk);
                    }
                    else
                    {
                        UpdatePlayerStateServerRpc(PlayerState.Run);
                    }
                }
            }
            else
            {
                StopEmoting();
                UpdatePlayerStateServerRpc(PlayerState.Air);
            }

            canJump = isGrounded;

            if (canJump)
            {
                if (velocity.y < 0f)
                {
                    velocity.y = 0f;
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -3.0f * Physics.gravity.y);
                }
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                emoteWheel.SetActive(!emoteWheel.activeSelf);
                //UpdatePlayerStateServerRpc(PlayerState.Emote);
            }

            //progressively increase movement speed rather than instant change
            float newSpeed = moveDir != Vector3.zero ? (ActiveRunningActionKey() ? runSpeed : walkSpeed) : walkSpeed;
            moveSpeed = Mathf.Lerp(moveSpeed, newSpeed, 2f * Time.deltaTime);
            
            //move the player
            Vector3 movePos = moveDir.normalized * moveSpeed;
            if (!isGrounded) velocity.y += Physics.gravity.y * Time.deltaTime;
            transform.position += (movePos + velocity) * Time.deltaTime;

            //let server know about position and rotation client changes
            if (oldPosition != transform.position || oldRotation != moveRot)
            {
                oldPosition = transform.position;
                oldRotation = moveRot;
                UpdateClientPositionAndRotationServerRpc(transform.position, moveRot);
            }
        }

        private static bool ActiveRunningActionKey()
        {
            return Input.GetKey(KeyCode.LeftShift);
        }

        public void StartEmoting(int choice)
        {
            UpdateEmoteChoiceServerRpc(choice);
            isEmoting = true;
            UpdatePlayerStateServerRpc(PlayerState.Emote);
        }

        public void StopEmoting()
        {
            isEmoting = false;
        }

        private bool CheckGrounded()
        {
            CapsuleCollider col = GetComponent<CapsuleCollider>();
            RaycastHit hit;
            bool grounded = Physics.Raycast(transform.position + transform.up * col.center.y, -transform.up, out hit, col.height / 2f + 0.05f, whatIsGround);
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

        [ServerRpc]
        public void UpdateEmoteChoiceServerRpc(int newChoice)
        {
            networkEmoteChoice.Value = newChoice;
        }
    }
}
