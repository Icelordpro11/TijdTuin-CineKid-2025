using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Invokes UnityEvents when an object with a specific tag enters or exits the trigger collider.
/// </summary>
public class TriggerEventHandler : MonoBehaviour
{
    [Header("Target Settings")] 
    [SerializeField] private string _targetTag = "Player";

    [Header("Trigger Events")]
    [SerializeField] private UnityEvent _onCameraEnter;

    [SerializeField] private UnityEvent _onCameraExit;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(_targetTag)) return;
        _onCameraEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(_targetTag)) return;
        _onCameraExit?.Invoke();
    }
}
