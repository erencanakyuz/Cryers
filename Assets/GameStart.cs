using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    private AudioSource audioSource;
    private bool isPaused = false;

    public GameObject gameOverMenu; // Oyun duraklatma menüsü
    public GameObject canvasPause;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Oyunu başlatırken zamanı sıfırla
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Oyunu duraklat

        gameOverMenu.SetActive(true); // Oyun duraklatma menüsünü etkinleştir
        canvasPause.SetActive(true);
    }


    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Oyunu devam ettir

        gameOverMenu.SetActive(false); // Oyun duraklatma menüsünü devre dışı bırak
        canvasPause.SetActive(false);
    }


}
