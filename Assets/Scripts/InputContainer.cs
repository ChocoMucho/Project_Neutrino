using UnityEngine;
using UnityEngine.InputSystem;

public class InputContainer : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public Vector3 MousePosition { get; private set; }
    public bool IsFocusPressed { get; private set; }
    public bool IsAttackPressed { get; private set; }

    private void Update()
    {
        MousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    public void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();
    }

    public void OnFocus(InputValue value)
    {
        IsFocusPressed = value.isPressed;
    }

    public void OnAttack(InputValue value)
    {
        IsAttackPressed = value.isPressed;
    }
}
