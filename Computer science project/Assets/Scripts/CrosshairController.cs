using UnityEngine;
using UnityEngine.InputSystem;

public class CrosshairController : MonoBehaviour
{
    public SpriteRenderer crosshairSprite; // Crosshair as a world-space GameObject
    public float moveSpeed = 10f; // Speed of the crosshair when using a controller

    private Vector2 crosshairPosition;  // Crosshair's position in world space
    private Vector2 mouseInput;         // Stores mouse position
    private Vector2 joystickInput;      // Stores joystick movement
    private Vector2 lastMousePosition;  // Stores last known mouse position

    private bool useJoystick = false;  // Start with mouse mode enabled
    private bool useMouse = true;

    private PlayerControls inputActions;  // Reference to the input system
    private InputAction mouseMoveAction;
    private InputAction joystickMoveAction;
    private Player player;

    private Camera mainCamera;  // Reference to the main camera
    public float maxCrosshairDistance = 5f; // Maximum distance the crosshair can move from the player
    private Transform playerTransform; // Reference to the player's transform

    void Start()
    {
        mainCamera = Camera.main; // Cache the main camera
        crosshairPosition = transform.position; // Set initial position
        lastMousePosition = Mouse.current.position.ReadValue(); // Store initial mouse position
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Find the player
    }

    private void Awake()
    {
        inputActions = new PlayerControls();  // Instantiate input actions

        mouseMoveAction = inputActions.CrosshairMovement.MouseMovement;   // Get mouse movement
        joystickMoveAction = inputActions.CrosshairMovement.JoystickMovement; // Get joystick movement

        mouseMoveAction.Enable();
        joystickMoveAction.Enable();
    }

    private void Update()
    {
        // Read inputs
        mouseInput = mouseMoveAction.ReadValue<Vector2>();
        joystickInput = joystickMoveAction.ReadValue<Vector2>();

        // Detect joystick movement
        if (joystickInput.magnitude > 0.1f)
        {
            useJoystick = true;
            useMouse = false;
            Cursor.visible = false; // Hide cursor when using joystick
        }
        else
        {
            // Only switch to mouse if the mouse has actually moved
            if ((mouseInput - lastMousePosition).sqrMagnitude > 1f)  // Check if mouse moved
            {
                useJoystick = false;
                useMouse = true;
                Cursor.visible = true; // Show cursor when using mouse
            }
        }

        // Move the crosshair based on the active input method
        if (useJoystick)
        {
            crosshairPosition += joystickInput * moveSpeed * Time.deltaTime;
        }
        else if (useMouse)
        {
            // Convert screen position to world position
            Vector3 worldMousePos = mainCamera.ScreenToWorldPoint(new Vector3(mouseInput.x, mouseInput.y, mainCamera.nearClipPlane));
            crosshairPosition = new Vector2(worldMousePos.x, worldMousePos.y);
        }

        // Clamp the crosshair position within the screen bounds
        ClampCrosshairPosition();

        // Apply world-space position
        transform.position = crosshairPosition;

        // Store last known mouse position for movement detection
        lastMousePosition = mouseInput;
    }

    private void OnDestroy()
    {
        // Clean up actions
        mouseMoveAction.Disable();
        joystickMoveAction.Disable();
    }

    private void ClampCrosshairPosition()
    {
        // Get screen bounds in world space
        Vector3 screenBottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));

        // Clamp the crosshair position within the screen bounds
        crosshairPosition.x = Mathf.Clamp(crosshairPosition.x, screenBottomLeft.x, screenTopRight.x);
        crosshairPosition.y = Mathf.Clamp(crosshairPosition.y, screenBottomLeft.y, screenTopRight.y);

        // Calculate the direction from the player to the crosshair
        Vector2 directionToPlayer = crosshairPosition - (Vector2)playerTransform.position;

        // Check if the crosshair is beyond the allowed max distance
        if (directionToPlayer.magnitude > maxCrosshairDistance)
        {
            // Normalize the direction to have a magnitude of 1, then multiply by max distance
            directionToPlayer = directionToPlayer.normalized * maxCrosshairDistance;

            // Set the crosshair position based on the new direction (relative to the player)
            crosshairPosition = (Vector2)playerTransform.position + directionToPlayer;
        }
    }
}
