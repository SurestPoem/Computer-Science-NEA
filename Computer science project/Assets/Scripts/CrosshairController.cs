using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    public Image crosshair; // The crosshair UI element
    public float moveSpeed = 10f; // Speed of the crosshair when using controller
    private Vector2 crosshairPosition;

    private Vector2 mouseInput;
    private Vector2 joystickInput;

    private bool useJoystick; // Flag to determine whether to use the joystick

    // Reference to the input action system
    private CrosshairControls inputActions; // This should match the generated class name
    private InputAction mouseMoveAction;
    private InputAction joystickMoveAction;


    void Start()
    {
        crosshairPosition = crosshair.transform.position; // Set initial position
    }

    private void Awake()
    {
        inputActions = new CrosshairControls(); // Instantiate the generated Input Actions class

        // Get the mouse and joystick input actions from the Input Action Asset
        mouseMoveAction = inputActions.Player.MouseMovement; // Reference MouseMovement action
        joystickMoveAction = inputActions.Player.JoystickMovement; // Reference JoystickMovement action

        // Enable the actions
        mouseMoveAction.Enable();
        joystickMoveAction.Enable();
    }

    private void Update()
{
    // Read mouse and joystick inputs
    mouseInput = mouseMoveAction.ReadValue<Vector2>();
    joystickInput = joystickMoveAction.ReadValue<Vector2>();

    // Switch control mode based on recent input
    if (joystickInput.magnitude > 0.1f)
    {
        useJoystick = true;
    }
    else if (mouseInput != crosshairPosition) // If mouse moves, switch to mouse control
    {
        useJoystick = false;
    }

    // Move the crosshair based on the active input method
    if (useJoystick)
    {
        crosshairPosition += joystickInput * moveSpeed * Time.deltaTime;

        // Clamp to screen boundaries
        crosshairPosition.x = Mathf.Clamp(crosshairPosition.x, 0, Screen.width);
        crosshairPosition.y = Mathf.Clamp(crosshairPosition.y, 0, Screen.height);
    }
    else
    {
        crosshairPosition = mouseInput;
    }

    // Apply position to UI element
    crosshair.transform.position = crosshairPosition;
}

    private void OnDestroy()
    {
        // Clean up the actions
        mouseMoveAction.Disable();
        joystickMoveAction.Disable();
    }
}