using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private EnemyDataSO enemyData;
    [SerializeField] private float spawnInterval = 2.0f;
    [SerializeField] FormationDataSO formationData;

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
        // SO에서 방향 읽고 viewpoint로 소환 지점 잡기
        Vector2 spawnSidePos = GetSpawnSidePos(formationData.spawnSide);
        // points 만큼 반복
        for(int i = 0; i < formationData.points.Count; ++i)
        {
            Vector2 holdPoint = formationData.points[i];
            var enemy = PoolManager.Instance.Spawn(formationData.enemyData.EnemyPrefab.GetComponent<Enemy>());
            enemy.HoldPosition = holdPoint;
            enemy.transform.position = spawnSidePos;
            enemy.Init();
        }
    }

    private Vector2 GetSpawnSidePos(SpawnSide spawnSide)
    {
        Vector3 pos = Vector3.zero;
        switch (spawnSide)
        {
            case SpawnSide.Left:
                pos.x = -0.1f;
                pos.y = 0.5f;
                break;
            case SpawnSide.Right:
                pos.x = 1.1f;
                pos.y = 0.5f;
                break;
            case SpawnSide.Bottom:
                pos.x = 0.5f;
                pos.y = -0.1f;
                break;
            case SpawnSide.Top:
                pos.x = 0.5f;
                pos.y = 1.1f;
                break;
        }

        pos.z = 10f; // 카메라에서 어느정도 떨어진 위치
        Vector3 worldPos = mainCamera.ViewportToWorldPoint(pos);
        worldPos.z = 0f; // z축 0으로 고정

        return worldPos;
    }

}
