using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffect : MonoBehaviour
{
    // 버튼 클릭 시 애니메이션 효과를 위한 변수
    [Range(0.01f, 0.5f)] public float duration = 0.1f;
    [Range(0.5f, 1.5f)] public float downScale = 0.9f;
    private RectTransform rectTransform;

    public Action OnButtonUP; 

    public void OnPointerDown(PointerEventData eventData)
    {
        rectTransform.DOKill();
        rectTransform.DOScale(downScale, duration).SetEase(Ease.OutQuad);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnButtonUP?.Invoke(); // 버튼이 눌렸을 때 실행할 동작을 호출
    }
}
