using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(Shootable))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class ZombieController : MonoBehaviour
{
    #region Variables

    public Transform[] startpoints;
    public Transform[] waypoints;
    public float fieldOfView = 110f;
    public float visibilityDistance = 100f;
    public float attackDistance = 1.7f;
    public LayerMask visibleLayers;
    public LayerMask autoOpen;
    public ZombieSounds sounds;

    private NavMeshAgent navigation;
    private Animator animator;
    private new AudioSource audio;

    private enum States
    {
        Attacking,
        Chasing,
        Checking,
        Patroling
    }
    private States state;
    private PlayerController player;
    private float distanceToPlayer;

    #endregion

    #region Initialization

    private void Start()
    {
        player = PlayerController.instance;
        navigation = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        GetComponent<Shootable>().OnShot += Die;
        EventManager.instance.OnNoiseAppeal += OnNoiseTrapTriggered;
        EventManager.instance.OnNewDay += NewDay;
        NewDay();
    }

    private void NewDay()
    {
        int rnd = Random.Range(0, startpoints.Length);
        navigation.enabled = false;
        transform.position = startpoints[rnd].position;
        navigation.enabled = true;
        state = States.Patroling;
        audio.Play();
        navigation.isStopped = false;
        enabled = true;
    }

    #endregion

    #region Updates

    private void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        UpdateState();
        UpdateAnimator();
        UpdateSounds();
    }

    private void UpdateAnimator()
    {
        if (navigation.isStopped)
        {
            animator.SetBool("Walking", false);
        }
        else
        {
            animator.SetBool("Walking", navigation.velocity.magnitude > 0.15 * navigation.speed);
        }
    }

    private bool DestinationToBeReached()
    {
        if (navigation.pathPending)
            return false;

        return navigation.remainingDistance <= navigation.stoppingDistance + 2.0f || navigation.pathStatus == NavMeshPathStatus.PathInvalid || navigation.velocity.sqrMagnitude == 0f;
    }

    private void UpdateState()
    {
        if (state == States.Attacking && distanceToPlayer <= attackDistance)
            return;

        if (distanceToPlayer <= attackDistance && PlayerIsSeen(false))
        {
            Attack();
        }
        else if (PlayerIsSeen() && !player.isHiding)
        {
            Chase();
        }
        else if (player.isHiding && state == States.Chasing)
        {
            Patrol();
        }
        else if (PlayerIsHeard())
        {
            Check(player.transform.position);
        }
        else if (state != States.Chasing && state != States.Checking)
        {
            Patrol();
        }
        else if (DestinationToBeReached())
        {
            state = States.Patroling;
            Patrol();
        }
    }

    private void UpdateSounds()
    {
        if (state == States.Attacking || audio.isPlaying)
            return;

        int rnd = Random.Range(0, sounds.basic.Length);
        audio.clip = sounds.basic[rnd];
        audio.Play();
    }

    #endregion

    #region StateActions

    private void Attack()
    {
        state = States.Attacking;
        animator.SetTrigger("Attack");
        audio.clip = sounds.attack;
        audio.Play();
        navigation.isStopped = true;
        audio.clip = sounds.attack;
        audio.Play();
        enabled = false;
        player.Attacked(transform);
        StartCoroutine(RotateTowardsPlayer());
    }

    public IEnumerator RotateTowardsPlayer()
    {
        enabled = false;
        while (enabled == false)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            yield return new WaitForFixedUpdate();
        }
    }

    private void Chase()
    {
        navigation.SetDestination(player.transform.position);
        state = States.Chasing;
    }

    private void Check(Vector3 position)
    {
        if (state == States.Chasing)
            return;

        navigation.SetDestination(position);
        state = States.Checking;
    }

    private void Patrol()
    {
        if (!DestinationToBeReached())
            return;

        int rnd = Random.Range(0, waypoints.Length);
        navigation.SetDestination(waypoints[rnd].position);
    }

    #endregion

    #region ActionTriggers

    private bool PlayerIsSeen(bool considerFieldOfView = true)
    {
        if (distanceToPlayer > visibilityDistance || player.isHiding)
            return false;

        float heightOffset = 0.2f; // to make sure we do not hit ground
        Vector3 myPosition = new Vector3(transform.position.x, transform.position.y + heightOffset, transform.position.z);
        Vector3 playerPosition = new Vector3(player.transform.position.x, player.transform.position.y + heightOffset, transform.position.z);

        Ray ray = new Ray(myPosition, player.transform.position - transform.position);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, visibilityDistance, visibleLayers, QueryTriggerInteraction.Ignore))
            return false;

        if (hit.transform != player.transform)
            return false;

        if (!considerFieldOfView)
            return true;

        Vector3 direction = hit.transform.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);

        return angle < fieldOfView * 0.5f;
    }

    private bool PlayerIsHeard()
    {
        return distanceToPlayer < player.noiseDistance;
    }

    private void OnNoiseTrapTriggered(Vector3 noisePosition, float noiseDistance)
    {
        if (state == States.Chasing || Vector3.Distance(noisePosition, transform.position) > noiseDistance)
            return;

        Check(noisePosition);
    }

    private void Die()
    {
        audio.clip = sounds.dying;
        audio.Play();
        animator.SetTrigger("Die");
        navigation.isStopped = true;
        enabled = false;
    }

    #endregion

    #region TriggerCheckes

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & autoOpen) == 0)
            return;

        Opener opener = other.transform.GetComponent<Opener>();
        opener?.SetOpenState(true);
    }

    #endregion
}
