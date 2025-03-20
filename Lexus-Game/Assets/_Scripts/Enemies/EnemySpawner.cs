using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    public int waveCount = 5;
    public int enemiesPerWave = 8;
    public float spawnInterval = 2f;

    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public float spawnRadius = 20f;
    
    private Transform player;
    public int currentWave { get; private set; }
    private int enemiesRemaining;
    private bool isSpawning = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartNextWave();
    }

    private void StartNextWave()
    {
        if (currentWave < waveCount)
        {
            currentWave++;
            enemiesRemaining = enemiesPerWave;
            if (!isSpawning)
            {
                StartCoroutine(SpawnWave());
            }
        }
    }

    private IEnumerator SpawnWave()
    {
        isSpawning = true;
        while (enemiesRemaining > 0)
        {
            SpawnEnemy();
            enemiesRemaining--;
            yield return new WaitForSeconds(spawnInterval);
        }
        isSpawning = false;
    }

    private void SpawnEnemy()
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPosition = new Vector3(
            player.position.x + randomCircle.x,
            0,
            player.position.z + randomCircle.y
        );

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.OnDeath += OnEnemyDeath;
        }
    }

    private void OnEnemyDeath()
    {
        if (!isSpawning && enemiesRemaining <= 0)
        {
            StartNextWave();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.position, spawnRadius);
        }
    }
}
