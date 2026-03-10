using UnityEngine;

public class Enemy : MonoBehaviour, IPoolable
{
    public Transform _playerTransform;
    private Health health;

    [SerializeField] private EnemyDataSO enemyDataSO;
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private Color color;

    [Header("Drop ExpGem Settings")]
    [SerializeField] private GameObject expGemPrefab;

    [Header("FSM")]
    EnemyStateMachine stateMachine;
    public Vector2 HoldPosition;

    private EnemyShooter shooter;

    private void Awake()
    {
        health = GetComponent<Health>();
        shooter = GetComponent<EnemyShooter>();
        stateMachine = new EnemyStateMachine(this, enemyDataSO);

        health.OnDeath += () => {
            ScoreManager.Instance.AddScore(enemyDataSO.ScoreReward);
        };
        health.OnDeath += RequestDespawn;
        stateMachine.Init();
    }

    void Start()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        //RotateTowardPlayer();
        stateMachine.Update();
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

    private void RequestDespawn()
    {
        PoolManager.Instance.Despawn(this);
    }

    public void TryStartBulletAttack()
    {
        if (enemyDataSO == null || enemyDataSO.Type != EnemyType.Bullet) return;
        if (enemyDataSO.Pattern == null) return;
        if (shooter == null) return;

        shooter.TryStartPatternAttack(enemyDataSO.Pattern);
    }
}
