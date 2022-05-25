using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class Jetpack : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] float thrust = 100f;

    [Header("Fuel Settings")]
    [SerializeField] float maxFuel = 100f;
    [SerializeField] private float currentFuel;
    [SerializeField] float minimumFuelThreshold = 20f;
    [SerializeField] float fuelBurnRate = 20f;
    [SerializeField] float fuelReplenishRate = 15f;
    [SerializeField] float replenishCooldown = 2f;
    private float noFuelTimer;

    [Header("Model")]
    public GameObject jetpackModel;
    public Animator anim;

    [Header("UI")]
    public Image fuelBar;
    [SerializeField] Gradient fuelBarGradient;

    [Header("VFX")]
    public VisualEffect[] flames;
    [SerializeField] float VFXSpawnAmount = 50;

    [Header("Audio")]
    public AudioSource sfx;
    [SerializeField] float maxVolume = 0.3f;

    private Rigidbody rb;
    private RatController playerScript;
    private bool isFlying = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerScript = GetComponent<RatController>();
        currentFuel = maxFuel;
    }

    private void OnEnable()
    {
        playerScript.SetJumpCount(0);
        jetpackModel.SetActive(true);
        currentFuel = maxFuel;
    }

    private void OnDisable()
    {
        playerScript.SetJumpCount(1);
        jetpackModel.SetActive(false);
        anim.SetBool("Jetpack", false);
    }

    void Update()
    {
        currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
        isFlying = Input.GetButton("Jump");

        if (isFlying)
        {
            if (currentFuel > 0)
            {
                noFuelTimer = 0;
                currentFuel -= fuelBurnRate * Time.deltaTime;

                for (int i = 0; i < flames.Length; i++)
                {
                    flames[i].SetFloat("Spawn Rate", VFXSpawnAmount);
                }

                anim.SetBool("Jetpack", true);

                sfx.volume = Mathf.Lerp(sfx.volume, maxVolume, 5 * Time.deltaTime);
            }
            else
            {
                sfx.volume = Mathf.Lerp(sfx.volume, 0, 5 * Time.deltaTime);

                for (int i = 0; i < flames.Length; i++)
                {
                    flames[i].SetFloat("Spawn Rate", 0);
                }

                anim.SetBool("Jetpack", false);
            }
        }
        else
        {
            noFuelTimer += Time.deltaTime;

            sfx.volume = Mathf.Lerp(sfx.volume, 0, 5 * Time.deltaTime);

            for (int i = 0; i < flames.Length; i++)
            {
                flames[i].SetFloat("Spawn Rate", 0);
            }

            anim.SetBool("Jetpack", false);
        }

        if (noFuelTimer >= replenishCooldown)
        {
            noFuelTimer = replenishCooldown;
            currentFuel += fuelReplenishRate * Time.deltaTime;
        }

        if (fuelBar != null)
        {
            float fuelPerc = currentFuel / maxFuel;
            fuelBar.fillAmount = fuelPerc;
            fuelBar.color = fuelBarGradient.Evaluate(fuelPerc);
        }
    }

    private void FixedUpdate()
    {
        if (isFlying)
        {
            if (currentFuel > 0)
            {
                rb.AddForce(transform.up * thrust, ForceMode.Impulse);
            }
        }
    }

    public void AddFuel(float amount)
    {
        currentFuel += amount;
        currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
    }
}
