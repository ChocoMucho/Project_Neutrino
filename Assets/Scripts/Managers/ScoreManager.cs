using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public int ScoreAmount { get; private set; }

    public Action OnScoreChanged; // UI에서 구독할 action

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddScore(int amount)
    {
        ScoreAmount += amount;
        OnScoreChanged?.Invoke();
        Debug.Log($"Gained {amount} Score. Total Score: {ScoreAmount}");
    }
}
