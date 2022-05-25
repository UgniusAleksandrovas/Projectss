using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectingCollision : MonoBehaviour
{
    public LayerMask layerMask;
    public int damage = 20;
    public float range = 1f;

    public float sightRange = 5f;
    public float sightAngle = 90f;

    private PlayerScript Player;

    public Transform target;

    private bool isHitting = false;

    Animator _animator;

    HealthBarController healthBar;
     ArmorBarController armorBar;

    public void Awake()
    {
        _animator = GetComponent<Animator>();
        Player = FindObjectOfType<PlayerScript>();
        healthBar = Player.GetComponent<HealthBarController>();
        armorBar = Player.GetComponent<ArmorBarController>();
    }
    void Update()
    {
        Vector3 dist = (Player.transform.position + Vector3.up) - (transform.position + Vector3.up):
        if (dist.magnitude < sightRange)
        {
            float cosAngle = Vector3.Dot(dist.normalized, transform.forward.normalized);
            float angle = Mathf.Acos(cosAngle) * Mathf.Rad2Deg;
            if (angle < sightAngle / 2)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up, dist, out hit, range, layerMask))
                {
                    if (!isHitting)
                    {
                        if (Physics.Raycast(transform.position + Vector3.up, dist, out hit, range, layerMask))
                        hitAnimation();
                        _animator.SetTrigger("Attack");
                    }
                }
            }
        }
    }

    public void hitAnimation()
    {
        StartCoroutine(startHitAnimation());
    }


    public IEnumerator startHitAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<audioManager>().Play("enemyHit");
    }

    void Hit()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, forward, out hit, range, layerMask))
        {
            {
                if (armorBar.armor > 0)
                {
                    FindObjectOfType<audioManager>().Play("PlayerDamage");
                    armorBar.TakeArmor(20);
                }
                else
                {
                    FindObjectOfType<audioManager>().Play("PlayerDamage");
                    healthBar.TakeDamage(20);
                }
            }
        }
        else
        {
            Debug.Log("Not Hit");
            Debug.DrawLine(transform.position, transform.TransformDirection(Vector3.forward) * 100, Color.white);
        }
    }

    void EndHit()
    {
        isHitting = false;
    }

    void StartHit()
    {
        isHitting = true;
    }
}