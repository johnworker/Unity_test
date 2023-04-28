using System.Collections;
using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    [Tooltip("What gameobject to rotate when player wants to look up/down")]
    public Transform head;
    [Tooltip("How many degrees can player rotate its head up")]
    public float maxRotationX = 90f;
    [Tooltip("How many degrees can player rotate its head down")]
    public float minRotationX = -90f;

    private float xRotation = 0f;

    private void Update()
    {
        xRotation -= InputManager.input.lookAxis.y;
        xRotation = Mathf.Clamp(xRotation, minRotationX, maxRotationX);

        head.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * InputManager.input.lookAxis.x);
    }

    public IEnumerator RotateTowards(Vector3 position)
    {
        enabled = false;
        while (enabled == false)
        {
            Vector3 direction = (position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 4f);
            yield return new WaitForFixedUpdate();
        }
    }
}
