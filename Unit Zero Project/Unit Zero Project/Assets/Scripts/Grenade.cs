using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

    public float timeToExplode;
    public int damage;
    public float range;
    public float sfxRange;
    public float explosionForce;
    public AudioClip explosionSound;
    public LayerMask collisionLayers;
    public GameObject explosion;
    public GameObject grenadeModel;

    private AudioSource m_AudioSource;

    // Start is called before the first frame update
    void Start() {
        m_AudioSource = GetComponent<AudioSource>();
        StartCoroutine(WaitTillExplode());
    }

    private void OnCollisionEnter(Collision collision) {
        MakeNoise(3f);
    }

    private IEnumerator WaitTillExplode() {
        yield return new WaitForSeconds(timeToExplode);
        GetComponent<Rigidbody>().isKinematic = true;
        grenadeModel.SetActive(false);
        explosion.transform.rotation = Quaternion.Euler(270, 0, 0);
        explosion.SetActive(true);
        m_AudioSource.PlayOneShot(explosionSound);
        MakeNoise(sfxRange);
        StartCoroutine(Explosion());
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    private IEnumerator Explosion() {
        for (int i = 0; i < 2; i++) {
            List<Rigidbody> rbs = new List<Rigidbody>();
            foreach (Rigidbody rb in FindObjectsOfType<Rigidbody>()) {
                if (Vector3.Distance(rb.position, transform.position) <= range) {
                    rbs.Add(rb);
                }
            }
            foreach (Rigidbody rb in rbs) {
                Vector3 dir = rb.position - transform.position;
                if (rb.GetComponent<SoldierAI>()) dir = (rb.transform.position + Vector3.up) - transform.position;
                RaycastHit hit;
                if (Physics.Raycast(transform.position, dir, out hit, range, collisionLayers)) {
                    if (hit.collider.GetComponent<EnemyHealth>()) {
                        hit.collider.GetComponent<EnemyHealth>().health -= damage;
                        hit.collider.GetComponent<EnemyHealth>().UpdateHealth();
                    }
                    if (hit.collider.GetComponent<GeneratorHealth>()) {
                        hit.collider.GetComponent<GeneratorHealth>().health -= damage;
                        hit.collider.GetComponent<GeneratorHealth>().UpdateHealth();
                    }
                    if (hit.collider.GetComponent<Rigidbody>()) {
                        hit.collider.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, range, 0.3f);
                    }
                }
            }
            yield return null;
        }
    }

    private void MakeNoise(float range) {
        SoldierAI[] enemies = FindObjectsOfType<SoldierAI>();
        foreach (SoldierAI soldier in enemies) {
            if (Vector3.Distance(transform.position, soldier.transform.position) <= range) {
                soldier.combatTime = soldier.searchTime;
            }
        }
    }
}
