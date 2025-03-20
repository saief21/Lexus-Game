using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth = 50;
    public int damage = 10;
    public float attackRange = 2f;
    public float attackRate = 1f;
    public int scoreValue = 100;
    public int moneyValue = 25;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float detectionRange = 10f;

    private int currentHealth;
    private Transform player;
    private NavMeshAgent agent;
    private float nextAttackTime;
    private GameManager gameManager;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            
            if (distanceToPlayer <= detectionRange)
            {
                agent.SetDestination(player.position);

                if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
                {
                    Attack();
                }
            }
        }
    }

    void Attack()
    {
        nextAttackTime = Time.time + attackRate;
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        gameManager.AddScore(scoreValue);
        gameManager.AddMoney(moneyValue);
        Destroy(gameObject);
    }
}
