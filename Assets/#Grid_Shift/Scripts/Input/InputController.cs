using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public event Action<Vector2Int> OnMove;
    public event Action OnUndo;
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Move.performed += HandleMove;
        inputActions.Player.Undo.performed += HandleUndo;
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= HandleMove;
        inputActions.Player.Undo.performed -= HandleUndo;

        inputActions.Player.Disable();
    }

    private void OnDestroy()
    {
        inputActions.Dispose();
    }

    private void HandleMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        Vector2Int direction;

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            direction = input.x > 0
                ? Vector2Int.right
                : Vector2Int.left;
        }
        else
        {
            direction = input.y > 0
                ? Vector2Int.up
                : Vector2Int.down;
        }

        OnMove?.Invoke(direction);
    }
    private void HandleUndo(InputAction.CallbackContext context)
    {
        OnUndo?.Invoke();
    }
}