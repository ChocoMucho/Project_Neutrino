using System;
using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int currentHealth;

    public Action OnHit; // important
    public Action OnDeath; // important

    void Start()
    {   
        currentHealth = maxHealth;
    }

    public void OnDamage(int damageAmount)
    {
        OnHit?.Invoke();
        currentHealth -= damageAmount;
        Debug.Log($"{gameObject.name} took {damageAmount} damage. Current health: {currentHealth}");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        OnDeath?.Invoke();
    }
}
