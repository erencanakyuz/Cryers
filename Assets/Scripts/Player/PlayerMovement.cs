using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D[] allColliders;
    public ManaManager manaManager;
    private float move;
    public float speed;
    public bool isAttacking = false;
    public float knockbackForce = 30f;
    public GameObject attackPoint;
    public float radious;
    public LayerMask enemyLayer;
    public float damage;
    private bool isGameOver = false;
    private AudioSource audioSource;
    public float jumpForce;
    private bool isGrounded = true;
    public AudioClip softPunchSound;
    public AudioClip hardPunchSound;
    public AudioClip walkingSound;
    public AudioClip jumpSound;
    public AudioClip landSound;
    private bool isWalkingSoundPlaying = false;
    private void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += DisablePlayerMovement;
    }
    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= DisablePlayerMovement;
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        allColliders = GetComponentsInChildren<Collider2D>();
        audioSource = GetComponent<AudioSource>();


    }

    private void FixedUpdate()
    {

        if (!isGameOver)
        {
            HorizontalMovement();
        }

    }

    private void Update()
    {
        if (!isGameOver)
        {
            Attack();
            Jump();

        }

    }

    private void HorizontalMovement()
    {
        move = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * move * speed * Time.deltaTime);

        if (Mathf.Abs(move) > 0.1f)
        {
            if (move > 0.1f)
            {
                anim.SetInteger("isWalking", 2); // Walking Right
                transform.localScale = new Vector3(1, 1, 1); // Karakteri sağa doğru çevir
            }
            else
            {
                anim.SetInteger("isWalking", 1); // Walking Left
                transform.localScale = new Vector3(-1, 1, 1); // Karakteri sola doğru çevir
            }
            // Yürüme sırasında ses çal
            if (!isWalkingSoundPlaying)
            {
                audioSource.clip = walkingSound;
                audioSource.loop = true;
                audioSource.Play();
                isWalkingSoundPlaying = true;
            }
        }
        else
        {
            anim.SetInteger("isWalking", 0); // Idle
            if (isWalkingSoundPlaying)
            {
                audioSource.Stop();
                isWalkingSoundPlaying = false;
            }
        }
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking && manaManager.HasEnoughManaForSoftPunch())
        {
            manaManager.DecreaseManaForSoftPunch();
            anim.SetBool("PlayerSoftPunch", true);
            isAttacking = true;
            if (softPunchSound != null)
            {
                audioSource.PlayOneShot(softPunchSound);
            }

        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {

            Invoke("ResetSoftPunch", 0.4f);
            isAttacking = false;
        }
        else if (Input.GetKeyDown(KeyCode.S) && !isAttacking && manaManager.HasEnoughManaForHardPunch())
        {
            manaManager.DecreaseManaForHardPunch();
            anim.SetBool("PlayerHardPunch", true);
            isAttacking = true;

            if ((anim.GetInteger("isWalking") == 0))
                Invoke("PlaySoftPunchSound", 0.4f);

        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            anim.SetBool("PlayerHardPunch", false);
            isAttacking = false;
        }
    }
    private void PlaySoftPunchSound()
    {
        // Play soft punch sound
        if (hardPunchSound != null)
        {
            audioSource.PlayOneShot(hardPunchSound);
        }
    }
    private void ResetSoftPunch()
    {
        anim.SetBool("PlayerSoftPunch", false);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded && manaManager.HasEnoughManaForJump())
        {
            manaManager.DecreaseManaForJump();
            audioSource.PlayOneShot(jumpSound);
            //AudioManager.instance.Play("ses-player-jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            anim.SetBool("isJumping", true);
            isGrounded = false;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("isJumping", false);
            isGrounded = true;
            audioSource.PlayOneShot(landSound);

        }
    }

    private void AttackDetection()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.transform.position, radious, enemyLayer);
        foreach (Collider2D enemyCollider in enemies)
        {
            ZombieHealth ZombieHealth = enemyCollider.GetComponent<ZombieHealth>();
            enemyhealth enemyHealth = null;
            if (ZombieHealth == null)
            {
                enemyHealth = enemyCollider.GetComponent<enemyhealth>();
            }
            if (ZombieHealth == null && enemyHealth == null)
            {
                Debug.Log("Health component not found on enemy!");
                continue; // Bu düşmanı atla ve sonraki düşmana geç
            }
            if (ZombieHealth != null)
            {
                ZombieHealth.TakeDamage(damage);

            }
            else if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);

            }
            // Get enemy's rigidbody for applying knockback force
            Rigidbody2D enemyRigidbody = enemyCollider.GetComponent<Rigidbody2D>();

            // Check if enemy has a Rigidbody2D component (essential for knockback)
            if (enemyRigidbody == null)
            {
                Debug.Log("Enemy does not have a Rigidbody2D component! Knockback effect won't work.");
                continue; // Skip this enemy if no Rigidbody2D
            }

            // Calculate knockback direction (away from player)
            Vector2 knockbackDirection = (enemyCollider.transform.position - transform.position).normalized;

            // Calculate knockback force based on knockbackDirection and knockbackForce
            Vector2 knockbackForceVector = knockbackDirection * knockbackForce;

            // Apply knockback force to the enemy's position
            enemyRigidbody.MovePosition(enemyRigidbody.position + knockbackForceVector * Time.fixedDeltaTime);

            // Trigger impact animation on the enemy
            Animator enemyAnimator = enemyCollider.GetComponent<Animator>();
            if (enemyAnimator != null)
            {
                if (anim.GetBool("PlayerSoftPunch"))
                {
                    enemyAnimator.SetTrigger("EnemySoftImpact");

                }
                else
                {
                    enemyAnimator.SetTrigger("EnemyHardImpact");

                }
            }
        }
    }

    private void DisablePlayerMovement()
    {
        isGameOver = true;
        foreach (Collider2D collider in allColliders)
        {
            collider.enabled = false;
        }
        anim.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        //gameObject.SetActive(false);
    }
    private void EnablePlayerMovement()
    {
        isGameOver = false;
        foreach (Collider2D collider in allColliders)
        {
            collider.enabled = true;
        }
        anim.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        //gameObject.SetActive(true);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.transform.position, radious);
    }
}
