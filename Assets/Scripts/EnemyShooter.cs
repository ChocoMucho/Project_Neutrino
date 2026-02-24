using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform muzzleTransform;
    [SerializeField] private float shootInterval = 0.1f;
    private float shootIntervalTimer = 0f;

    void Start()
    {
        shootIntervalTimer = shootInterval;
    }

    void Update()
    {
        if (bulletPrefab != null)
        {
            shootIntervalTimer -= Time.deltaTime;
            if (shootIntervalTimer <= 0f)
            {
                Shoot();
                shootIntervalTimer = shootInterval;
            }
        }
    }

    public void Shoot()
    {
        var bullet = PoolManager.Instance.Spawn(bulletPrefab.GetComponent<Bullet>());
        bullet.transform.position = muzzleTransform.position;
        bullet.Init(muzzleTransform.up, 5f, LayerMask.NameToLayer("EnemyBullet"));
    }
}
