using UnityEngine;

[RequireComponent(typeof(Interactor))]
[RequireComponent(typeof(AudioSource))]
public class MasterDoor : MonoBehaviour
{
    public Transform barricades;
    public Animator animOpen;
    public AudioClip barricadeDamageSound;
    public AudioClip keyOpenSound;
    private bool isLocked;

    private HUDManager hud;
    private new AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        GetComponent<Interactor>().OnInteraction += Interact;
        hud = HUDManager.instance;
        isLocked = true;
    }

    private void Interact()
    {
        Collector item = PlayerController.instance.inventory.collector;

        if (barricades.gameObject.activeSelf)
        {
            if (item != null && item.type == Collector.Types.Crowbar)
            {
                barricades.gameObject.SetActive(false);
                audio.clip = barricadeDamageSound;
                audio.Play();
            }
            else
            {
                hud.ShowGameplayHint("010038");
            }
        }
        else if (isLocked)
        {
            if (item != null && item.type == Collector.Types.Key)
            {
                isLocked = false;
                animOpen.SetBool("open", true);
                audio.clip = keyOpenSound;
                audio.Play();
            }
            else
            {
                hud.ShowGameplayHint("010036");
            }
        }
    }
}
