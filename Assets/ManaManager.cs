using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaManager : MonoBehaviour
{
    public float mana = 100;
    private Animator animator;
    public float jumpManaCost = 5f;
    public float softPunchManaCost = 10f;
    public float hardPunchManaCost = 20f;
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(RegenerateMana()); // Mana yenileme Coroutine'ini başlat
        UpdateManaAnimation();
    }

    void Update()
    {
        // Örnek: Space tuşuna basıldığında zıplama
        if (Input.GetKeyDown(KeyCode.W))
        {
            DecreaseMana(5);
        }

        // Örnek: F tuşuna basıldığında SoftPunch
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DecreaseMana(10);
        }

        // Örnek: G tuşuna basıldığında HardPunch
        if (Input.GetKeyDown(KeyCode.S))
        {
            DecreaseMana(20);
        }
    }
    public bool HasEnoughManaForJump()
    {
        return mana >= jumpManaCost;
    }
    public void DecreaseManaForJump()
    {
        mana -= jumpManaCost;
        // Diğer işlemler...
    }
    public bool HasEnoughManaForSoftPunch()
    {
        return mana >= softPunchManaCost;
    }
    public bool HasEnoughManaForHardPunch()
    {
        return mana >= hardPunchManaCost;
    }

    public void DecreaseManaForHardPunch()
    {
        mana -= hardPunchManaCost;
        // Diğer işlemler...
    }
    public void DecreaseManaForSoftPunch()
    {
        mana -= softPunchManaCost;
        // Diğer işlemler...
    }
    public void DecreaseMana(float amount)
    {
        mana -= amount;
        mana = Mathf.Clamp(mana, 0, 100); // mana'yı 0 ile 100 arasında tut
        UpdateManaAnimation();
    }

    private void UpdateManaAnimation()
    {
        // Animator parametresini güncelle
        animator.SetFloat("ManaLevel", mana);

        // Duruma göre animasyon değişikliği
        if (mana > 75)
        {
            animator.Play("Calm");
        }
        else if (mana > 50)
        {
            animator.Play("LittleFussy");
        }
        else if (mana > 25)
        {
            animator.Play("Fussy");
        }
        else
        {
            animator.Play("Panic");
        }
    }

    private IEnumerator RegenerateMana()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 1 saniye bekle
            IncreaseMana(10); // Mana'yı 5 birim artır
        }
    }

    private void IncreaseMana(float amount)
    {
        mana += amount;
        mana = Mathf.Clamp(mana, 0, 100); // mana'yı 0 ile 100 arasında tut
        UpdateManaAnimation();
    }
}
