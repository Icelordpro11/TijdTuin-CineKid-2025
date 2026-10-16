using System.Collections;
using UnityEngine;

/// <summary>
/// <c>SeedDespawnTimer</c> is a Unity MonoBehaviour script that manages the despawning of a seed object after a specified time when it is not in contact with a controller.
/// </summary>
public class SeedDespawnTimer : MonoBehaviour
{
    [Header("manages how long a object needs to despawn.")] 
    [SerializeField] private float _despawnTimer;
   
    [Header("creates a Coroutine for the code to use (See DespawnAfterTime)")]
    private Coroutine _despawnCoroutine;

    private bool _isInSpawnpoint = true;

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Controller")) return;
        if (_despawnCoroutine == null) return;

        StopCoroutine(_despawnCoroutine);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Controller")) return;
        if (_isInSpawnpoint) return;

        _despawnCoroutine = StartCoroutine(DespawnAfterTime());
    }

    public void StartTimer()
    {
        _isInSpawnpoint = false;
        _despawnCoroutine = StartCoroutine(DespawnAfterTime());
    }

    private IEnumerator DespawnAfterTime()
    {
        yield return new WaitForSeconds(_despawnTimer);
        Destroy(gameObject);
    }
}
