using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Interactor))]
public class Opener : MonoBehaviour
{
    private Animator animator;
    private new AudioSource audio;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        GetComponent<Interactor>().OnInteraction += ChangeOpenState;
        EventManager.instance.OnNewDay += NewDay;
        
        enabled = false;
    }

    private void ChangeOpenState()
    {
        SetOpenState(!animator.GetBool("open"));
    }

    public void SetOpenState(bool open)
    {
        if (open == animator.GetBool("open"))
            return;

        animator.SetBool("open", open);
        if (audio != null)
            audio.Play();
    }

    private void NewDay()
    {
        animator.SetBool("open", false);
    }
}
