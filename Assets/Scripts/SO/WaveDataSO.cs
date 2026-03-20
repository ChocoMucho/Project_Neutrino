using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct WaveData
{
    public FormationDataSO formationData;
    public float waveDuration;
    public bool isClearWait;
}

[CreateAssetMenu(fileName = "WaveDataSO", menuName = "Scriptable Objects/WaveDataSO")]
public class WaveDataSO : ScriptableObject
{
    public List<WaveData> waveDataList = new List<WaveData>();
}
