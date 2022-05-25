using UnityEngine;
using UnityEngine.UI;

public class FPSController : MonoBehaviour {

    public bool isWalking;
    public bool isCrouching;
    public bool isMoving;
    public bool isJumping;
    public bool isSliding;
    public float crouchSpeed;
    public float walkSpeed;
    public float runSpeed;
    public float jumpSpeed;
    public float slideSpeed;
    public float slideTime;
    private float slideTimeO;
    private Vector3 slideDir;
    public Vector3 gravityMultiplier;
    public float crouchHeight;
    public float crouchRate;
    private float originalHeight;
    private float headHeight;
    private float crouchHeadHeight;
    public GameObject head;
    public GameObject hands;
    [Range (0.001f, 5f)] public float xSensitivity;
    [Range (0.001f, 5f)] public float ySensitivity;
    [Range (0.001f, 1f)] public float xAimSensitivity;
    [Range (0.001f, 1f)] public float yAimSensitivity;
    [Range (50f, 500f)] private float xSensitivityAim;
    [Range (50f, 500f)] private float ySensitivityAim;
    [Range (40f, 75f)] public float FOV;
    public float FOVOriginal;
    private float destination;
    private float FOVWeight;
    public bool lockMouse;
    public bool hideMouse;
    public bool freezePosition;
    public bool freezeRotation;
    //public GameObject pauseMenu;
    //public Slider sens;
    //public Slider aimSens;
    public bool canAim;

    private float jumpForce;
    private float xSensitivityF;
    private float ySensitivityF;
    private bool pauseTime;
    public bool movingForward;
    //private Vector3 desiredMove;
    public float moveSpeed;
    private Vector3 moveDir;
    private float rotationY;
    private float rotationX;
    //private float timer;
    public float walkingSpeed;
    public float runningSpeed;
    public float zoom;
    public float zoomSpeed;
    public ParticleSystem blood;
    private CharacterController theCharacterController;

    void Awake () {
        Transform spawnPos = GameObject.FindWithTag("PlayerSpawnPoint").transform;
        if (spawnPos != null) {
            //DontDestroyOnLoad(gameObject);
            transform.position = spawnPos.position;
        }
        walkSpeed = 2f;
        runSpeed = 4f;
        walkingSpeed = walkSpeed;
        runningSpeed = runSpeed;
    }

