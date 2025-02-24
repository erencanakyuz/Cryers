using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class Timer : MonoBehaviour
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
                StartCoroutine(EndTimerRoutine());
            }
        }
    }
    void UpdateTimerText()
    {
        timerText.text = currentTime.ToString("0.00");
    }

    IEnumerator EndTimerRoutine()
    {
        audioSource.PlayOneShot(endSound);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MarketScene");
    }
}
