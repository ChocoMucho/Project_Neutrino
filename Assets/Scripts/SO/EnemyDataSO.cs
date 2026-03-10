using UnityEngine;

public enum EnemyType
{
    Dash,
    Bullet,
}

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Scriptable Objects/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    [Header("Enemy Type")]
    public EnemyType Type;

    public GameObject EnemyPrefab;
    public int HP;
    public int Damage;
    public int Speed;
    public Color EnemyColor;
    public int ExpReward;
    public int ScoreReward;

    [Header("Bullet Attack (Type = Bullet)")]
    public PatternDataSO Pattern;

    [Header("FSM")]
    public float HoldTime;
    public Vector2 ExitDirection;
    public float rotationSpeed;
}
