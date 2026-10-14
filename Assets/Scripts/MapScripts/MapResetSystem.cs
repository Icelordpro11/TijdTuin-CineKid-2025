using System;
using UnityEngine;

/// <summary>
/// Handles resetting and respawning the map. 
/// </summary>
public class MapResetSystem : MonoBehaviour
{
    public static MapResetSystem Instance { get; private set; }

    [SerializeField] [Range(1f, 60f)] private float _resetTime = 60f;
    [SerializeField] private GameObject _mapPrefab;

    private Vector3 _spawnPosition;
    private GameObject _currentMap;
    private Quaternion _spawnRotation;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //Stores the current map and saves its transform for future respawns
        _currentMap = GetCurrentMap();
        SaveMapTransform(_currentMap?.transform);
    }

    /// <summary>
    /// Ends the current experience by destroying the active map, 
    /// stopping all timers, and scheduling a respawn after a delay.
    /// </summary>
    public void EndExperience()
    {
        AsyncTimerManager.StopAllTimers();
        _currentMap = GetCurrentMap();

        if (_currentMap)
        {
            SaveMapTransform(_currentMap.transform);
            Destroy(_currentMap);
            _currentMap = null;
        }

        AsyncTimerManager.Timer(TimeSpan.FromSeconds(_resetTime), null, SpawnNewMap);
    }

    /// <summary>
    /// Instantiates a new map prefab at the previously saved position and rotation.
    /// </summary>
    public void SpawnNewMap()
    {
        if (!_mapPrefab)
        {
            Debug.LogError("Map Prefab is not assigned.");
            return;
        }

        var map = Instantiate(_mapPrefab, _spawnPosition, _spawnRotation, transform);
        _currentMap = map;
    }
    
    /// <summary>
    /// Returns the first child GameObject under this transform, assumed to be the current map.
    /// </summary>
    private GameObject GetCurrentMap()
    {
        if (transform.childCount > 0) return transform.GetChild(0).gameObject;

        Debug.LogWarning("No child map found under MapLocationPrefab");
        return null;
    }

    /// <summary>
    /// Saves the provided transform’s position and rotation for respawn purposes.
    /// </summary>
    private void SaveMapTransform(Transform target)
    {
        if (!target) return;

        _spawnPosition = target.position;
        _spawnRotation = target.rotation;
    }
}
