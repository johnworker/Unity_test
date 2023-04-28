using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    // Input actions
    public Action OnInputInteract;
    public Action OnInputHide;
    public Action OnInputDrop;
    public Action OnInputESC;
    public Action OnInputShoot;
    public delegate void JoystickTouch(PointerEventData data);
    public event JoystickTouch OnJoystickMoveTouch;
    public event JoystickTouch OnJoystickRotateTouch;

    // Gameplay actions
    public delegate void NoiseAppeal(Vector3 position, float distance);
    public event NoiseAppeal OnNoiseAppeal;
    public Action OnPlayerDied;
    public Action OnPlayerAttacked;
    public Action OnNewDay;

    //Setting actions
    public Action OnLanguageChange;

    private void Awake()
    {
        instance = this;
        enabled = false;
    }

    #region InputTriggers

    public void TriggerInputInteract()
    {
        OnInputInteract?.Invoke();
    }

    public void TriggerInputHide()
    {
        OnInputHide?.Invoke();
    }

    public void TriggerInputDrop()
    {
        OnInputDrop?.Invoke();
    }

    public void TriggerInputESC()
    {
        OnInputESC?.Invoke();
    }

    public void TriggerInputShoot()
    {
        OnInputShoot?.Invoke();
    }

    public void TriggerJoystickMoveTouch(PointerEventData data)
    {
        OnJoystickMoveTouch?.Invoke(data);
    }

    public void TriggerJoystickRotateTouch(PointerEventData data)
    {
        OnJoystickRotateTouch?.Invoke(data);
    }

    #endregion

    #region GameplayTriggers

    public void TriggerOnNoiseAppeal(Vector3 position, float distance)
    {
        OnNoiseAppeal?.Invoke(position, distance);
    }

    public void TriggerPlayerDied()
    {
        OnPlayerDied?.Invoke();
    }

    public void TriggerNewDay()
    {
        OnNewDay?.Invoke();
    }

    public void TriggerPlayerAttacked()
    {
        OnPlayerAttacked?.Invoke();
    }

    #endregion

    #region SettingTriggers

    public void TriggerLanguageChange()
    {
        OnLanguageChange?.Invoke();
    }

    #endregion
}
