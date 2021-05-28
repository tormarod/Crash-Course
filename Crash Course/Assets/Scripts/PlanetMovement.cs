using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetMovement : MonoBehaviour
{
    bool moveAllowed;
    Collider2D col;
    public float moveSpeed;
    public GameObject restartPanel;
    private AudioSource source;
    public GameObject explosionEffect;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        col = GetComponent<Collider2D>();
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
    }
    public void GameOver()
    {
        Invoke("Delay", 1.5f);
    }

    public void Delay()
    {
        restartPanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Asteroid")
        {
            Destroy(collision.gameObject);
            Instantiate(explosionEffect, collision.transform.position, Quaternion.identity);
            source.Play();
            GameOver();
            Debug.Log("Hit");
        }
    }
}
