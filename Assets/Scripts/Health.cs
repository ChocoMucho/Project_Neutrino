using System;
using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int currentHealth;

    public Action OnHit; // important
    public Action OnDeath; // important
    public Action OnHealthDecreased; // important

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    void Start()
    {   
        currentHealth = maxHealth;
    }

    public void OnDamage(int damageAmount)
    {
        // 플레이어가 무적 상태인 경우 데미지를 무시합니다.
        // TODO: 이 부분은 Player 클래스에서 관리하는 것이 더 적절할 수 있음. 현재는 Health 컴포넌트가 Player의 상태를 직접 확인하고 있음.
        Player player = GetComponent<Player>();
        if (player != null)
        {
            if (player.IsInvincible)
            {
                Debug.Log($"{gameObject.name} is invincible and took no damage.");
                return;
            }
        }

        OnHit?.Invoke();
        currentHealth -= damageAmount;
        OnHealthDecreased?.Invoke();
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
