using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputContainer inputContainer;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float focusMoveSpeed = 2.5f;

    private void Awake()
    {
        inputContainer = GetComponent<InputContainer>();
    }

    void Update()
    {
        Move();
    }

    public void Move()
    {
        if (inputContainer == null)
        {
            return;
        }

        Vector2 moveInput = inputContainer.MoveInput;
        if (moveInput == Vector2.zero)
        {
            return;
        }

        moveInput = moveInput.normalized;

        float currentSpeed = inputContainer.IsFocusPressed ? focusMoveSpeed : moveSpeed;

        Vector3 delta = new Vector3(moveInput.x, moveInput.y, 0f) * (currentSpeed * Time.deltaTime);
        transform.Translate(delta, Space.World);

        // player position clamping
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        viewportPos.x = Mathf.Clamp01(viewportPos.x);
        viewportPos.y = Mathf.Clamp01(viewportPos.y);
        transform.position = Camera.main.ViewportToWorldPoint(viewportPos);
    }
}
