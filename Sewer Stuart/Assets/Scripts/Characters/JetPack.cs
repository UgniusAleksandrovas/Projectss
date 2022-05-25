using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JetPack : MonoBehaviour
{
    [SerializeField] float sidewaysSpeed;
    [SerializeField] float forwardSpeed;
    [SerializeField] float thrust = 400f;
    [SerializeField] float rotationSpeed = 4f;
    [SerializeField] float boxLength;
    [SerializeField] float boxWidth;
    [SerializeField] float boxHeight;
    [SerializeField] float maxFuel;
    [SerializeField] float fuelReplenishRate;
    [SerializeField] float fuelBurnRate;
    [SerializeField] float replenishCooldown;
    [SerializeField] Transform groundPosition;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Slider fuelBar;
    [SerializeField] Text currentFuelText;
    [SerializeField] Text fuelReplenishCountdown;

    private Rigidbody rb;
    private float horizInput;
    private float vertInput;
    private Collider[] isGrounded = new Collider[1];
    private float currentFuel;
    private float timerOfHavingNoFuel = 0f;

    private bool isFlying = false;
    private bool hasFuel = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentFuel = maxFuel;

        fuelReplenishCountdown.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        horizInput = Input.GetAxis("Horizontal");
        vertInput = Input.GetAxis("Vertical");

        isFlying = Input.GetButton("Jump");

        if (currentFuel < 0)
        {
            hasFuel = false;
            currentFuel = 0;
        }

        if (hasFuel == false)
        {
            timerOfHavingNoFuel += Time.deltaTime;
            if (timerOfHavingNoFuel >= replenishCooldown)
            {
                hasFuel = true;
                timerOfHavingNoFuel = 0;
            }
            fuelReplenishCountdown.gameObject.SetActive(true);
            fuelReplenishCountdown.text = "Fuel Replenishes In: " + (replenishCooldown - timerOfHavingNoFuel).ToString("F0");
        }
        else
        {
            fuelReplenishCountdown.gameObject.SetActive(false);
        }

        fuelBar.value = (currentFuel / maxFuel) * 100;

        currentFuelText.text = "Current Fuel: " + fuelBar.value.ToString("F0");

        ChangeFuelBarColor();
    }

    void FixedUpdate()
    {
        isGrounded[0] = null;
        Physics.OverlapBoxNonAlloc(groundPosition.position, new Vector3(boxLength / 2, boxHeight / 2, boxWidth / 2), isGrounded, groundPosition.rotation, groundLayer);

        // Gradually rotates back upright when player is leaning to one side when moving left or right
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 0.2f * Time.deltaTime);

        // Prevents forwards and sideways movement when flying
        if (isGrounded[0] && isFlying == false)
        {
            rb.velocity = new Vector3(horizInput * sidewaysSpeed, rb.velocity.y, vertInput * sidewaysSpeed);
            rb.freezeRotation = true;
        }

        else if (isFlying && hasFuel)
        {

            // Moves up when not grounded
            rb.AddForce(transform.rotation * Vector3.up * thrust);
            rb.AddForce(transform.rotation * Vector3.forward * forwardSpeed, ForceMode.Force);

            Vector3 rotation = new Vector3(0, 0, -horizInput * rotationSpeed);
            transform.Rotate(rotation);
            rb.freezeRotation = false;
            rb.constraints = RigidbodyConstraints.FreezeRotationZ;

            currentFuel -= fuelBurnRate * Time.deltaTime;
        }

        if (!isFlying && hasFuel)
        {
            ReplenishFuel();
        }
    }

    private void ReplenishFuel()
    {
        if (currentFuel < maxFuel)
        {
            currentFuel += fuelReplenishRate * Time.deltaTime;
        }
    }

    // Changes the colour of the fuel bar depending on the amount of fuel remaining
    private void ChangeFuelBarColor()
    {
        if (currentFuel >= 60)
        {
            // Green
            fuelBar.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.green;
        }

        if (currentFuel < 60 && currentFuel >= 40)
        {
            // Yellow
            fuelBar.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.yellow;
        }

        if (currentFuel < 40 && currentFuel >= 20)
        {
            // Orange
            fuelBar.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1f, 0.64f, 0f);
        }

        if (currentFuel < 20)
        {
            // Red
            fuelBar.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.red;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Fuel Pickup")
        {
            currentFuel += 30;
            Destroy(col.gameObject);
        }
    }
}