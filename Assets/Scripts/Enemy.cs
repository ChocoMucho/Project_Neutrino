using UnityEngine;

public class Enemy : MonoBehaviour, IPoolable, IDamageable
{
    private Transform _playerTransform;

    [SerializeField] private EnemyDataSO enemyDataSO;
    [SerializeField] private int currentHealth;
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private Color color;

    [Header("Drop ExpGem Settings")]
    [SerializeField] private GameObject expGemPrefab;

    void Start()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        RotateTowardPlayer();
    }

    private void RotateTowardPlayer()
    {
        // 1. dir
        Vector2 direction = _playerTransform.position - transform.position;
        // 2. angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // 3. angle - 90f
        transform.rotation = Quaternion.Euler(0,0, angle - 90f);
    }

    public void Init()
    {
        currentHealth = enemyDataSO.HP;
        speed = enemyDataSO.Speed;
        damage = enemyDataSO.Damage;
        color = enemyDataSO.EnemyColor;
    }

    public void OnSpawn()
    {
    }

    public void OnDespawn()
    {
    }

    public void OnDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log($"{gameObject.name} took {damageAmount} damage. Current health: {currentHealth}");
        if (currentHealth <= 0)
        {
            Die();
            RequestDespawn();
        }
    }

    public void Die()
    {
        if (expGemPrefab != null)
        {
            ExpGem gem = PoolManager.Instance.Spawn(expGemPrefab.GetComponent<ExpGem>());
            gem.transform.position = this.transform.position;

            gem.Init(enemyDataSO.ExpReward);
        }
    }

    private void RequestDespawn()
    {
        PoolManager.Instance.Despawn(this);
    }
}
