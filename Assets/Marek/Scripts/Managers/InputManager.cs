using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputHandler input;
    public static InputManager instace;

    [Header("Movement")]
    [Tooltip("Name of the axis in 'Project Settings - Input' for moving the player forward/backward")]
    public string moveHorizontalAxis = "Horizontal";
    [Tooltip("Name of the axis in 'Project Settings - Input' for moving the player right/left")]
    public string moveVerticalAxis = "Vertical";

    [Space(5)]
    [Header("Rotations")]
    [Tooltip("Name of the axis in 'Project Settings - Input' for rotating the player left/right")]
    public string lookHorizontalAxis = "Mouse X";
    [Tooltip("Name of the axis in 'Project Settings - Input' for rotating the player's head up/down")]
    public string lookVerticalAxis = "Mouse Y";

    [Space(5)]
    [Header("Actions")]
    [Tooltip("Name of the button in 'Project Settings - Input' for the player to interact")]
    public string interactBtn = "Interact";
    [Tooltip("Name of the button in 'Project Settings - Input' for the player to hide")]
    public string hideBtn = "Hide";
    [Tooltip("Name of the button in 'Project Settings - Input' for the player to croach")]
    public string dropBtn = "Drop";
    [Tooltip("Name of the button in 'Project Settings - Input' for the player to shoot")]
    public string shootBtn = "WeaponShoot";


    [Space(5)]
    [Header("Settings")]
    public float mouseSensitivity = 10f;
    public float joystickDeadZone = 5f;
    public float scrollSensitivity;

    private void Awake()
    {
        instace = this;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            input = new InputHandlerMobile(this);
        }
        else
        {
            input = new InputHandlerPC(this);
        }
    }

    private void Start()
    {
        input.SubscribeEvents();
    }

    private void Update()
    {
        input.Update();
    }
}
