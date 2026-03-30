using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_OptionPanel : MonoBehaviour
{
    public Button closeButton;

    void Start()
    {
        // 닫기 버튼에 클릭 이벤트 추가
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));// 애니메이션이 끝난 후 패널 비활성화
    }
}
