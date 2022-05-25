using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Obstacle_Icicle : Obstacle
{
    [HideInInspector] public bool canMove;

    [Header("Icicle Settings")]
    [SerializeField] float startFallDistance = 12f;
    [SerializeField] Vector2 randomScale = new Vector2(0.75f, 1.25f);
    [SerializeField] float fallAcceleration = 7f;
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject model;
    [SerializeField] Collider icicleCollider;

    bool raycastCheck = true;

    Rigidbody rb;
    TunnelSpawner tunnelSpawner;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (explosion != null)
        {
            icicleCollider.enabled = true;
            model.SetActive(true);
            explosion.SetActive(false);
        }
    }

    public override void Initialization()
    {
        transform.localScale *= Random.Range(randomScale.x, randomScale.y);
        player = FindObjectOfType<RatController>();
        tunnelSpawner = FindObjectOfType<TunnelSpawner>();
    }

    void FixedUpdate()
    {
        if (player == null)
        {
            player = FindObjectOfType<RatController>();
        }
        float dist = transform.position.z - player.transform.position.z;
        if (dist < startFallDistance)
        {
            canMove = true;
            raycastCheck = true;
        }
        if (canMove)
        {
            fallAcceleration = tunnelSpawner.moveSpeed;
            rb.velocity += transform.up * fallAcceleration * Time.fixedDeltaTime;
        }
        if (raycastCheck)
        {
            RayCastCollision();
        }
    }

    public override void OnTriggerEnter(Collider other) { }

    public override void OnCollisionEnter(Collision other) { }

    public void RayCastCollision()
    {
        RaycastHit hit;
        Vector3 pos = transform.position;
        Vector3 dir = transform.up;
        float dist = 2f;

        if (Physics.Raycast(pos, dir, out hit, dist))
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            icicleCollider.enabled = false;
            model.SetActive(false);
            if (explosion != null)
            {
                explosion.SetActive(true);
            }
            StartCoroutine(Despawn());

            raycastCheck = false;
        }
    }

    public override IEnumerator Despawn()
    {
        yield return new WaitForSeconds(despawnTimer);
        Destroy(gameObject);
    }
}
