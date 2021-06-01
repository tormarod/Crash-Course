using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameMaster : MonoBehaviour
{
    public GameObject restartPanel;
    float timer = 0;
    public TMP_Text displayTimer;

    public void Update()
    {
        timer += Time.deltaTime;
        displayTimer.text = ((int)timer).ToString();
    }
    public void GoToGameScene()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
    public void Pause()
    {
        restartPanel.SetActive(true);
        Time.timeScale = 0;
    }

}
