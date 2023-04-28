using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(NavMeshObstacle))]
[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(PlayerRotator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    #region Variables

    public static PlayerController instance;
    public PlayerInvetory inventory;

    // Child references
    public Camera eyesCamera;
    public Collider meleeZone;
    public Transform itemHolder;

    // Layer references
    public LayerMask interactionLayer;
    public LayerMask hidingAreaLayer;
    public LayerMask movableNoiseItems;

    // Interactions checks
    [Tooltip("How far (maximum) the player can be from the object he interacts with")]
    public float maxInteractionDistance = 1f;
    public Interactor interactorHit { get; protected set; }
    public Transform hidingArea { get; protected set; }

    // Sounds
    public AudioClip[] footsteps;
    public AudioClip dying;

    // State references
    [HideInInspector]
    public bool isHiding;
    [HideInInspector]
    public float noiseDistance; // how far the player's noise can be heard
    [Tooltip("How long the dying process should take")]
    private Transform carriedItem;
    private Transform carriedOriginalParent;
    private bool isCarrying { get => carriedItem != null; }
    private Camera hidingCamera;
    private Vector3 unhidePosition;
    private Vector3 startPosition;
    private Quaternion startRotation;

    // Required components
    private CharacterController characterController;
    private NavMeshObstacle navObstacle;
    private PlayerMover mover;
    private PlayerRotator rotator;
    private new AudioSource audio;

    #endregion

    #region Initialization

    private void Awake()
    {
        inventory = new PlayerInvetory(this);
        instance = this;
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        navObstacle = GetComponent<NavMeshObstacle>();
        mover = GetComponent<PlayerMover>();
        rotator = GetComponent<PlayerRotator>();
        audio = GetComponent<AudioSource>();

        SubscribeInputEvents();
        EventManager.instance.OnNewDay += NewDay;

        isHiding = false;
        carriedItem = null;
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    private void NewDay()
    {
        SubscribeInputEvents();
        transform.position = startPosition;
        transform.rotation = startRotation;
        mover.enabled = true;
        rotator.enabled = true;
        audio.Stop();
        inventory.Reset();
    }

    private void SubscribeInputEvents()
    {
        EventManager.instance.OnInputInteract += OnInputInteract;
        EventManager.instance.OnInputHide += OnInputHide;
        EventManager.instance.OnInputDrop += OnInputDrop;
        EventManager.instance.OnInputShoot += Shoot;
    }

    private void UnsubscribeInputEvents()
    {
        EventManager.instance.OnInputInteract -= OnInputInteract;
        EventManager.instance.OnInputHide -= OnInputHide;
        EventManager.instance.OnInputDrop -= OnInputDrop;
        EventManager.instance.OnInputShoot -= Shoot;
    }

    #endregion

    #region Updates

    private void FixedUpdate()
    {
        GetPossibleInteraction();
        UpdateSound();
    }

    private void UpdateSound()
    {
        noiseDistance = isHiding ? 0f : (mover.currentSpeed / mover.speed) * mover.noiseMaxDistance;

        if (noiseDistance < mover.noiseMaxDistance / 2f || audio.isPlaying)
            return;

        int rnd = Random.Range(0, footsteps.Length);
        audio.clip = footsteps[rnd];
        audio.Play();
    }

    public Vector3 GetCurrentVelocity()
    {
        return characterController.velocity;
    }

    #endregion

    #region Inputs

    private void OnInputInteract()
    {
        if (interactorHit == null)
            return;

        interactorHit.Interact();
    }

    private void OnInputHide()
    {
        if (isHiding)
        {
            HideStateSwap();
        }
        else if (hidingArea != null)
        {
            hidingCamera = hidingArea.GetComponentInChildren<Camera>();
            unhidePosition = transform.position;
            HideStateSwap();
        }
    }

    private void OnInputDrop()
    {
        if (!isHiding)
            inventory.DropItem();
    }

    private void OnInputShoot()
    {
        Shoot();
    }

    #endregion

    #region Actions

    private void HideStateSwap()
    {
        isHiding = !isHiding;

        eyesCamera.enabled = !isHiding;
        if (inventory.item != null)
            inventory.item.gameObject.SetActive(!isHiding);
        hidingCamera.enabled = isHiding;
        characterController.enabled = !isHiding;
        navObstacle.enabled = !isHiding;
        mover.enabled = !isHiding;
        rotator.enabled = !isHiding;
        meleeZone.enabled = !isHiding;

        transform.position = isHiding ? hidingCamera.transform.position : unhidePosition;
    }

    private void Shoot()
    {
        inventory.weapon?.Shoot();
    }

    public void Attacked(Transform attacker)
    {
        mover.enabled = false;
        rotator.enabled = false;
        UnsubscribeInputEvents();
        audio.Stop();
        StartCoroutine(rotator.RotateTowards(attacker.position));
        EventManager.instance.TriggerPlayerAttacked();
        StartCoroutine(StartDying());
    }

    private IEnumerator StartDying()
    {
        audio.clip = dying;
        audio.Play();
        yield return new WaitForSeconds(LevelManager.instance.timeDeadToDay);
        EventManager.instance.TriggerPlayerDied();
    }

    #endregion

    #region Physics

    private void GetPossibleInteraction()
    {
        Ray ray = eyesCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxInteractionDistance, ~(1 << gameObject.layer)))
        {
            if (((1 << hit.transform.gameObject.layer) & interactionLayer) == 0)
            {
                interactorHit = null;
            }
            else
            {
                interactorHit = hit.transform.GetComponent<Interactor>();
            }
        }
        else
        {
            interactorHit = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & hidingAreaLayer) != 0)
            hidingArea = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == hidingArea)
            hidingArea = null;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody otherRB = hit.collider.attachedRigidbody;

        if (otherRB == null || otherRB.isKinematic || ((1 << hit.collider.gameObject.layer) & movableNoiseItems) == 0)
            return;

        MovableNoiseItem noiseItem = hit.transform.GetComponent<MovableNoiseItem>();
        noiseItem?.PlayerHit(new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z));
    }

    #endregion
}
