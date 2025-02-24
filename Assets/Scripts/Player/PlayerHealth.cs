using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerHealth : MonoBehaviour
{

    public static event Action OnPlayerDeath;
    public float maxHealth = 100f;
    public Image healthBarImage;
    public Sprite[] healthSprites;


    private Rigidbody2D rb;
    public float health = 100f;
    public GameObject deathScreen;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }

    void Update()
    {
        UpdateHealthUI();
        if (health <= 0)
        {
            health = 0;
            OnPlayerDeath?.Invoke();
        }
    }


    public void TakeDamage(float damage)
    {
        health -= damage;
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        // Sağlık durumuna göre uygun resmi atama
        if (health >= 75)
        {
            healthBarImage.sprite = healthSprites[0];
        }
        else if (health >= 50)
        {
            healthBarImage.sprite = healthSprites[1];
        }
        else if (health >= 25)
        {
            healthBarImage.sprite = healthSprites[2];
        }
        else
        {
            healthBarImage.sprite = healthSprites[3];
        }
    }
}
