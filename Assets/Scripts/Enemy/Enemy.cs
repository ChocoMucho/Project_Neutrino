using UnityEngine;

public class Enemy : MonoBehaviour, IPoolable
{
    public Transform _playerTransform;
    private Health health;

    private EnemyDataSO enemyData;
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
        
        health.OnDeath += () => {
            ScoreManager.Instance.AddScore(enemyData.ScoreReward);
        };
        health.OnDeath += RequestDespawn;
        stateMachine = new EnemyStateMachine(this);
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

    public void Init(EnemyDataSO enemyData)
    {
        if(enemyData == null)
        {
            Debug.LogError("EnemyDataSO is null!");
            return;
        }
        this.enemyData = enemyData;
        speed = this.enemyData.Speed;
        damage = this.enemyData.Damage;
        color = this.enemyData.EnemyColor;

        stateMachine.Init(enemyData);
    }

    public void OnSpawn()
    {
        WaveManager.Instance.AddSpawnCount();
    }

    public void OnDespawn()
    {
        WaveManager.Instance.SubtractSpawnCount();
    }

    private void RequestDespawn()
    {
        PoolManager.Instance.Despawn(this);
    }

    public void TryStartBulletAttack()
    {
        if (enemyData == null || enemyData.Type != EnemyType.Bullet) return;
        if (enemyData.Pattern == null) return;
        if (shooter == null) return;

        shooter.TryStartPatternAttack(enemyData.Pattern);
    }
}
