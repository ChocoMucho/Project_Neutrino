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
    public EnemyType enemyType;

    public GameObject EnemyPrefab;
    public int HP;
    public int Damage;
    public int Speed;
    public Color EnemyColor;
    public int ExpReward;

    [Header("FSM")]
    public float HoldTime;
    public Vector2 ExitDirection;
}
