using UnityEngine;

public class InputHandlerPC : InputHandler
{
    public InputHandlerPC(InputManager im) : base(im)
    {   
    }

    public override void Update()
    {
        UpdateMovementInput();
        UpdateRotationInput();
        CallActionInputTriggers();
    }

    private void UpdateMovementInput()
    {
        Vector2 movement;
        movement.x = Input.GetAxis(mgr.moveHorizontalAxis);
        movement.y = Input.GetAxis(mgr.moveVerticalAxis);
        movementAxis = movement;
        movementClampedAxis = movement.magnitude > 1f ? movement.normalized : movementAxis;
    }

    private void UpdateRotationInput()
    {
        Vector2 look;
        look.x = Input.GetAxis(mgr.lookHorizontalAxis) * mgr.mouseSensitivity;
        look.y = Input.GetAxis(mgr.lookVerticalAxis) * mgr.mouseSensitivity;
        lookAxis = look;
    }

    private void CallActionInputTriggers()
    {
        if (Input.GetButtonDown(mgr.dropBtn))
            EventManager.instance.TriggerInputDrop();

        if (Input.GetButtonDown(mgr.hideBtn))
            EventManager.instance.TriggerInputHide();

        if (Input.GetButtonDown(mgr.interactBtn))
            EventManager.instance.TriggerInputInteract();

        if (Input.GetButtonDown(mgr.shootBtn))
            EventManager.instance.TriggerInputShoot();

        if (Input.GetKeyDown(KeyCode.Escape))
            EventManager.instance.TriggerInputESC();
    }

    public override void SubscribeEvents()
    {
        return;
    }
}
