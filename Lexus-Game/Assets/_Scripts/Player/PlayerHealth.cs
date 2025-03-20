using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Paramètres de Santé")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    public UnityEvent<float> onHealthChanged;
    public UnityEvent onPlayerDeath;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        onHealthChanged?.Invoke(currentHealth / maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        onHealthChanged?.Invoke(currentHealth / maxHealth);
    }

    private void Die()
    {
        onPlayerDeath?.Invoke();
        // Désactiver les contrôles du joueur
        GetComponent<PlayerController>().enabled = false;
    }
}
