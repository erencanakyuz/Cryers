using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class enemyhealth : MonoBehaviour
{
    public float knockbackForce = 1f;
    public float maxHealth = 100f;
    public Image healthBarImage;
    public Sprite[] healthSprites;
    private AudioSource audioSource; // AudioSource tanımı
    public AudioClip endSound; // AudioClip tanımı
    public Sprite deathSprite;
    private Rigidbody2D rb;
    public float health = 100f;
    private Animator anim;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody component take
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        health = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        UpdateHealthUI();
        if (health <= 0)
        {

            Die();
        }
    }

    void Die()
    {
        rb.AddForce(Vector2.right * knockbackForce, ForceMode2D.Impulse);
        GetComponent<Collider2D>().enabled = false;

        GetComponent<SpriteRenderer>().sprite = deathSprite;
        anim.enabled = false;
        rb.simulated = false;
        StartCoroutine(EndTimerRoutine());

    }
    IEnumerator EndTimerRoutine()
    {
        audioSource.PlayOneShot(endSound);

        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("GoldenFrontArenaScene(Hard)");
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {


        // Can durumuna göre uygun resmi atama
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
