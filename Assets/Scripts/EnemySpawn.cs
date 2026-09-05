using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawn : MonoBehaviour
{
    [Header("Düþman Prefab'larý (Inspector'dan Sürükleyin)")]
    public GameObject[] enemyPrefabs;


    [Header("Doðma Noktalarý")]
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;

    [Header("Level / Doðma Hýzý Ayarlarý")]
    public float baseSpawnInterval = 5.0f;
    public float minSpawnInterval = 1f;
    public float speedUpPerLevel = 0.25f;

    [Header("Sahne Ayarý")]
    public int firstLevelBuildIndex = 1;

    private float currentSpawnInterval;
    private float timer;
    private int maxSpawnCount;
    private int spawnedSoFar = 0;
    private int activeEnemyCount = 0;

    void Start()
    {
        // 1. Dizi kontrolü
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogError("Düþman Prefab dizesi boþ! Inspector üzerinden Prefab ekleyin.");
        }

        // 2. Level bazlý doðma süresi ve toplam düþman sayýsý hesabý
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int levelOffset = Mathf.Max(0, currentSceneIndex - firstLevelBuildIndex);

        maxSpawnCount = Mathf.Max(levelOffset + 6, levelOffset * 3);
        currentSpawnInterval = Mathf.Max(minSpawnInterval, baseSpawnInterval - (levelOffset * speedUpPerLevel));
    }

    void Update()
    {
        // Gerekli kontroller veya doðacak düþman kalmadýysa durdur
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;
        if (spawnedSoFar >= maxSpawnCount) return;

        // Zamanlayýcý
        timer += Time.deltaTime;

        if (timer >= currentSpawnInterval)
        {
            timer = 0f;
            spawnedSoFar++;
            activeEnemyCount++;
            SpawnRandomEnemy();
        }
    }

    void SpawnRandomEnemy()
    {
        // 1. Rastgele bir düþman prefab'ý seç
        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject selectedEnemy = enemyPrefabs[randomEnemyIndex];

        // 2. Rastgele doðma yönü seç (0: Sol, 1: Sað)
        Vector2 spawnPosition = new Vector2(Random.Range(leftSpawnPoint.position.x, rightSpawnPoint.position.x), leftSpawnPoint.position.y);

        // 3. Düþmaný oluþtur
        if (selectedEnemy != null)
        {
            
            GameObject spawnedEnemy = Instantiate(selectedEnemy, spawnPosition, Quaternion.identity);
            spawnedEnemy.SetActive(true);
            Debug.Log("Active Enemy Count: " + activeEnemyCount);

        }
    }

    public void ResetSpawner()
    {
        spawnedSoFar = 0;
        timer = 0f;
    }

    public void OnEnemyKilled()
    {
        activeEnemyCount--;
        Debug.Log("Active Enemy Count: " + activeEnemyCount);
        if (spawnedSoFar >= maxSpawnCount && activeEnemyCount <= 0)
        {
            NextLevel();
        }
    }

    void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Tüm seviyeler tamamlandý!");
            // Oyun bitti veya ana menüye dönme gibi iþlemler yapýlabilir.
            SceneManager.LoadScene("Main Menu"); // Örnek olarak ana menüye dönüyoruz
        }
    }
}
