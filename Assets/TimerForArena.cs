using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class TimerForArena : MonoBehaviour
{

    [Header("Component")]
    public TextMeshProUGUI timerText;
    public AudioSource audioSource;
    public AudioClip endSound;



    [Header("Timer Settings")]
    private float currentTime;

    void Start()
    {
        currentTime = 60f;
        UpdateTimerText();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerText();

            if (currentTime <= 0)
            {
                currentTime = 0;
                UpdateTimerText();
                if (audioSource != null && endSound != null)
                {
                    // audioSource ve endSound değişkenleri null değilse bu bloğu çalıştır
                    audioSource.PlayOneShot(endSound);
                }

                SceneManager.LoadScene("GameStart");
            }
        }
    }
    void UpdateTimerText()
    {
        timerText.text = currentTime.ToString("0.00");
    }

}
