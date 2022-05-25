using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSHeadBob : MonoBehaviour {
    
    public float walkBobSpeed = 0.2f;
    public float walkBobAmount = 0.05f;

    public float runBobSpeed = 0.4f;
    public float runBobAmount = 0.1f;

    public float rotationMultiplierX = 8f;
    public float rotationMultiplierZ = 5f;
    public float midpoint = 2.0f;

    public LayerMask footstepCollisionLayers;
    public AudioSource footstepSource;
    public List<AudioClip> footstepGrass;
    public List<AudioClip> footstepConcrete;
    public List<AudioClip> footstepMetal;
    public List<AudioClip> footstepWood;

    private float timer = 0.0f;
    private float bobbingSpeed = 0.2f;
    private float bobbingAmount = 0.05f;
    private FPSController thePlayer;

    // Use this for initialization
    void Start() {
        thePlayer = FindObjectOfType<FPSController>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (thePlayer.isWalking) {
            bobbingSpeed = walkBobSpeed;
            bobbingAmount = walkBobAmount;
        }
        else {
            bobbingSpeed = runBobSpeed;
            bobbingAmount = runBobAmount;
        }
        float waveslice = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0) {
            timer = 0.0f;
        }
        else if (thePlayer.isJumping != true && thePlayer.isSliding != true && thePlayer.freezePosition != true) {
            waveslice = Mathf.Sin(timer);
            timer = timer + bobbingSpeed;
            if (timer > Mathf.PI * 2) {
                timer = timer - (Mathf.PI * 2);
            }
            if (footstepSource != null) {
                if (timer >= Mathf.PI - 0.35f && timer <= Mathf.PI) {
                    RaycastHit hit;
                    if (Physics.Raycast(footstepSource.transform.position, Vector3.down, out hit, footstepCollisionLayers)) {
                        SurfaceMaterial surface = hit.collider.GetComponent<SurfaceMaterial>();
                        if (surface == null) {
                            return;
                        }
                        if (surface.thisSurface == SurfaceMaterial.Surface.grass) {
                            footstepSource.PlayOneShot(footstepGrass[Random.Range(0, footstepGrass.Count)]);
                        }
                        else if (surface.thisSurface == SurfaceMaterial.Surface.concrete) {
                            footstepSource.PlayOneShot(footstepConcrete[Random.Range(0, footstepConcrete.Count)]);
                        }
                        else if (surface.thisSurface == SurfaceMaterial.Surface.metal) {
                            footstepSource.PlayOneShot(footstepMetal[Random.Range(0, footstepMetal.Count)]);
                        }
                        else if (surface.thisSurface == SurfaceMaterial.Surface.wood) {
                            footstepSource.PlayOneShot(footstepWood[Random.Range(0, footstepWood.Count)]);
                        }
                    }
                    if (thePlayer.isWalking != true) {
                        thePlayer.MakeNoise(5f);
                    }
                    else {
                        thePlayer.MakeNoise(2f);
                    }
                }
            }
        }
        if (waveslice != 0) {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;
            transform.localPosition = new Vector3(0, midpoint + translateChange, 0);
            transform.localRotation = Quaternion.Euler(translateChange * rotationMultiplierX, 0, translateChange * rotationMultiplierZ);
        }
        else {
            transform.localPosition = new Vector3(0, midpoint, 0);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
