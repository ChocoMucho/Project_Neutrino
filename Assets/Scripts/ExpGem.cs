using UnityEngine;

public class ExpGem : MonoBehaviour, IPoolable
{
    [Header("Gem Settings")]
    [SerializeField] private float delayToMove = 0.5f; 
    [SerializeField] private float moveSpeed = 10f;    

    private int expAmount;
    private Transform playerTransform;
    private float timer;
    private bool isMoving;

    public void Init(int amount)
    {
        expAmount = amount;
    }

    void Update()
    {
        if (!isMoving)
        {
            timer += Time.deltaTime;
            if (timer >= delayToMove)
            {
                isMoving = true;
            }
        }
        else
        {
            if (playerTransform != null)
            {
                transform.position = Vector3.Lerp(transform.position, playerTransform.position, Time.deltaTime * moveSpeed);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(expAmount);
            }

            PoolManager.Instance.Despawn(this);
        }
    }

    public void OnSpawn()
    {
        timer = 0f;
        isMoving = false;

        if (playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }
    }

    public void OnDespawn()
    {
    }
}
