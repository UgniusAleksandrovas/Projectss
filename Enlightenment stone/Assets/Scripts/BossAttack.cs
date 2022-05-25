using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public LayerMask layerMask;
    public int damage = 50;
    public float range = 1f;

    public float sightRange = 5f;
    public float sightAngle = 90f;

    private PlayerScript Player;

    public Transform target;

    private bool isHitting = false;

    public ParticleSystem enemyattack;

    Animator _animator;

    HealthBarController healthBar;
    ArmorBarController armorBar;


    public void Start()
    {
        _animator = GetComponent<Animator>();
        Player = FindObjectOfType<PlayerScript>();
        healthBar = Player.GetComponent<HealthBarController>();
        armorBar = Player.GetComponent<ArmorBarController>();
    }
    void Update()
    {
        transform.LookAt(target);
        Vector3 dist = (Player.transform.position + Vector3.up) - (transform.position + Vector3.up);
        if (dist.magnitude < sightRange)
        {
            float cosAngle = Vector3.Dot(dist.normalized, transform.forward.normalized);
            float angle = Mathf.Acos(cosAngle) * Mathf.Rad2Deg;
            if (angle < sightAngle / 2)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up, dist, out hit, range, layerMask))
                {
                    PlayerScript layer = hit.collider.GetComponent<PlayerScript>();
                    if (layer != null)
                    {
                        {
                            if (!isHitting)
                            {
                                FindObjectOfType<audioManager>().Play("EnemyBlast");
                                enemyattack.Play();
                                _animator.SetTrigger("Attack");
                            }
                        }

                    }
                }
            }
        }
    }

    void Hit()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up, forward, out hit, range, layerMask))
        {
            PlayerScript layer = hit.collider.GetComponent<PlayerScript>();
            if (layer != null)
            {
                {
                    if (armorBar.armor > 0)
                    {
                        armorBar.TakeArmor(25);
                    }
                    else
                    {
                        healthBar.TakeDamage(35);
                    }
                }
            }
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