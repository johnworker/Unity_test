using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandlerMobile : InputHandler
{
    private int moveID;
    private int rotateID;

    private Touch moveTouch;
    private Touch rotateTouch;

    public InputHandlerMobile(InputManager im) : base(im)
    {
        moveID = 0;
        rotateID = 0;
    }

    public override void SubscribeEvents()
    {
        EventManager.instance.OnJoystickMoveTouch += SetMoveTouch;
        EventManager.instance.OnJoystickRotateTouch += SetRotateTouchID;
    }

    public override void Update()
    {
        UpdateMovement();
        UpdateRotation();
    }

    private void SetMoveTouch(PointerEventData data)
    {
        moveID = data.pointerId;
        
        foreach(Touch t in Input.touches)
        {
            if (t.fingerId == moveID)
            {
                moveTouch = t;
                return;
            }
        }
    }

    private void SetRotateTouchID(PointerEventData data)
    {
        rotateID = data.pointerId;

        foreach (Touch t in Input.touches)
        {
            if (t.fingerId == rotateID)
            {
                rotateTouch = t;
                return;
            }
        }
    }

    private bool TouchIDExists(int ID)
    {
        foreach(Touch touch in Input.touches)
        {
            if (touch.fingerId == ID)
                return true;
        }

        return false;
    }

    private void UpdateMovement()
    {
        if (!TouchIDExists(moveID))
            return;

        Vector2 movement;
        movement.x = moveTouch.position.x - JoyistickMove.instance.transform.position.x;
        movement.x = movement.x <= InputManager.instace.joystickDeadZone ? 0.0f : movement.x / JoyistickMove.instance.maxThumb;
        movement.y = moveTouch.position.y - JoyistickMove.instance.transform.position.y;
        movement.y = movement.y <= InputManager.instace.joystickDeadZone ? 0.0f : movement.y / JoyistickMove.instance.maxThumb;
        movementAxis = movement;
        movementClampedAxis = movement.magnitude > 1f ? movement.normalized : movementAxis;
    }

    private void UpdateRotation()
    {
        if (!TouchIDExists(rotateID))
            return;

        lookAxis = rotateTouch.deltaPosition * InputManager.instace.scrollSensitivity;
    }
}
