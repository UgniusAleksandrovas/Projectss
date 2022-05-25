using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour
{

    [SerializeField]
    float speed = 8f;
    [SerializeField]
    float duration = 2f;
    private float Timer;

    public Rigidbody rb;

    public int damage = 20;
    void Start()
    {
        Timer = duration;
    }

    // Update is called once per frame
    void Update()
    {

        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        Timer -= Time.deltaTime;
        if(Timer <= 0f)
        {
            FindObjectOfType<audioManager>().Play("blastDestroy");
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            FindObjectOfType<audioManager>().Play("EnemyDamaged");
            enemy.addHealth(-damage);
        }
        FindObjectOfType<audioManager>().Play("blastDestroy");
        Destroy(gameObject);
    }
}
