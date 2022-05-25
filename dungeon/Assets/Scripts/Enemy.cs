using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 1f;
    public int damage;
    public float attackRate = 1f; 
    public float attackRange;
    private float waitTillNextHit;
    
    private Vector2 movement;
    private Rigidbody2D rb;
    private Transform player;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>().transform;
    }
    private void Update()
    {
        Vector3 direction = player.position - transform.position;
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //rb.rotation = angle;
        //direction.Normalize();
        movement = direction;

        if (direction.magnitude <= attackRange) {
            if (waitTillNextHit <= 0) {
                player.GetComponent<Health>().Damage(damage);
                waitTillNextHit = 1;
            }
            waitTillNextHit -= attackRate * Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        moveCharacter(movement);
    }
    void moveCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }
}
