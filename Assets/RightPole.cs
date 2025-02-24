using UnityEngine;

public class RightPole : MonoBehaviour
{
    public Animator poleAnimator; // RightPole objesinin Animator bileşeni

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter called"); // Çarpışma tetiklendi mi kontrol etmek için

        // Eğer çarpışan obje "Monster" tag'ine sahipse
        if (collision.gameObject.CompareTag("Monster"))
        {
            Debug.Log("Collision with Monster detected"); // Monster ile çarpışma

            // Monster objesinin Animator bileşenini al
            Animator monsterAnimator = collision.gameObject.GetComponent<Animator>();

            if (monsterAnimator != null)
            {
                Debug.Log("Monster animator found and trigger set"); // Monster animatörü bulundu ve tetikleme yapıldı
                                                                     // Monster objesinin Animator bileşeninde "PlayRightPoleTrigger" trigger'ını tetikle
                monsterAnimator.SetTrigger("PlayRightPoleTrigger");
            }

            if (poleAnimator != null)
            {
                Debug.Log("Pole animator trigger set"); // Pole animatörü tetikleme yapıldı
                                                        // RightPole objesinin Animator bileşeninde "PlayRightPoleTrigger" trigger'ını tetikle
                poleAnimator.SetTrigger("PlayRightPoleTrigger");
            }
        }
    }
}
