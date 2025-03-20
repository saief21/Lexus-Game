using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private int moneyValue = 5;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private AudioClip deathSound;
    
    public event Action OnDeath;
    
    private float currentHealth;
    
    private void Start()
    {
        currentHealth = maxHealth;
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
        // Effets visuels et sonores
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }
        
        // Ajouter le score et l'argent
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
            GameManager.Instance.AddCurrency(moneyValue);
        }
        
        // Notifier les autres systèmes
        OnDeath?.Invoke();
    }
}
