using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    MonoBehaviour target;

    [SerializeField] GameObject FootSteps;
    [SerializeField] AudioSource footsteps;

    [SerializeField] float moveSpeed = 5;
    [SerializeField] float rotationSpeed = 30;
    Vector3 moveDir;
    Vector3 lookDir;

    public int damage;

    Animator _animator;
    CharacterController cc;


    public bool isGrounded = true;
    Vector3 moveVector;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        moveDir = Vector3.forward * ver + Vector3.right * hor;

        cc.Move(moveDir * moveSpeed * Time.deltaTime);
        cc.Move(moveVector * Time.deltaTime);

        if (cc.isGrounded)
        {
            moveVector.y = 0;
        }
        else
        {
            moveVector += Physics.gravity * Time.deltaTime;
        }

        if (moveDir.magnitude <= 0.1f)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity))
            {

                if (hor +- ver != 0)
                {
                    footsteps.volume = 0.7f;
                }
                else
                {
                    footsteps.volume = 0f;
                }
                Vector3 direction = hitInfo.point - transform.position;
                direction.y = 0f;
                direction.Normalize();
                lookDir = direction;
            }
        }
        else
        {
            lookDir = moveDir;
        }
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), rotationSpeed * Time.deltaTime);

        _animator.SetFloat("VelocityX", moveDir.x, 0.1f, Time.deltaTime);
        _animator.SetFloat("VelocityZ", moveDir.z, 0.1f, Time.deltaTime);

    }
}
