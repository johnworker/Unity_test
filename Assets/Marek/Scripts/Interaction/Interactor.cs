using System;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public Action OnInteraction;

    private void Start()
    {
        enabled = false;
    }

    public void Interact()
    {
        OnInteraction?.Invoke();
    }
}
