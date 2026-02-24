using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public int ScoreAmount { get; private set; }

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
        Debug.Log($"Gained {amount} Score. Total Score: {ScoreAmount}");
    }
}
