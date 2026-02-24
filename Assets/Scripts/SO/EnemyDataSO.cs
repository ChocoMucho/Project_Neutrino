using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Scriptable Objects/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    public GameObject EnemyPrefab;
    public int HP;
    public int Damage;
    public int Speed;
    public Color EnemyColor;
    public int ExpReward;
}
