using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public enum EnemyState { Charging, Waiting };
    [SerializeField] EnemyState currentState = EnemyState.Waiting;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite attackSprite;
    [SerializeField] LayerMask collisionLayers;
    [SerializeField] float sightRange;
    [SerializeField] int damage;
    [SerializeField] float chargeSpeed;
    Vector2 newVelocity;
    [SerializeField] float chargeTime = 0.5f;
    float waitTillStopCharge;
    [SerializeField] float chargeResetTime = 1f;
    float waitTillCharge;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] AudioSource soundEffect;

    int moveDir;
    CapsuleCollider2D col;
    Rigidbody2D rb;
    PlayerController player;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>();
        col = GetComponent<CapsuleCollider2D>();
        waitTillStopCharge = chargeTime;
    }

    // Update is called once per frame
    void Update() {
        waitTillCharge -= Time.deltaTime;
        waitTillCharge = Mathf.Clamp(waitTillCharge, 0, chargeResetTime);

        waitTillStopCharge -= Time.deltaTime;
        waitTillStopCharge = Mathf.Clamp(waitTillStopCharge, 0, chargeTime);
        if (waitTillStopCharge <= 0) currentState = EnemyState.Waiting;

        if (waitTillCharge <= 0) {
            if (currentState == EnemyState.Waiting) {
                Vector2 dist = player.transform.position - transform.position;
                if (dist.magnitude < sightRange) {
                    sr.flipX = dist.x > 0;
                    moveDir = dist.x < 0 ? -1 : 1;
                    waitTillCharge = chargeResetTime;
                    waitTillStopCharge = chargeTime;
                    currentState = EnemyState.Charging;
                }
            }
        }
        
        switch (currentState) {
            case EnemyState.Waiting:
                newVelocity.x = 0;
                sr.sprite = defaultSprite;
                break;

            case EnemyState.Charging:
                newVelocity.x = moveDir * chargeSpeed;
                sr.sprite = attackSprite;

                if (Physics2D.CircleCast(col.bounds.center, col.bounds.extents.y, Vector2.right * moveDir, (col.bounds.extents.x / 2) + 0.01f, collisionLayers)) {
                    player.GetComponent<Health>().Damage(damage);
                    soundEffect.Play();
                    currentState = EnemyState.Waiting;
                }
                break;
        }
        newVelocity.y = rb.velocity.y;
        rb.velocity = newVelocity;
    }
}
