using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Statistiques")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private int currencyValue = 5;

    [Header("Navigation")]
    [SerializeField] private float detectionRange = 20f;
    [SerializeField] private float moveSpeed = 5f;

    private float currentHealth;
    private NavMeshAgent agent;
    private Transform player;
    private float nextAttackTime;

    private void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        agent.speed = moveSpeed;
        agent.stoppingDistance = attackRange;
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Si le joueur est à portée de détection
        if (distanceToPlayer <= detectionRange)
        {
            // Suivre le joueur
            agent.SetDestination(player.position);

            // Attaquer si assez proche
            if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        nextAttackTime = Time.time + attackRate;
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.AddScore(scoreValue);
        GameManager.Instance.AddCurrency(currencyValue);
        Destroy(gameObject);
    }
}
