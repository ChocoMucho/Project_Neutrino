using UnityEngine;
using UnityEngine.InputSystem;

public class InputContainer : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool IsFocusPressed { get; private set; }

    public void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();
    }

    public void OnFocus(InputValue value)
    {
        IsFocusPressed = value.isPressed;
    }
}
