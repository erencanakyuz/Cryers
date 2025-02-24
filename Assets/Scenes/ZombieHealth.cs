using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ZombieHealth : MonoBehaviour
{
    public float knockbackForce = 1f;
    public float maxHealth = 100f;
    private Animator anim;
    private bool isDead = false;

    private Rigidbody2D rb;
    public float health = 100f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody component take
        health = maxHealth;
        anim = GetComponent<Animator>();
    }

    void Update()
    {

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        string deathAnimation = "ZombieDeath1";
        if (gameObject.name == "Zombie1")
        {
            Debug.Log("1det");
            deathAnimation = "ZombieDeath1";
        }
        else if (gameObject.name == "Zombie2")
        {
            Debug.Log("2det");
            deathAnimation = "ZombieDeath2";
        }
        else
        {
            // Default to Death1 if the name doesn't match any specific condition
            Debug.Log("3det");
            deathAnimation = "ZombieDeath1";
        }

        anim.Play(deathAnimation);
        rb.AddForce(Vector2.right * knockbackForce, ForceMode2D.Impulse);

        // Start coroutine after ensuring the animation has started playing
        StartCoroutine(DestroyAfterDeathAnimation(deathAnimation));
    }

    private IEnumerator DestroyAfterDeathAnimation(string animationName)
    {
        // Optional: Short delay to ensure the animation state has updated
        yield return new WaitForEndOfFrame();

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        float waitTime = 0f;

        // Manually specify the lengths of your animations or get the length dynamically
        if (animationName == "ZombieDeath1")
        {
            waitTime = stateInfo.length; // Replace with the actual length if needed
        }
        else if (animationName == "ZombieDeath2")
        {
            waitTime = stateInfo.length; // Replace with the actual length if needed
        }

        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
    public void TakeDamage(float damage)
    {
        if (isDead) return;
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }



}
