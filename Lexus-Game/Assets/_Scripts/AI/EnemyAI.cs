using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float detectionRange = 20f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string walkParameterName = "IsWalking";
    [SerializeField] private string attackParameterName = "Attack";

    [Header("Son")]
    [SerializeField] private AudioClip[] attackSounds;
    [SerializeField] private AudioClip[] deathSounds;
    [SerializeField] private AudioClip[] hitSounds;

    private NavMeshAgent agent;
    private Transform player;
    private float nextAttackTime;
    private AudioSource audioSource;
    private bool isDead;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Configurer l'agent
        agent.stoppingDistance = attackRange;
    }

    private void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Mettre à jour l'animation de marche
        if (animator != null)
        {
            animator.SetBool(walkParameterName, agent.velocity.magnitude > 0.1f);
        }

        if (distanceToPlayer <= detectionRange)
        {
            // Se déplacer vers le joueur
            agent.SetDestination(player.position);

            // Attaquer si assez proche
            if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
            {
                Attack();
            }
        }
        else
        {
            // Arrêter de poursuivre si le joueur est trop loin
            agent.ResetPath();
        }
    }

    private void Attack()
    {
        nextAttackTime = Time.time + attackRate;

        // Animation d'attaque
        if (animator != null)
        {
            animator.SetTrigger(attackParameterName);
        }

        // Son d'attaque
        if (attackSounds != null && attackSounds.Length > 0)
        {
            AudioClip randomAttackSound = attackSounds[Random.Range(0, attackSounds.Length)];
            audioSource.PlayOneShot(randomAttackSound);
        }

        // Infliger des dégâts au joueur
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(float damage)
    {
        // Jouer un son de dégât
        if (hitSounds != null && hitSounds.Length > 0)
        {
            AudioClip randomHitSound = hitSounds[Random.Range(0, hitSounds.Length)];
            audioSource.PlayOneShot(randomHitSound);
        }
    }

    public void Die()
    {
        isDead = true;
        agent.isStopped = true;

        // Animation de mort
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        // Son de mort
        if (deathSounds != null && deathSounds.Length > 0)
        {
            AudioClip randomDeathSound = deathSounds[Random.Range(0, deathSounds.Length)];
            audioSource.PlayOneShot(randomDeathSound);
        }

        // Désactiver les collisions
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        // Détruire l'objet après la fin de l'animation/son
        float longestClipLength = 0f;
        if (deathSounds != null && deathSounds.Length > 0)
        {
            foreach (AudioClip clip in deathSounds)
            {
                longestClipLength = Mathf.Max(longestClipLength, clip.length);
            }
        }
        Destroy(gameObject, longestClipLength + 1f);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualiser les rayons d'attaque et de détection
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
