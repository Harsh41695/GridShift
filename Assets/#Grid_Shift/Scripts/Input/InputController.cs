using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public event Action<Vector2Int> OnMove;
    public event Action OnUndo;
    public event Action OnRestart;

    private InputSystem_Actions inputActions;

    [Header("Swipe Settings")]
    [SerializeField] private float minimumSwipeDistance = 50f;

    private Vector2 swipeStartPosition;
    private bool isSwiping;

    private bool inputEnabled = false;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Move.performed += HandleMove;
        inputActions.Player.Undo.performed += HandleUndo;
        inputActions.Player.Restart.performed += HandleRestart;

        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= HandleMove;
        inputActions.Player.Undo.performed -= HandleUndo;
        inputActions.Player.Restart.performed -= HandleRestart;

        inputActions.Player.Disable();
    }

    public void EnableGameplayInput()
    {
        inputEnabled = true;
    }

    private void OnDestroy()
    {
        inputActions.Dispose();
    }

    private void Update()
    {
        HandleSwipe();
    }

    // Keyboard / Gamepad
    private void HandleMove(InputAction.CallbackContext context)
    {

        if (!inputEnabled)
            return;

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

    // Mobile Swipe
    private void HandleSwipe()
    {
        if (!inputEnabled)
            return;

        if (Touchscreen.current == null)
            return;

        var touch = Touchscreen.current.primaryTouch;

        if (touch.press.wasPressedThisFrame)
        {
            swipeStartPosition = touch.position.ReadValue();
            isSwiping = true;
        }

        if (touch.press.wasReleasedThisFrame && isSwiping)
        {
            Vector2 swipeEndPosition = touch.position.ReadValue();

            Vector2 swipeDelta =
                swipeEndPosition - swipeStartPosition;

            isSwiping = false;

            if (swipeDelta.magnitude < minimumSwipeDistance)
                return;

            Vector2Int direction;

            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                direction = swipeDelta.x > 0
                    ? Vector2Int.right
                    : Vector2Int.left;
            }
            else
            {
                direction = swipeDelta.y > 0
                    ? Vector2Int.up
                    : Vector2Int.down;
            }

            OnMove?.Invoke(direction);
        }
    }

    private void HandleUndo(InputAction.CallbackContext context)
    {
        OnUndo?.Invoke();
    }

    private void HandleRestart(InputAction.CallbackContext context)
    {
        OnRestart?.Invoke();
    }
}