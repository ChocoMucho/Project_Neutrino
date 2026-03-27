using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerGlitchEffect glitchEffect;

    [SerializeField] private float invincibleDuration = 0.5f;
    public bool IsInvincible { get; private set; }

    private Health health;

    void Start()
    {
        glitchEffect = GetComponentInChildren<PlayerGlitchEffect>();
        health = GetComponent<Health>();
        if (health != null)
        {
            health.OnHit += TriggerInvincibility;
        }
    }

    public void TriggerInvincibility()
    {
        StartCoroutine(InvincibilityCoroutine());
    }

    private IEnumerator InvincibilityCoroutine()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null && glitchEffect != null)
        {
            collider.enabled = false;
            glitchEffect.SetGlitch(true);
            IsInvincible = true;

            yield return new WaitForSeconds(invincibleDuration);

            collider.enabled = true;
            glitchEffect.SetGlitch(false);
            IsInvincible = false;
        }
    }
}
