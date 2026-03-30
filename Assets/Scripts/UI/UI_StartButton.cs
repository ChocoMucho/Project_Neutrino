using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections;

public class UI_StartButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // 버튼 클릭 시 애니메이션 효과를 위한 변수
    [Range(0.01f, 0.5f)] public float duration = 0.1f;
    [Range(0.5f, 1.5f)] public float downScale = 0.9f;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rectTransform.DOKill();
        rectTransform.DOScale(downScale, duration).SetEase(Ease.OutQuad);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StartCoroutine(LoadSceneWithDelay());
    }

    // 버튼 클릭 후 씬 전환까지 딜레이를 주기 위한 코루틴
    private IEnumerator LoadSceneWithDelay()
    {
        rectTransform.DOKill();
        rectTransform.DOScale(1, duration).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(duration);
        
        SceneManagerEx.Instance.LoadScene(SceneType.InGameScene);
    }
}
