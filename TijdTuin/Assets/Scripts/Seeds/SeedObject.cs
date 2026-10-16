using UnityEngine;

/// <summary>
/// <c>SeedObject</c> is responsible for handling the planting of seeds when they collide with soil zones.
/// </summary>
public class SeedObject : MonoBehaviour
{
    [SerializeField] private SeedSO _seedData;
    private static int _plantCounter = 0;
    private SoilZone[] _zones;
    private int _filledZones;


    /// <summary>
    /// Method <c>PlantSeed</c> instantiates a new plant in the specified soil zone.
    /// </summary>
    /// <param name="target">is the transform of the soil zone where the seed will be planted.</param>
    private void PlantSeed(Transform target)
    {
        //Puts the final position of the plant as the current zones position.
        var finalPosition = target.transform.position;

        //Makes a new plant with the use of the SO, taking the plats prefab from it and getting the position from before.
        GameObject _newPlant = Instantiate(_seedData.plantPrefab, finalPosition, Quaternion.identity, target);
        _newPlant.SetActive(false);
        
        _zones[_filledZones].spawnedPlant = _newPlant;
        _zones[_filledZones].NameplateUpdater(_seedData);
        _zones[_filledZones].SeedSpriteRenderer.sprite  = _seedData.seedSprite;
        
        _plantCounter++;
        _newPlant.name = $"Plant_{_plantCounter}";
        

        Destroy(transform.parent.gameObject);
    }

    private void OnParticleCollision(GameObject other)
    {
        //Checks if the collider that its hitting is that of the gameobject that has the tag "PlantSoil".
        if (!other.gameObject.CompareTag("PlantSoil")) return;
        var zm = other.gameObject.GetComponent<ZonesManager>();
        var ss = other.gameObject.GetComponent<SeedingSoil>();
        _zones = zm.zones;
        _filledZones = zm.amountOfZonesFilled;

        if (!ss.fullySeeded || zm.allZonesFilled) return;
        zm.amountOfZonesFilled++;
        PlantSeed(_zones[_filledZones].getZone);
        OwlDisplacer.instance.SeedCounter++;
        ss.fullySeeded = false;
    }
}