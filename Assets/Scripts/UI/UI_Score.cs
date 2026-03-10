using UnityEngine;
using UnityEngine.UI;

public class UI_Score : MonoBehaviour
{
    [SerializeField] private Text text;

    void Start()
    {
        text = GetComponent<Text>();
        ScoreManager.Instance.OnScoreChanged += UpdateScoreText;
    }

    public void UpdateScoreText()
    {
        text.text = ScoreManager.Instance.ScoreAmount.ToString();
    }
}
