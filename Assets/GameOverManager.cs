using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip gameOverSound;
    public GameObject gameOverMenu;

    private bool isPaused = false;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += EnableGameOverMenu;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= EnableGameOverMenu;
    }

    public void EnableGameOverMenu()
    {
        gameOverMenu.SetActive(true);
        if (gameOverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(gameOverSound);
        }
        // Oyunu duraklat
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        isPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
        }
    }

    public void Retry()
    {
        // Zamanı yeniden başlat
        Time.timeScale = 1f;

        // Mevcut sahneyi yeniden yükle
        SceneManager.LoadScene("Arena");
    }

    public void GoToMarket()
    {
        // Market sahnesine geçiş yap
        Time.timeScale = 1f;
        SceneManager.LoadScene("MarketScene");
    }

    public void ExitGame()
    {
        // Oyunu kapat
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameStart");
    }

    public void ResumeGame()
    {
        gameOverMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Scene1()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GoldenFrontArenaScene(Basic)");
    }
    public void NextScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Arena");
    }
}
