using UnityEngine;

public class HUDControls : MonoBehaviour
{
    public GameObject interactionPoint;
    public GameObject interactionHint;
    public GameObject hideHint;
    public GameObject shootHint;
    public GameObject dropHint;

    private PlayerController player;

    void Start()
    {
        player = PlayerController.instance;
    }

    void Update()
    {
        interactionPoint.SetActive(player.interactorHit != null);
        interactionHint.SetActive(player.interactorHit != null);
        hideHint.SetActive(player.hidingArea != null);
        shootHint.SetActive(player.inventory.weapon != null && player.hidingArea != null);
        dropHint.SetActive(player.inventory.item != null && !player.isHiding);
    }
}
