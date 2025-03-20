using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    public float enemySpawnRate = 3f;
    public float difficultyIncreaseRate = 0.1f;
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;

    [Header("Player Stats")]
    public int score = 0;
    public int money = 0;

    private float nextEnemySpawn = 0f;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!isGameOver && Time.time >= nextEnemySpawn)
        {
            SpawnEnemy();
            nextEnemySpawn = Time.time + enemySpawnRate;
            enemySpawnRate = Mathf.Max(1f, enemySpawnRate - difficultyIncreaseRate);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0 || spawnPoints.Length == 0) return;

        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);

        Instantiate(enemyPrefabs[randomEnemyIndex], 
                   spawnPoints[randomSpawnIndex].position, 
                   spawnPoints[randomSpawnIndex].rotation);
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public bool TryPurchase(int cost)
    {
        if (money >= cost)
        {
            money -= cost;
            return true;
        }
        return false;
    }

    public void GameOver()
    {
        isGameOver = true;
        // Attendre quelques secondes avant d'afficher le menu Game Over
        Invoke("ShowGameOverMenu", 2f);
    }

    void ShowGameOverMenu()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    public void RestartGame()
    {
        score = 0;
        money = 0;
        isGameOver = false;
        SceneManager.LoadScene("GameScene");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
