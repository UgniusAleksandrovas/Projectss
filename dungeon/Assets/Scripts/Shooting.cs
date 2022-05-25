using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject projectile;
    public Transform shotPoint;

    private float TimeBetweenShots;
    public float StartTimeBetweenShots;
    private void Update()
    {
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;

        if (TimeBetweenShots <= 0)
        {

            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(projectile, shotPoint.position, transform.rotation);
                TimeBetweenShots = StartTimeBetweenShots;
            }
        }
        else
        {
            TimeBetweenShots -= Time.deltaTime;
        }
    }
}
