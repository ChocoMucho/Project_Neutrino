using UnityEngine;

[CreateAssetMenu(fileName = "PatternData", menuName = "Scriptable Objects/PatternData")]
public class PatternDataSO : ScriptableObject
{
    public int shotCount = 3;           // 한 번에 쏘는 총알 수
    public float angleGap = 10f;        // 총알 간 각도
    public float startAngle = 0f;       // 시작 각도 (기본: Vector2.up)
    public float rotateSpeed = 0f;      // 발사 시 회전 속도 (나선형 패턴용)
    public int waveCount = 1;           // 발사 횟수
    public float fireInterval = 0.5f;   // 연사 간격
}