using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
    private Vector2 direction;
    private float speed;
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        CheckOffScreen();
    }

    public void OnSpawn() // Inner reset
    {
        
    }

    public void Init(Vector2 direction, float speed, int layer) // Outer reset
    {
        this.direction = direction;
        this.speed = speed;
        gameObject.layer = layer;
    }

    private void RequestDespawn()
    {
        PoolManager.Instance.Despawn(this);
    }

    public void OnDespawn()
    {
        Debug.Log("Bullet despawned");
    }

    public void CheckOffScreen()
    {
        Vector3 viewPos = _mainCamera.WorldToViewportPoint(transform.position);
        if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
        {
            RequestDespawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.OnDamage(1);
            RequestDespawn();
        }
    }
}
