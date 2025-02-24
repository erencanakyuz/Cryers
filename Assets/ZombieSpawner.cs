using System.Collections;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab1; // İlk zombi prefab'ı
    public GameObject zombiePrefab2; // İkinci zombi prefab'ı
    public float spawnInterval = 2f; // Spawn aralığı (2 saniye)
    public int totalZombies = 5; // Toplam zombi sayısı
    public Vector2 spawnAreaMin; // Spawn alanı minimum koordinatları
    public Vector2 spawnAreaMax; // Spawn alanı maksimum koordinatları
    public float minDistanceFromPlayer = 10f;
    private int zombiesSpawned = 0; // Spawnlanan zombi sayısı
    public Transform player;
    void Start()
    {
        StartCoroutine(SpawnZombies());
    }

    IEnumerator SpawnZombies()
    {
        while (zombiesSpawned < totalZombies)
        {
            SpawnZombie();
            zombiesSpawned++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnZombie()
    {
        Vector2 spawnPosition;
        float distanceFromPlayer;

        // Uygun bir pozisyon bulana kadar döngüye devam et
        do
        {
            // Rastgele bir pozisyon belirle
            spawnPosition = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            // Player'dan olan uzaklığı hesapla
            distanceFromPlayer = Vector2.Distance(spawnPosition, player.position);
        } while (distanceFromPlayer < minDistanceFromPlayer);

        // Rastgele bir zombi prefab'ı seç
        GameObject zombiePrefab = Random.value < 0.5f ? zombiePrefab1 : zombiePrefab2;

        // Zombi yarat
        Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
    }
}
