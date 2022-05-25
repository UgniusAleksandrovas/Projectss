using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalDoor : MonoBehaviour
{
    [SerializeField] private float spawnAngleRandom = 45f;

    [SerializeField] private bool Opened = false;
    [SerializeField] private float doorSpeed = 3f;
    [SerializeField] private GameObject Door;
    [SerializeField] private GameObject Particles;

    [SerializeField] private GameObject[] lights;
    [SerializeField] private Material greenLight;

    [SerializeField] AudioSource SFX;

    void Start()
    {
        Vector3 pivot = new Vector3(0, 0, transform.position.z);
        transform.RotateAround(pivot, transform.root.GetComponent<TunnelRespawner>().spawnPoint.forward, Random.Range(-spawnAngleRandom, spawnAngleRandom));
    }


    void Update()
    {
        if (Opened)
        {
            //Opens The Door + Activates The Particles.
            Door.transform.Translate(Vector3.up * doorSpeed * Time.deltaTime, Space.World);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        RatController rat = other.transform.GetComponent<RatController>();
        if (rat != null)
        {
            Opened = true;
            SFX.Play();
            rat.audioController.Successful();

            Particles.SetActive(true);

            //Switches Lights to Green
            foreach (GameObject lightObj in lights)
            {
                lightObj.GetComponent<MeshRenderer>().material = greenLight;
                lightObj.transform.Find("light").GetComponent<Light>().color = Color.green;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        RatController rat = other.transform.GetComponent<RatController>();
        if (rat != null)
        {
            Opened = true;
            SFX.Play();
            rat.audioController.Successful();

            Particles.SetActive(true);

            //Switches Lights to Green
            foreach (GameObject lightObj in lights)
            {
                lightObj.GetComponent<MeshRenderer>().material = greenLight;
                lightObj.transform.Find("light").GetComponent<Light>().color = Color.green;
            }
        }
    }
}
