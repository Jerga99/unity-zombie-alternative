using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject
{
    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction<bool> RollEvent = delegate { };

    public bool LeftMouseDown() => Mouse.current.leftButton.isPressed;
    public bool RightMouseDown() => Mouse.current.rightButton.isPressed;
    public Vector3 MousePosition() => Mouse.current.position.ReadValue();

    public void OnMove(Vector2 value)
    {
        MoveEvent.Invoke(value);
    }

    public void OnRoll(bool value)
    {
        RollEvent.Invoke(value);
    }


}

