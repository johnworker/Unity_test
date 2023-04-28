using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class MovableNoiseItem : MonoBehaviour
{
    public float noiseDistance = 13f;

    private Rigidbody rb;
    private new AudioSource audio;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        EventManager.instance.OnNewDay += NewDay;
        startPosition = transform.position;
        startRotation = transform.rotation;
        enabled = false;
    }

    public void PlayerHit(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
        if (!audio.isPlaying)
            audio.Play();
        EventManager.instance.TriggerOnNoiseAppeal(transform.position, noiseDistance);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<ZombieController>() == null)
            return;

        if (!audio.isPlaying)
            audio.Play();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, noiseDistance);
    }

    private void NewDay()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}
