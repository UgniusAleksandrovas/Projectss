using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float acceleration;
    public float walkSpeed;
    public float runSpeed;
    public float strafeSpeed;
    public float jumpSpeed;

    public Transform cameraPivot;
    [Range(0.001f, 5f)] public float xSensitivity;
    [Range(0.001f, 5f)] public float ySensitivity;
    [Range(0.001f, 1f)] public float xAimSensitivity;
    [Range(0.001f, 1f)] public float yAimSensitivity;
    private float xSensitivityO;
    private float ySensitivityO;
    private float xSensitivityAim;
    private float ySensitivityAim;
    private float rotationX;
    private float rotationY;

    public bool lockMouse;
    public bool freezeRotation;

    private float moveSpeed;
    private Animator theAnim;
    private Rigidbody theRB;
    private CharacterController theCC;

    private Vector3 vel;

    void Start() {
        theAnim = GetComponent<Animator>();
        theCC = GetComponent<CharacterController>();
        theRB = GetComponent<Rigidbody>();
        xSensitivityO = xSensitivity;
        ySensitivityO = ySensitivity;
        xSensitivityAim = xSensitivity * xAimSensitivity;
        ySensitivityAim = ySensitivity * yAimSensitivity;
        lockMouse = true;
        freezeRotation = false;
    }

    private void FixedUpdate() {
        Vector3 vel = new Vector3(theRB.velocity.x, 0, theRB.velocity.z);
        theAnim.SetFloat("MoveSpeed", vel.magnitude * Input.GetAxisRaw("Vertical"));
        theAnim.SetFloat("HorizontalSpeed", vel.magnitude * Input.GetAxis("Horizontal"));
        if (Input.GetAxis("Horizontal") != 0) {
            if (vel.magnitude < strafeSpeed) {
                theRB.velocity += Input.GetAxis("Horizontal") * transform.right * acceleration * Time.deltaTime;
            }
        }
        else {
            if (vel.magnitude < moveSpeed) {
                theRB.velocity += Input.GetAxis("Vertical") * transform.forward * acceleration * Time.deltaTime;
            }
        }
        if (IsGrounded()) {
            if (Input.GetKey(KeyCode.Space)) {
                theAnim.SetBool("Jump", true);
                theRB.velocity += Vector3.up * jumpSpeed;
            }
        }
        else {
            theAnim.SetBool("Jump", false);
        }
        if (freezeRotation != true) {
            transform.eulerAngles = new Vector3(0, rotationY, 0);
        }
    }

    void Update() {
        moveSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        /*
        theAnim.SetFloat("MoveSpeed", vel.magnitude * Input.GetAxisRaw("Vertical"));
        theAnim.SetFloat("HorizontalSpeed", vel.magnitude * Input.GetAxis("Horizontal"));
        vel = ((Input.GetAxis("Vertical") * transform.forward * moveSpeed) + (Input.GetAxis("Horizontal") * transform.right * strafeSpeed)) * Time.deltaTime;
        theCC.Move(vel);*/
        rotationY += Input.GetAxis("Mouse X") * xSensitivity * Time.timeScale;
        rotationX -= Input.GetAxis("Mouse Y") * ySensitivity * Time.timeScale;
        if (freezeRotation != true) {
            //transform.eulerAngles = new Vector3(0, rotationY, 0);
            cameraPivot.localEulerAngles = new Vector3(rotationX, cameraPivot.localEulerAngles.y, cameraPivot.localEulerAngles.z);
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            freezeRotation = !freezeRotation;
            lockMouse = !lockMouse;
        }
        if (lockMouse == true) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    bool IsGrounded() {
        return Physics.Raycast(transform.position + Vector3.up, -Vector3.up, 1.1f);
    }
}
