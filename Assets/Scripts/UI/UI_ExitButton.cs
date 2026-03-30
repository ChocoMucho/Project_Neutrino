using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ExitButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
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
        rectTransform.DOKill();
        rectTransform.DOScale(1, duration).SetEase(Ease.OutBack);
        
        Application.Quit();
    }
}
