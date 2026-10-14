using UnityEngine;

/// <summary>
/// <c>OpenSeedBag</c> is a Unity MonoBehaviour script that handles the opening animation of a seed bag when a controller enters its trigger area.
/// </summary>
public class OpenSeedBag : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void OnTriggerEnter(Collider other)
    {
        const string animatorBool = "Open";
        // Check if the collider belongs to the controller
        if (other.CompareTag("Controller"))
        {
           _animator.SetBool(animatorBool, true);
        }
        else
        {
            _animator.SetBool(animatorBool, false);
        }
    }
} 