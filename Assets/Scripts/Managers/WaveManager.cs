using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private EnemyDataSO enemyData;
    [SerializeField] private float spawnInterval = 2.0f;

    private float timer = 0f;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        timer = spawnInterval; // Immediate spawn on start
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    public void SpawnEnemy()
    {
        Vector3 spawnPos = GetRandomSpawnPos();
        Enemy spawnedEnemy = PoolManager.Instance.Spawn(enemyData.EnemyPrefab.GetComponent<Enemy>());
        // Spawn된 적의 위치 설정
        spawnedEnemy.transform.position = spawnPos;
        spawnedEnemy.Init();
    }

    public Vector3 GetRandomSpawnPos()
    {
        Vector3 pos = Vector3.zero;
        // 0~1 사이의 랜덤 값
        // 스위치문으로 4군데 중 한군데 선택
        int side = Random.Range(0, 4);
        switch (side)
        {
            case 0: // Left
                pos.x = -0.1f;
                pos.y = Random.Range(0f, 1f);
                break;
            case 1: // Right
                pos.x = 1.1f;
                pos.y = Random.Range(0f, 1f);
                break;
            case 2: // Bottom
                pos.x = Random.Range(0f, 1f);
                pos.y = -0.1f;
                break;
            case 3: // Top
                pos.x = Random.Range(0f, 1f);
                pos.y = 1.1f;
                break;
        }
        pos.z = 10f; // 카메라에서 어느정도 떨어진 위치
        Vector3 worldPos = mainCamera.ViewportToWorldPoint(pos);
        worldPos.z = 0f; // z축 0으로 고정

        return worldPos;
    }
}
