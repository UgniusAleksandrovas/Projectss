using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    [SerializeField]
    Transform blastSpawn;
    [SerializeField] GameObject FireBallPrefab;
    [SerializeField] float shootInterval = 0.5f;
    float waitTillShoot = 1f;

    Animator attack;
    PlayerScript ps;

     void Start()
    {
        attack = GetComponent<Animator>();
        ps = GetComponent<PlayerScript>();
    }

     void Update()
    {
        if (waitTillShoot <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                attack.SetTrigger("Attack");
                GameObject BlastObject = Instantiate(FireBallPrefab);
                BlastObject.transform.position = transform.position + transform.forward;
                BlastObject.transform.forward = transform.forward;
                BlastObject.transform.position = blastSpawn.transform.position;
                BlastObject.GetComponent<Blast>().damage = ps.damage;
                waitTillShoot = shootInterval;
            }
        }
        waitTillShoot -= Time.deltaTime;
    }
}
