using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public bool instantKill = false;
    public float speedMultiplier = 0.1f;
    public float speedDuration = 1f;
    public float knockbackSpeed = 400f;
    public float playerImmortalityTimer = 4f;
    public Material playerImmortalityMaterial;
    public float despawnTimer = -1f;

    [Header("SFX")]
    public AudioSource hitSound;
    public Vector2 hitSoundPitchRange = new Vector2(0.9f, 1.1f);

    [Header("Camera Shake")]
    public float magnitude = 0.3f;
    public float roughness = 8f;
    public float fadeInDuration = 0.2f;
    public float fadeOutDuration = 0.6f;

    [HideInInspector] public RatController player;
    CameraShake cameraShake;

    private void Start()
    {
        Initialization();
    }

    public virtual void Initialization()
    {

    }

    public virtual void OnCollisionEnter(Collision other)
    {
        if (knockbackSpeed > 0f)
        {
            Vector3 dir = -other.transform.forward + (other.transform.up * 0.5f);
            Rigidbody rb = other.transform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.AddForce(dir * knockbackSpeed);
            }
        }

        player = other.transform.GetComponent<RatController>();
        if (player != null)
        {
            if (instantKill)
            {
                player.GameOver();
            }
            if (playerImmortalityTimer > 0f)
            {
                player.SetIgnoreObstacles(playerImmortalityTimer, playerImmortalityMaterial);
            }
            if (speedDuration > 0f)
            {
                player.StartCoroutine(player.ChangeSpeed(speedDuration, speedMultiplier));
            }
            if (hitSound != null)
            {
                hitSound.pitch = Random.Range(hitSoundPitchRange.x, hitSoundPitchRange.y);
                hitSound.Play();
            }
            if (knockbackSpeed > 0f)
            {
                player.Stumble();
            }
            cameraShake = FindObjectOfType<CameraShake>();
            cameraShake.ShakeOnce(magnitude, roughness, fadeInDuration, fadeOutDuration);
        }
        if (despawnTimer > 0f)
        {
            StartCoroutine(Despawn());
        }
    }
    
    public virtual void OnTriggerEnter(Collider other)
    {
        player = other.transform.GetComponent<RatController>();
        if (player != null)
        {
            if (instantKill)
            {
                player.GameOver();
            }
            if (playerImmortalityTimer > 0f)
            {
                player.SetIgnoreObstacles(playerImmortalityTimer, playerImmortalityMaterial);
            }
            if (speedDuration > 0f)
            {
                player.StartCoroutine(player.ChangeSpeed(speedDuration, speedMultiplier));
            }
            if (hitSound != null)
            {
                hitSound.pitch = Random.Range(hitSoundPitchRange.x, hitSoundPitchRange.y);
                hitSound.Play();
            }
            if (knockbackSpeed > 0f)
            {
                player.Stumble();
            }
            cameraShake = FindObjectOfType<CameraShake>();
            cameraShake.ShakeOnce(magnitude, roughness, fadeInDuration, fadeOutDuration);
        }
        if (despawnTimer > 0f)
        {
            StartCoroutine(Despawn());
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        player = other.transform.GetComponent<RatController>();
        if (player != null)
        {
            if (playerImmortalityTimer > 0f)
            {
                player.SetIgnoreObstacles(playerImmortalityTimer, playerImmortalityMaterial);
            }
        }
        if (despawnTimer > 0f)
        {
            StartCoroutine(Despawn());
        }
    }

    public virtual IEnumerator Despawn()
    {
        yield return new WaitForSeconds(despawnTimer);
        Destroy(gameObject);
    }
}
