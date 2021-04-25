using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMove : MonoBehaviour
{

    Vector2 targetPosition;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = GetRandomPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Vector2)transform.position != targetPosition) {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        } else {
            targetPosition = GetRandomPosition();
        }
    }

    Vector2 GetRandomPosition() {
        GameObject planet = GameObject.Find("Terran");
        Transform planetTransform = planet.transform;
        Vector2 position = planetTransform.position;
        return position;
    }
}
