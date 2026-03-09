using UnityEngine;
using System.Collections;

public class PatternTester : MonoBehaviour
{
    [SerializeField] PatternDataSO pattern;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] GameObject muzzle;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Fire());
        }
    }

    private IEnumerator Fire()
    {
        if (pattern == null || bulletPrefab == null) yield break;

        var bulletComponent = bulletPrefab.GetComponent<Bullet>();
        if (bulletComponent == null) yield break;

        int layer = LayerMask.NameToLayer("EnemyBullet");
        Vector2 spawnPos = transform.position;

        for (int wave = 0; wave < pattern.waveCount; wave++)
        {
            float waveBaseAngle = pattern.startAngle + wave * pattern.rotateSpeed;

            for (int i = 0; i < pattern.shotCount; i++)
            {
                float angle;
                if (pattern.shotCount >= 2)
                {
                    float fanStartAngle = -(pattern.angleGap * (pattern.shotCount - 1)) / 2f;
                    angle = waveBaseAngle + fanStartAngle + i * pattern.angleGap;
                }
                else
                {
                    angle = waveBaseAngle;
                }

                Vector2 direction = (Quaternion.Euler(0, 0, angle) * muzzle.transform.up).normalized;
                var bullet = Instantiate(bulletComponent);
                bullet.Init(spawnPos, direction, bulletSpeed, layer);
            }

            if (wave < pattern.waveCount - 1)
            {
                yield return new WaitForSeconds(pattern.fireInterval);
            }
        }
    }
}
