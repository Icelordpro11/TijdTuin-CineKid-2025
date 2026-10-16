using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class SeedPacketRespawner : MonoBehaviour
{
    [Header("Seed Packet Settings")]
    [SerializeField] private GameObject _packetPrefab;
    [SerializeField] private Transform _parentSpawnPoint;
    [SerializeField] private float _respawnDelay;
    [SerializeField] private Vector3 _rotationOffset = new(0, 65f, 0);

    private Transform _spawnPoint;
    private GameObject _spawnedPrefab;

    private void Start()
    {
        _spawnPoint = transform;
        SpawnPacket();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject != _spawnedPrefab) return;

        if (other.gameObject.TryGetComponent(out SeedDespawnTimer despawner))
        {
            despawner.StartTimer();
            StartCoroutine(RespawnPacketAfterDelay());
        }
    }

    private IEnumerator RespawnPacketAfterDelay()
    {
        yield return new WaitForSeconds(_respawnDelay);
        SpawnPacket();
    }

    private GameObject SpawnPacket()
    {
        GameObject packet = Instantiate(_packetPrefab, _spawnPoint.position, Quaternion.identity, _parentSpawnPoint);
        packet.transform.localRotation = Quaternion.Euler(_rotationOffset);
        _spawnedPrefab = packet;
        return packet;
    }
}