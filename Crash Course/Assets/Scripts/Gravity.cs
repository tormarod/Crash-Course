using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    Rigidbody2D rb;

    Vector2 lookDirection;

    float lookAngle;

    [Header ("Gravity")]
    
    // Distance where gravity works
    [Range(0.0f, 1000.0f)]
    public float maxGravDist = 150.0f;
    
    // Gravity force
    [Range(0.0f, 1000.0f)]
    public float maxGravity = 150.0f;
    
    // Your planet
    public GameObject planet;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Distance to the planet
        float dist = Vector2.Distance(planet.transform.position, transform.position);

        // Gravity
        Vector2 v = planet.transform.position - transform.position;
        rb.AddForce(v.normalized * (1.0f - dist / maxGravDist) * maxGravity);

        // Rotating to the planet
        lookDirection = planet.transform.position - transform.position;
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);  
    }
}