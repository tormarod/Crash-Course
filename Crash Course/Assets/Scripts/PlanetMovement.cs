using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.SocialPlatforms;

public class PlanetMovement : MonoBehaviour
{
    bool moveAllowed;
    Collider2D col;
    public float moveSpeed;
    public GameObject restartPanel;
    private AudioSource source;
    public GameObject explosionEffect;
    float lives = 1;
    bool shield = false;
    public GameObject shieldEffect;
    public TMP_Text eventText;
    float sizeTimer;
    bool small = false;
    float score = 0;
    float textTimer = 0;
    float speedTimer = 0;
    bool faster = false;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        col = GetComponent<Collider2D>();
        AdManager.instance.RequestInterstitial();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            transform.position = Vector2.MoveTowards(transform.position, touchPosition, moveSpeed * Time.deltaTime);

        }
        sizeTimer += Time.deltaTime;
        if (small && sizeTimer > 10f)
        {
            transform.localScale = new Vector3(2f, 2f, 1f);
            sizeTimer = 0f;
        }
        score += Time.deltaTime;
        textTimer += Time.deltaTime;
        if (textTimer > 3f)
        {
            eventText.text = "";
        }
        speedTimer += Time.deltaTime;
        if (faster && speedTimer > 10f)
        {
            faster = false;
            moveSpeed -= 4;
        }
    }
    public void GameOver()
    {
        Social.ReportScore((int)score, "CgkI3s7JseQVEAIQAg", (bool success) => {
            // handle success or failure
        });
        Invoke("Delay", 1.5f);
    }

    public void Delay()
    {
        restartPanel.SetActive(true);
        if (Random.Range(0, 4) == 0)
        {
            AdManager.instance.ShowInterstitial();
        }
        Time.timeScale = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Asteroid")
        {
            Destroy(collision.gameObject);
            Instantiate(explosionEffect, collision.transform.position, Quaternion.identity);
            source.Play();
            if (shield)
            {
                shieldEffect.SetActive(false);
                shield = false;
            }
            else
            {
                if (lives >= 1)
                {
                    lives -= 1;
                    if (lives < 1)
                    {
                        GameOver();
                    }
                }
            }
        }
        if (collision.tag == "Powerup")
        {
            Destroy(collision.gameObject);
            int randomChoice = (int)Random.Range(0f, 4f);
            Debug.Log(randomChoice);
            switch (randomChoice)
            {
                case 0:
                    Debug.Log("Case 0");
                    sizeTimer = 0f;
                    small = true;
                    eventText.text = "Reduced Size!";
                    textTimer = 0;
                    transform.localScale = new Vector3(1.3f, 1.3f, 1f);
                    break;
                case 1:
                    Debug.Log("Case 1");
                    eventText.text = "Shields Active!";
                    textTimer = 0;
                    shieldEffect.SetActive(true);
                    shield = true;
                    break;
                case 2:
                    Debug.Log("Case 2");
                    eventText.text = "Asteroid destruction!";
                    textTimer = 0;
                    GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
                    foreach (GameObject asteroid in asteroids)
                    {
                        Destroy(asteroid);
                    }
                    break;
                case 3:
                    moveSpeed += 4;
                    speedTimer = 0;
                    eventText.text = "More speed!";
                    textTimer = 0;
                    faster = true;
                    break;
            }
        }
    }
}
