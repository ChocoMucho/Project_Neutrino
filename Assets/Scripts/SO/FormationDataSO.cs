using UnityEngine;
using System.Collections.Generic;

public enum SpawnSide
{
    Left,
    Right,
    Bottom,
    Top
}

[CreateAssetMenu(fileName = "FormationDataSO", menuName = "Scriptable Objects/FormationDataSO")]
public class FormationDataSO : ScriptableObject
{
    public EnemyDataSO enemyData;
    public SpawnSide spawnSide;
    public List<Vector2> points = new List<Vector2>();
}