    // Use this for initialization
    void Start () {
        theCharacterController = GetComponent<CharacterController> ();
        slideTimeO = slideTime;
        slideSpeed -= runSpeed;
        xSensitivityAim = xSensitivity * xAimSensitivity;
        ySensitivityAim = ySensitivity * yAimSensitivity;
        xSensitivityF = xSensitivity;
        ySensitivityF = ySensitivity;
        FOVOriginal = FOV;
        originalHeight = theCharacterController.height;
        headHeight = head.transform.localPosition.y;
        crouchHeadHeight = headHeight - (originalHeight - crouchHeight);
        rotationY = transform.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update () {
        if (freezePosition == false) {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
                isMoving = true;
                if (Input.GetKeyDown(KeyCode.LeftShift)) {
                    isWalking = false;
                    slideTime = slideTimeO;
                }
            }
            else {
                isMoving = false;
                isWalking = true;
            }
            isCrouching = Input.GetKey(KeyCode.LeftControl);
            if (Input.GetKeyDown(KeyCode.LeftControl)) slideDir = transform.forward;
            if (isWalking != true && isMoving == true && movingForward == true) {
                ChangeFovBy(runSpeed * 4, 2);
                if (isCrouching == true) {
                    if (slideTime > 0) {
                        theCharacterController.Move(slideDir * slideSpeed * Time.deltaTime);
                        slideTime -= Time.deltaTime;
                        isSliding = true;
                    }
                    else {
                        isWalking = true;
                        isSliding = false;
                    }
                }
                else {
                    isSliding = false;
                }
            }
            else {
                if (Input.GetKey(KeyCode.Mouse1)) {
                    ChangeFovBy(-zoom, zoomSpeed);
                    xSensitivity = xSensitivityAim;
                    ySensitivity = ySensitivityAim;
                }
                else {
                    ChangeFovBy(0, 3);
                    xSensitivity = xSensitivityF;
                    ySensitivity = ySensitivityF;
                }
            }

            if (isCrouching == true) {
                theCharacterController.height = Mathf.Lerp(theCharacterController.height, crouchHeight, crouchSpeed * Time.deltaTime);
                head.transform.localPosition = new Vector3(0, Mathf.Lerp(head.transform.localPosition.y, crouchHeadHeight, crouchRate * Time.deltaTime), 0);
            }
            else {
                theCharacterController.height = Mathf.Lerp(theCharacterController.height, originalHeight, crouchSpeed * Time.deltaTime);
                head.transform.localPosition = new Vector3(0, Mathf.Lerp(head.transform.localPosition.y, headHeight, crouchRate * Time.deltaTime), 0);
            }

            jumpForce = jumpSpeed;
            if (isCrouching) {
                moveSpeed = crouchSpeed;
            }
            else if (isWalking) {
                moveSpeed = walkingSpeed;
            }
            else {
                moveSpeed = runningSpeed;
            }
            Move();
        }
        else {
            Camera.main.fieldOfView = FOVOriginal;
        }
        rotationX = Mathf.Clamp (rotationX, -85, 85);
        if (freezeRotation != true) {
            rotationY += Input.GetAxis("Mouse X") * xSensitivity * Time.timeScale;
            rotationX -= Input.GetAxis("Mouse Y") * ySensitivity * Time.timeScale;
            transform.eulerAngles = new Vector3 (0, rotationY, 0);
            head.transform.localEulerAngles = new Vector3 (rotationX, 0, 0);
        }
        /*
        if (Input.GetKeyDown (KeyCode.Escape)) {
            FreezeTime(3);
            LockMouse(3);
            LockPosition(3);
            LockRotation(3);
        }*/

        if (lockMouse == true) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }/*
        if (pauseTime == true) {
            Time.timeScale = 0;
        }
        else {
            Time.timeScale = 1f;
        }*/
    }

    void Move () {
        GetComponent<Rigidbody> ().isKinematic = true;
        GetComponent<Rigidbody> ().useGravity = true;
        if (Input.GetAxis("Vertical") > 0) {
            movingForward = true;
        }
        else {
            movingForward = false;
        }
        theCharacterController.Move(((transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"))).normalized * moveSpeed * Time.deltaTime);
        theCharacterController.Move (moveDir * Time.deltaTime);
        if (theCharacterController.isGrounded) {
            if (Input.GetKey (KeyCode.Space)) {
                isJumping = true;
                moveDir.y = jumpForce;
            }
            else {
                isJumping = false;
                moveDir.y = 0;
            }
        }
        else {
            moveDir += ((Physics.gravity * 2) + gravityMultiplier) * Time.deltaTime;
        }
    }

    public void ChangeFovBy (float amount, float speed) {
        destination = FOVOriginal + amount;
        if (destination > Camera.main.fieldOfView) {
            FOV += ((destination - FOVWeight) - Camera.main.fieldOfView) * speed * Time.deltaTime;
        }
        else {
            if (destination < Camera.main.fieldOfView) {
                FOV -= (Camera.main.fieldOfView - (destination - FOVWeight)) * speed * Time.deltaTime;
            }
        }
        Camera.main.fieldOfView = FOV;
    }

    public void ChangeMovementSpeed (float weight) {
        walkSpeed = walkingSpeed - weight;
        runSpeed = runningSpeed - weight * 2;
        FOVWeight = weight * 2;
    }

    public void MakeNoise(float range) {
        SoldierAI[] enemies = FindObjectsOfType<SoldierAI>();
        foreach(SoldierAI soldier in enemies) {
            if (Vector3.Distance(transform.position, soldier.transform.position) <= range) {
                soldier.combatTime = soldier.searchTime;
            }
        }
    }

    public void LockMouse (int forceLock) {
        if (forceLock == 1) {
            lockMouse = true;
        }
        if (forceLock == 2) {
            lockMouse = false;
        }
        if (forceLock == 3) {
            lockMouse = !lockMouse;
        }
    }

    public void FreezeTime (int forceFreeze) {
        if (forceFreeze == 1) {
            pauseTime = true;
        }
        if (forceFreeze == 2) {
            pauseTime = false;
        }
        if (forceFreeze == 3) {
            pauseTime = !pauseTime;
        }
    }

    public void LockRotation (int forceLock) {
        if (forceLock == 1) {
            freezeRotation = true;
        }
        if (forceLock == 2) {
            freezeRotation = false;
        }
        if (forceLock == 3) {
            freezeRotation = !freezeRotation;
        }
    }

    public void LockPosition (int forceLock) {
        if (forceLock == 1) {
            freezePosition = true;
        }
        if (forceLock == 2) {
            freezePosition = false;
        }
        if (forceLock == 3) {
            freezePosition = !freezePosition;
        }
    }
}
