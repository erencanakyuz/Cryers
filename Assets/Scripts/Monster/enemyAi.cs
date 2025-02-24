using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform target; // Karakterin konumunu hedef alacak
    public GameObject attackPoint;
    public LayerMask playerLayer;
    public float softKnockBackForce = 30f;
    public float hardKnockBackForce = 10f;
    public float moveSpeed = 5f;
    public float attackInterval = 3f;
    public float attackRange = 1.5f;
    public float stoppingDistance = 10f; // Düşmanın karaktere ne kadar yaklaşacağını ayarlar
    public float damage;
    public GameObject exclamationObject; // Exclamation objesine referans
    private Animator exclamationAnimator; // Exclamation objesinin Animator bileşeni
    private bool isExclamationPlaying = false; // Exclamation animasyonunun oynatılıp oynatılmadığını kontrol eder

    public bool playerIsAttacking;
    private Animator anim;
    private Rigidbody2D rb;
    public bool canAttack = true;
    private bool isAttacking = false;
    public float hardPunchInterval = 4f;
    public float softPunchInterval = 2f;
    public PlayerMovement playerMovement;

    private AudioSource audioSource;
    public AudioClip SoftImpactSound;
    public AudioClip HardImpactSound;
    public enum EnemyAttackType
    {
        HardPunch,
        SoftPunch
    }

    public EnemyAttackType attackType;
    public float hardPunchDamage;
    public float softPunchDamage;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        exclamationAnimator = exclamationObject.GetComponent<Animator>();



        StartCoroutine(AttackRoutine()); // baştaki yumruğun sebebi olabilir
    }

    private void FixedUpdate()
    {
        AI();

    }

    private void AI()
    {
        if (target == null)
            return;

        // Hedefe doğru yürüme
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
        if (!isAttacking && Vector2.Distance(transform.position, target.position) < attackRange)
        {
            StartCoroutine(AttackRoutine());
        }
    }


    IEnumerator AttackRoutine()
    {
        if (!canAttack)
            yield break; // Saldırı yapılamıyorsa çık
        isAttacking = true;

        // Rastgele bir saldırı türü seçin
        attackType = (Random.Range(0, 2) == 0) ? EnemyAttackType.HardPunch : EnemyAttackType.SoftPunch;

        // Saldırı türüne göre animasyonu tetikleyin
        if (attackType == EnemyAttackType.HardPunch)
        {
            if (exclamationObject != null && exclamationAnimator != null && !isExclamationPlaying)
            {
                // Exclamation animasyonunu başlatmadan önce bir süre bekleyin
                yield return new WaitForSeconds(0.5f); // Hard Punch animasyonundan önce oynatmak istediğiniz süreyi belirtin

                // Exclamation animasyonunu oynat
                exclamationAnimator.SetTrigger("ExclamationTrigger");
                isExclamationPlaying = true;
            }
            audioSource.Play();
            anim.SetTrigger("HardPunch");
            isExclamationPlaying = false;
            yield return new WaitForSeconds(hardPunchInterval);
        }
        else if (attackType == EnemyAttackType.SoftPunch)
        {
            anim.SetTrigger("SoftPunch");
            yield return new WaitForSeconds(softPunchInterval);
        }

        // Saldırı animasyonunun tamamlanmasını bekle
        yield return new WaitForSeconds(attackInterval);

        isAttacking = false;

        // Bir sonraki saldırıyı başlatın
        StartCoroutine(AttackRoutine());
    }


    public void AttackDetection()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, playerLayer);

        foreach (Collider2D player in hitPlayers)
        {
            // Apply damage and trigger player animations based on attack type
            if (attackType == EnemyAttackType.HardPunch)
            {

                player.GetComponent<PlayerHealth>().health -= hardPunchDamage;
                player.GetComponent<Animator>().SetTrigger("PlayerHardImpact");
                audioSource.PlayOneShot(HardImpactSound);
                Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
                player.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * hardKnockBackForce, ForceMode2D.Impulse);
            }
            else if (attackType == EnemyAttackType.SoftPunch)
            {
                player.GetComponent<PlayerHealth>().health -= softPunchDamage;
                player.GetComponent<Animator>().SetTrigger("PlayerSoftImpact");
                audioSource.PlayOneShot(SoftImpactSound);
                Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
                player.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * softKnockBackForce, ForceMode2D.Impulse);
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
