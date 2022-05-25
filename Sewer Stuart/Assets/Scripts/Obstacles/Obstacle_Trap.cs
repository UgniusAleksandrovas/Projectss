using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Obstacle_Trap : Obstacle
{
    [Header("Trap Settings")]
    [SerializeField] float trapDurationMax = 100f;
    [SerializeField] float trapBreakAmount = 20f;
    [SerializeField] float trapDecayRate = 15f;
    float trapDuration;

    [SerializeField] bool randomRotation = true;
    [SerializeField] Animator anim;
    [SerializeField] Transform trapPosition;
    [SerializeField] float moveSpeed = 0f;
    bool trapped;

    [SerializeField] GameObject UI;
    [SerializeField] Image UIFillAmount;

    CameraShake cam;

    public override void Initialization()
    {
        UI = FindObjectOfType<RatController>().trappedUI;
        UIFillAmount = FindObjectOfType<RatController>().trappedUIFillAmount;
        UI.SetActive(false);
        if (anim != null)
        {
            anim.enabled = false;
        }
        trapDuration = trapDurationMax;
    }

    void Update()
    {
        if (player != null)
        {
            if (player.gameOver)
            {
                return;
            }
        }
        if (trapped)
        {
            trapDuration = Mathf.Clamp(trapDuration, 0, trapDurationMax);
            trapDuration += trapDecayRate * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                trapDuration -= trapBreakAmount;
                cam.ShakeOnce(magnitude, roughness, fadeInDuration, fadeOutDuration);
            }
            if (trapDuration <= 0)
            {
                trapped = false;
                player.ResetSpeed();
                player.transform.parent = null;
                //FindObjectOfType<CameraController>().canRotate = true;
                UI.SetActive(false);
                GetComponent<Collider>().enabled = false;

                player.audioController.Successful();

                if (playerImmortalityTimer > 0f)
                {
                    player.SetIgnoreObstacles(playerImmortalityTimer, playerImmortalityMaterial);
                }
            }

            float perc = 1 - trapDuration / trapDurationMax;
            UIFillAmount.fillAmount = perc;
        }
    }

    public override void OnCollisionEnter(Collision other) { }

    public override void OnTriggerEnter(Collider other)
    {
        player = other.transform.GetComponent<RatController>();
        if (player != null)
        {
            if (hitSound != null)
            {
                hitSound.pitch = Random.Range(hitSoundPitchRange.x, hitSoundPitchRange.y);
                hitSound.Play();
            }

            cam = FindObjectOfType<CameraShake>();
            cam.ShakeOnce(magnitude, roughness, fadeInDuration, fadeOutDuration);

            player.FreezePosition();
            player.transform.position = trapPosition.position;
            //player.transform.rotation = trapPosition.rotation;
            player.transform.parent = transform;
            //FindObjectOfType<CameraController>().canRotate = false;
            trapped = true;
            UI.SetActive(true);
            if (anim != null)
            {
                anim.enabled = true;
            }
        }
    }

    public override void OnTriggerExit(Collider other) { }
}
