using UnityEngine;

[RequireComponent(typeof(Interactor))]
public class Collector : MonoBehaviour
{
    private Collider[] colliders;
    private Rigidbody rb;

    public enum Types
    {
        Crowbar,
        Key,
        Weapon
    }

    public Types type;

    private void Start()
    {
        GetComponent<Interactor>().OnInteraction += Collect;
        colliders = GetComponentsInChildren<Collider>();
        rb = GetComponent<Rigidbody>();
        EventManager.instance.OnNewDay += NewDay;
        enabled = false;
    }

    private void Collect()
    {
        PlayerController.instance.inventory.AddItem(transform);
    }

    public void Collected(bool collected)
    {
        PhysicsActivation(!collected);
        // Add a small rotation for dropping effect
        rb.AddTorque(new Vector3(0.1f, 0f, 0.1f), ForceMode.Impulse);
    }

    public void PhysicsActivation(bool activation)
    {
        foreach (Collider c in colliders)
        {
            c.enabled = activation;
        }

        if (rb != null)
            rb.isKinematic = !activation;
    }

    private void NewDay()
    {
        PhysicsActivation(true);
    }
}
