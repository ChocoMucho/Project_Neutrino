using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Camera camera;
    // 진짜 임시
    public GameObject bulletPrefab;

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        Move();

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            var bullet = PoolManager.Instance.Spawn(bulletPrefab.GetComponent<Bullet>());
            bullet.transform.position = transform.position + new Vector3(0,1,0);
        }
    }

    public void Move()
    {
        // 마우스 위치로 이동
        // 마우스 위치 2d 월드 좌표로 변환 후에 캐릭터를 그 위치로 이동(Lerp나 SmotthhDamp 사용)
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPosition = camera.ScreenToWorldPoint(mousePos);
        worldPosition.z = 0; // z축 고정
        transform.position = Vector3.Lerp(transform.position, worldPosition, Time.deltaTime * 5f);
    }
}
