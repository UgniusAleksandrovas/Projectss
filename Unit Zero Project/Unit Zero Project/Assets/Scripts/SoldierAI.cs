using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierAI : MonoBehaviour {

    private CharacterController theCharacterController;
    private Health healthScript;
    [HideInInspector] public Animator anim;

    public bool canMove;
    public bool sneak;
    public bool inCombat;
    public float searchTime;
    public float shootTime;
    public float sightRange;
    public LayerMask collisionLayers;
    public float sightAngle;
    public float walkSpeed;
    public float runSpeed;
    public float turnSpeed;
    public EnemyGun gunScript;
    public ParticleSystem blood;
    public GameObject fire;

    [HideInInspector] public float combatTime;
    [HideInInspector] public bool canShoot;
    private float shootTimeO;
    private float timeToNextAction;
    private float timeToChangeDirection;
    private float angle;
    [HideInInspector] public Vector3 gravityMultiplier = new Vector3(0, 2, 0);
    [HideInInspector] public GameObject blackHole;
    [HideInInspector] public float blackHolePull;
    private FPSController player;

    public NavMeshAgent agent;

    // Use this for initialization
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        theCharacterController = GetComponent<CharacterController>();
        healthScript = GetComponent<Health>();
        anim = GetComponent<Animator>();
        player = FindObjectOfType<FPSController>();
        shootTimeO = shootTime;
    }

    // Update is called once per frame
    void Update() {
        Vector3 distance = player.transform.position - (transform.position + (Vector3.up * 1.5f));
        if (distance.magnitude < sightRange) {
            float angle = (Mathf.Atan2(distance.x, distance.z) * Mathf.Rad2Deg) % 360;
            if (angle < 0) angle += 360;
            float angleLow = transform.eulerAngles.y - (sightAngle / 2);
            float angleUpp = transform.eulerAngles.y + (sightAngle / 2);
            if ((angle > angleLow) && (angle < angleUpp)) {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + (Vector3.up * 1.5f), distance, out hit, sightRange, collisionLayers)) {
                    FPSController hitObj = hit.collider.GetComponent<FPSController>();
                    if (hitObj == player) {
                        combatTime = searchTime;
                        sneak = false;
                        if (distance.magnitude <= gunScript.shootRange) {
                            canShoot = true;
                            shootTime -= Time.deltaTime;
                            shootTime = Mathf.Clamp(shootTime, 0, shootTimeO);
                        }
                        else {
                            shootTime = shootTimeO;
                            canShoot = false;
                        }
                    }
                    else {
                        sneak = true;
                        shootTime = shootTimeO;
                        canShoot = false;
                    }
                }
            }
            else {
                shootTime = shootTimeO;
                canShoot = false;
            }
        }
        inCombat = combatTime > 0;
        combatTime -= Time.deltaTime;
        combatTime = Mathf.Clamp(combatTime, 0, searchTime);
        if (canMove) {
            if (inCombat == true) {
                if (canShoot) {
                    GunFire();
                }
                else {
                    if (sneak) {
                        Sneak();
                    }
                    else {
                        Run();
                    }
                }
            }
            else {
                DontMove();
            }
        }
    }

    void DontMove() {
        agent.SetDestination(transform.position);
        agent.speed = walkSpeed;
        anim.SetBool("running", false);
        anim.SetBool("walking", false);
    }

    void Sneak() {
        agent.SetDestination(player.transform.position);
        agent.speed = walkSpeed;
        anim.SetBool("running", false);
        anim.SetBool("walking", true);
    }

    void Run() {
        agent.SetDestination(player.transform.position);
        agent.speed = runSpeed;
        anim.SetBool("running", true);
        anim.SetBool("walking", false);
    }

    void GunFire() {
        agent.SetDestination(transform.position);
        agent.speed = walkSpeed;
        anim.SetBool("running", false);
        anim.SetBool("walking", false);
        Quaternion targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f));
        if (shootTime <= 0) {
            if (gunScript.shootType == EnemyGun.WeaponType.SemiAuto) {
                gunScript.Shoot(player.transform.position - gunScript.transform.position);
            }
            else if (gunScript.shootType == EnemyGun.WeaponType.Shotgun) {
                gunScript.Shotgun(player.transform.position - gunScript.transform.position);
            }
        }
    }

    public void MakeNoise(float range) {
        SoldierAI[] enemies = FindObjectsOfType<SoldierAI>();
        foreach (SoldierAI soldier in enemies) {
            if (Vector3.Distance(transform.position, soldier.transform.position) <= range) {
                soldier.combatTime = soldier.searchTime;
            }
        }
    }
}
