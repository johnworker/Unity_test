using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EventManager))]
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public Spawner[] spawners;
    public int day { get; protected set; }
    public float timeDeadToDay = 2.0f;

    public void Awake()
    {
        day = 1;
        instance = this;        
    }

    private void Start()
    {
        Spawn();
        EventManager.instance.OnPlayerDied += StartNewDay;
        EventManager.instance.OnInputESC += ESC;
        Cursor.lockState = CursorLockMode.Locked;
        enabled = false;
    }

    private void StartNewDay()
    {
        ++day;
        Spawn();
        if (day > 1)
            EventManager.instance.TriggerNewDay();
    }

    private void Spawn()
    {
        foreach (Spawner s in spawners)
        {
            s.Spawn();
        }
    }

    private void ESC()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
