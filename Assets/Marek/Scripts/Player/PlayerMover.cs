using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMover : MonoBehaviour
{
    public float speed = 3f;
    [Tooltip("When the player walks at his full speed, he can be heard as far as this distance")]
    public float noiseMaxDistance = 8f;
    [HideInInspector]
    public float currentSpeed;
    [Tooltip("Free fall acceleration is applied when the player is further from the ground than this distance")]
    public float groundDistanceCheck = 0.1f;
    public LayerMask groundLayer;
    [Tooltip("We could think of this as feet position when standing still")]
    public Transform groundCheck;

    private CharacterController characterController;
    private float gravityForce;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        gravityForce = Physics.gravity.y;
    }

    private void Update()
    {
        ApplyInputMovement();
        ApplyGravity();
    }

    private void ApplyInputMovement()
    {
        float x = InputManager.input.movementClampedAxis.x;
        float z = InputManager.input.movementClampedAxis.y;
        Vector3 movement = transform.right * x + transform.forward * z;

        currentSpeed = movement.magnitude * speed;
        characterController.Move(movement * speed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        bool isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundDistanceCheck, groundLayer);

        Vector3 gravity = Vector3.zero;
        gravityForce = isGrounded ? Physics.gravity.y : gravityForce + Physics.gravity.y * Time.deltaTime;

        gravity.y = gravityForce;
        characterController.Move(gravity * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, noiseMaxDistance);
    }
}
