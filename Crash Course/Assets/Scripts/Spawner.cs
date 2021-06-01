using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public GameObject powerupPrefab;
    public GameObject planet;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    float asteroidRandom;
    float powerupRandom;
    GameObject newAsteroid;
    GameObject newPowerup;
    float timer = 0f;
    float randomtime;
    int selectSapwn;

    // Update is called once per frame
    void Update()
    {
        float interval = Time.deltaTime;
        asteroidRandom = Random.Range(0f, 1f);

        if (interval > asteroidRandom) {
            newAsteroid = Instantiate(asteroidPrefab, GetRandomPositionOutsideScreen(), Quaternion.identity);
            newAsteroid.GetComponent<Gravity>().planet = planet;
        }

        timer += Time.deltaTime;
        randomtime = Random.Range(15f, 30f);
        if (timer > randomtime)
        {
            newPowerup = Instantiate(powerupPrefab, GetRandomPositionOnScreen(), Quaternion.identity);
            timer = 0;
        }
    }

    Vector2 GetRandomPositionOutsideScreen() {
        selectSapwn = Random.Range(0, 8);
        float randomX = 0;
        float randomY = 0;
        switch (selectSapwn)
        {
            case 0:
                randomX = Random.Range(minX - 2, minX - 1);
                randomY = Random.Range(maxY + 1, maxY + 2);
                break;
            case 1:
                randomX = Random.Range(minX, maxX);
                randomY = Random.Range(maxY + 1, maxY + 2);
                break;
            case 2:
                randomX = Random.Range(maxX + 1, maxX + 2);
                randomY = Random.Range(maxY + 1, maxY + 2);
                break;
            case 3:
                randomX = Random.Range(maxX + 1, maxX + 2);
                randomY = Random.Range(minY, maxY);
                break;
            case 4:
                randomX = Random.Range(maxX + 1, maxX + 2);
                randomY = Random.Range(minY - 1, minY - 2);
                break;
            case 5:
                randomX = Random.Range(minX, maxX);
                randomY = Random.Range(minY - 2, minY - 1);
                break;
            case 6:
                randomX = Random.Range(minX - 2, minX - 1);
                randomY = Random.Range(minY - 2, minY - 1);
                break;
            case 7:
                randomX = Random.Range(minX - 2, minX - 1);
                randomY = Random.Range(minY, maxY);
                break;
        }
        return new Vector2(randomX, randomY);
    }

    Vector2 GetRandomPositionOnScreen()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        return new Vector2(randomX, randomY);
    }

    public void SpawnPowerup()
    {
        newPowerup = Instantiate(powerupPrefab, GetRandomPositionOnScreen(), Quaternion.identity);
    }
}
