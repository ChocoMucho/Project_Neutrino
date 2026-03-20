using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    private Camera mainCamera;

    [Header("Wave Data")]
    [SerializeField] private WaveDataSO waveData;
    [SerializeField] private int spawnCount;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        mainCamera = Camera.main;

        StartWave();
    }

    void Update()
    {
        /*timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }*/
    }

    public void StartWave()
    {
        StartCoroutine(ExecuteStageWaves());
    }

    private IEnumerator ExecuteStageWaves()
    {
        foreach (var wave in waveData.waveDataList)
        {
            SpawnEnemy(wave.formationData);

            if (wave.isClearWait)
            {
                yield return new WaitUntil(() => spawnCount <= 0);
            }
            else
            {
                yield return new WaitForSeconds(wave.waveDuration);
            }
        }
    }

    public void SpawnEnemy(FormationDataSO formationData)
    {
        // SO에서 방향 읽고 viewpoint로 소환 지점 잡기
        Vector2 spawnSidePos = GetSpawnSidePos(formationData.spawnSide);
        // points 만큼 반복
        for (int i = 0; i < formationData.points.Count; ++i)
        {
            Vector2 holdPoint = formationData.points[i];
            var enemy = PoolManager.Instance.Spawn(formationData.enemyData.EnemyPrefab.GetComponent<Enemy>());
            enemy.HoldPosition = holdPoint;
            enemy.transform.position = spawnSidePos;
            enemy.Init(formationData.enemyData);
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

    public void AddSpawnCount() => spawnCount++;
    public void SubtractSpawnCount() => spawnCount--;
}
