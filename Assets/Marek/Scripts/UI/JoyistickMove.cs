using UnityEngine;
using UnityEngine.EventSystems;

public class JoyistickMove : MonoBehaviour, IPointerDownHandler
{
    public static JoyistickMove instance;

    public RectTransform thumb;
    public float maxThumb = 45f;

    private InputHandler input;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        input = InputManager.input;
    }

    public void Update()
    {
        Vector3 position = new Vector3();
        position.x = input.movementClampedAxis.x * maxThumb;
        position.y = input.movementClampedAxis.y * maxThumb;

        thumb.localPosition = position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        EventManager.instance.TriggerJoystickMoveTouch(eventData);
    }
}
