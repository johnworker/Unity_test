using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class ScaryFrame : MonoBehaviour
{
    public float impulse = 10f;
    public float torque = 5f;

    private Rigidbody rb;
    private Collider trigger;
    private new AudioSource audio;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private void Start()
    {
        trigger = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();

        rb.isKinematic = true;

        startPosition = transform.position;
        startRotation = transform.rotation;

        EventManager.instance.OnNewDay += NewDay;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        rb.isKinematic = false;
        rb.AddForce(-transform.right * impulse, ForceMode.Impulse);
        rb.AddTorque(new Vector3(torque, torque, torque), ForceMode.Impulse);
        audio.Play();
        trigger.enabled = false;
    }

    private void NewDay()
    {
        trigger.enabled = true;
        rb.isKinematic = true;
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}
