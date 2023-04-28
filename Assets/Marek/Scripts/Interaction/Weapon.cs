using UnityEngine;

[RequireComponent(typeof(Interactor))]
[RequireComponent(typeof(Collector))]
[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    public LayerMask ignoreLayers;
    [Tooltip("Maximum distance the shot can reach")]
    public float maxDistance = 30f;
    public float noiseDistance = 17f;
    public GameObject shotEffect;

    private new AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        shotEffect.SetActive(false);
        enabled = false;
    }

    public void Shoot()
    {
        EventManager.instance.TriggerOnNoiseAppeal(transform.position, noiseDistance);
        shotEffect.SetActive(true);
        audio.Play();

        RaycastHit hit;
        Ray ray = PlayerController.instance.eyesCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (!Physics.Raycast(ray, out hit, maxDistance, ~ignoreLayers, QueryTriggerInteraction.Ignore))
            return;

        Shootable shootable = hit.transform.GetComponent<Shootable>();

        if (shootable)
            shootable.Shot();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, noiseDistance);
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
