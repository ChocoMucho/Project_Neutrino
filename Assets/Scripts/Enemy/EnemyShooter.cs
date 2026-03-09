using UnityEngine;
using System.Collections;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform muzzleTransform;
    [SerializeField] private float shootInterval = 0.1f;
    private float shootIntervalTimer = 0f;

    private Coroutine fireRoutine;

    public bool isFiring => fireRoutine != null; // 코루틴 진행중 판별용 프로퍼티

    void Start()
    {
        shootIntervalTimer = shootInterval;
    }

    void Update()
    {
        shootIntervalTimer -= Time.deltaTime;
    }

    public void TryStartPatternAttack(PatternDataSO pattern)
    {
        if (pattern == null) return;
        if (bulletPrefab == null || muzzleTransform == null) return;
        if (fireRoutine != null) return;
        if (shootIntervalTimer > 0f) return;

        fireRoutine = StartCoroutine(FirePatternOnce(pattern));
    }

    private IEnumerator FirePatternOnce(PatternDataSO pattern)
    {
        try
        {
            var bulletComponent = bulletPrefab.GetComponent<Bullet>();
            if (bulletComponent == null)
            {
                fireRoutine = null;
                yield break;
            }

            int layer = LayerMask.NameToLayer("EnemyBullet");
            float bulletSpeed = 5f;

            for (int wave = 0; wave < pattern.waveCount; wave++)
            {
                float waveBaseAngle = pattern.startAngle + wave * pattern.rotateSpeed;

                int shotCount = Mathf.Max(0, pattern.shotCount);
                for (int i = 0; i < shotCount; i++)
                {
                    float angleOffset;
                    if (shotCount >= 2)
                    {
                        float fanStartAngle = -(pattern.angleGap * shotCount) / 2f;
                        angleOffset = waveBaseAngle + fanStartAngle + i * pattern.angleGap;
                    }
                    else
                    {
                        angleOffset = waveBaseAngle;
                    }

                    Vector2 direction = (Quaternion.Euler(0, 0, angleOffset) * (Vector2)muzzleTransform.up).normalized;

                    var bullet = Object.Instantiate(bulletComponent);
                    bullet.Init(muzzleTransform.position, direction, bulletSpeed, layer);
                }

                if (wave < pattern.waveCount - 1)
                {
                    yield return new WaitForSeconds(pattern.fireInterval);
                }
            }
        }
        finally
        {
            shootIntervalTimer = shootInterval;
            fireRoutine = null;
        }
    }
}
