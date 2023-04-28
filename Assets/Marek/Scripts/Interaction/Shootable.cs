using System;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    public Action OnShot;

    private void Start()
    {
        enabled = false;
    }

    public void Shot()
    {
        OnShot?.Invoke();
    }
}
