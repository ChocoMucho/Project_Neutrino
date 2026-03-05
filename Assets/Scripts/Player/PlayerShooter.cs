using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputContainer inputContainer;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootInterval = 0.2f;
    private float shootIntervalTimer = 0f;

    void Start()
    {
        shootIntervalTimer = shootInterval;
        inputContainer = GetComponent<InputContainer>();
    }

    void Update()
    {
        shootIntervalTimer -= Time.deltaTime;
        if (shootIntervalTimer < 0f)
        {             
            if (inputContainer.IsAttackPressed)
            {
                Shoot();
                shootIntervalTimer = shootInterval;
            }
        }
    }

    public void Shoot()
    {
        var bullet = PoolManager.Instance.Spawn(bulletPrefab.GetComponent<Bullet>());
        Vector2 dir = (inputContainer.MousePosition - transform.position).normalized;

        bullet.Init(transform.position,dir, 10f, LayerMask.NameToLayer("PlayerBullet"));
    }
}
