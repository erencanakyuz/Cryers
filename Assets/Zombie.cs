using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour
{
    public Transform target; // Karakterin konumunu hedef alacak
    public GameObject attackPoint;
    public LayerMask playerLayer;
    public float moveSpeed = 5f;
    public float attackInterval = 2f; // Saldırı aralığı 4 saniye
    public float attackRange = 6f;
    public float stoppingDistance = 1.0f; // Düşmanın karaktere ne kadar yaklaşacağını ayarlar
    public float damage;
    public float hardKnockBackForce = 10f;
    private Animator anim;
    private Rigidbody2D rb;
    public bool canAttack;
    private bool isAttacking = false;
    private bool isAwake = false; // Zombinin hareket etmeye başladığını kontrol eder
    private bool isMoving = false; // Zombinin hareket edip etmediğini kontrol eder
    public float attackCooldown = 2f; // Saldırı arası bekleme süresi
    private const int zombieLayer = 6; // Assuming you set the Zombie layer to the 8th slot
    private const int zombieLayerMask = 1 << zombieLayer;
    private AudioSource audioSource;
    public AudioClip SoftImpactSound;
    // Start fonksiyonu
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        Physics2D.IgnoreLayerCollision(zombieLayer, zombieLayer);
        StartCoroutine(AttackDelaySystem());
        StartCoroutine(AwakeZombieAfterDelay(3f)); // 3 saniye gecikme
    }

    // Update fonksiyonu
    void Update()
    {
        if (isAwake)
        {
            AI();
        }
    }

    private void AI()
    {
        if (target == null)
            return;

        Vector2 direction = (target.position - transform.position).normalized;
        if (Vector2.Distance(transform.position, target.position) > stoppingDistance)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            anim.SetBool("isMoving", true);

            // Yürüme yönüne göre dönme
            if (direction.x < 0) // Sağa doğru hareket ediyorsa
            {
                transform.localScale = new Vector3(1, 1, 1); // Düşmanı sağa doğru çevir
            }
            else if (direction.x > 0) // Sola doğru hareket ediyorsa
            {
                transform.localScale = new Vector3(-1, 1, 1); // Düşmanı sola doğru çevir
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("isMoving", false);
        }

        // Saldırı aralığı içindeyse ve saldırmıyorsa saldırıyı başlat
        if (canAttack && !isAttacking && Vector2.Distance(transform.position, target.position) < attackRange)
        {
            StartCoroutine(AttackRoutine());
        }
    }
    IEnumerator AttackDelaySystem()
    {
        // 2 saniye bekle
        yield return new WaitForSeconds(2);

        // Gecikmeden sonra yapılacak işlemler
        Debug.Log("2 saniye geçti!");
    }
    IEnumerator AttackRoutine()
    {
        if (!canAttack)
        {
            yield break; // Saldırı yapılamıyorsa çık
        }

        isAttacking = true;


        if (canAttack) // Sadece canAttack true ise animasyonu tetikle
        {
            anim.SetTrigger("EnemyKick");
            canAttack = false;
        }
        // 2 saniye bekle
        yield return new WaitForSeconds(attackInterval);

        // Gecikmeden sonra yapılacak işlemler



        isAttacking = false;
        canAttack = true;
    }


    private IEnumerator AwakeZombieAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAwake = true;
    }

    public void AttackDetection()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, playerLayer);

        foreach (Collider2D player in hitPlayers)
        {
            Debug.Log("Collision detected with player");

            // Hasar ver ve animasyon tetikle
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.health -= damage;
                player.GetComponent<Animator>().SetTrigger("PlayerHardImpact");
                audioSource.PlayOneShot(SoftImpactSound);
                // Player'a geri tepme kuvveti uygula
                Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
                player.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * hardKnockBackForce, ForceMode2D.Impulse);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }
}
