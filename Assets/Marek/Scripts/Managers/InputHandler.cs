using UnityEngine;

public abstract class InputHandler
{
    public Vector2 movementAxis { get; protected set; }
    public Vector2 movementClampedAxis { get; protected set; } // Clamp magnitude to maximum of 1
    public Vector2 lookAxis { get; protected set; }

    protected InputManager mgr;

    public InputHandler(InputManager im)
    {
        mgr = im;
    }

    public abstract void Update();
    public abstract void SubscribeEvents();
}
