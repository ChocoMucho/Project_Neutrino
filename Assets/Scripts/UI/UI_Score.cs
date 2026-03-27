using DG.Tweening;
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
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.DOScale(1.5f, 0.1f).SetLoops(2, LoopType.Yoyo);
    }
}
