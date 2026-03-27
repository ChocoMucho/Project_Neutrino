using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHealth : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Sprite heartSprite; 

    private GameObject[] heartImages; 

    private int storedHealthValue;

    private void Start()
    {
        InitializeUI();

        if (playerHealth != null)
        {
            playerHealth.OnHealthDecreased += DecreaseHeartImage;
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthDecreased -= DecreaseHeartImage;
        }
    }

    private void InitializeUI()
    {
        if (playerHealth == null) return;

        storedHealthValue = playerHealth.MaxHealth;

        heartImages = new GameObject[storedHealthValue];

        for (int i = 0; i < storedHealthValue; i++) // 체력 이미지 생성 추가
        {
            GameObject newHeartObj = new GameObject("Heart_" + i);
            newHeartObj.transform.SetParent(this.transform, false);
            Image newHeartImage = newHeartObj.AddComponent<Image>();
            newHeartImage.sprite = heartSprite;
            heartImages[i] = newHeartObj;
        }
    }

    public void DecreaseHeartImage()
    {
        if (storedHealthValue <= 0) return;

        storedHealthValue--;

        if (storedHealthValue < heartImages.Length)
        {
            heartImages[storedHealthValue].SetActive(false);
        }
    }
}
