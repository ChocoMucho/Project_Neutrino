using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "Scriptable Objects/BulletData")]
public class BulletDataSO : ScriptableObject
{
    public Sprite bulletSprite;       // 총알 스프라이트
    public float speed = 10f;         // 총알 속도
    public int damage = 1;            // 총알 공격력
}