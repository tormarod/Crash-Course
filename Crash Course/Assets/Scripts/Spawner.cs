using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject asteroidPrefab;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    float random;
    public GameObject planet;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float interval = Time.deltaTime;
        random = Random.Range(0f, 1f);

        if (interval > random) {
            GameObject newAsteroid = Instantiate(asteroidPrefab, GetRandomPosition(), Quaternion.identity);
            newAsteroid.GetComponent<Gravity>().planet = planet;
        }
    }

    Vector2 GetRandomPosition() {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        if (randomX > 0) {
            randomX = randomX;
        } else {
            randomX = randomX;
        }
        if (randomY > 0) {
            randomY = randomY;
        } else {
            randomY = randomY;
        }
        return new Vector2(randomX, randomY);
    }

}
