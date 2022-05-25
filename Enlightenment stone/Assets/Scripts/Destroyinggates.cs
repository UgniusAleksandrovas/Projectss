using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyinggates : MonoBehaviour
{ 

    [SerializeField] float cubeSize = 0.2f;
    [SerializeField] int cubesInRow = 5;

    [SerializeField] float explosionForce = 50f;
    [SerializeField] float explosionRadius = 4f;
    [SerializeField] float explosionUpward = 0.4f;
    float clonesPivotDistance;
    Vector3 clonesPivot;


    void Start()
    {
        clonesPivotDistance = cubeSize * cubesInRow / 2;

        clonesPivot = new Vector3(clonesPivotDistance, clonesPivotDistance, clonesPivotDistance);
    }
    public void OnMouseDrag()
    {
        explode();
    }
    public void explode()
    {
        FindObjectOfType<audioManager>().Play("brickWall");
        Destroy(gameObject);
        for(int x = 0; x < cubesInRow; x++)
        {
            for(int y = 0; y < cubesInRow; y++)
            {
                for(int z = 0; z < cubesInRow; z++)
                {
                    createPiece(x, y, z);
                }
            }
        }
        Vector3 explosionPos = transform.position;

        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward);
            }
        }
    }

    void createPiece(int x, int y, int z)
    {
        GameObject clone;
        clone = GameObject.CreatePrimitive(PrimitiveType.Cube);

        clone.transform.position = transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z) - clonesPivot;
        clone.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        clone.AddComponent<Rigidbody>();
        clone.GetComponent<Rigidbody>().mass = cubeSize;
        clone.transform.GetComponent<Renderer>().material = gameObject.GetComponent<Renderer>().material;
    }
}
