using UnityEngine;

/// <summary>
/// <c>SeedingSystem</c> is responsible for managing the seeding process when particles collide with soil.
/// </summary>
public class SeedingSystem : MonoBehaviour
{

    [Header("Seed Meter")]
    [SerializeField] private int _seedingMeter;
    [SerializeField] private int _seedRequired; 
    [SerializeField] private float _timer;
    
    private bool _soilIsFilled;
    
    private const float PlantTime = 2f;
    private void Update()
    {
        if (_soilIsFilled) return;
        _soilIsFilled = false;
        
        
        _timer += Time.deltaTime;
    }
    private void OnParticleCollision(GameObject other)
    {
        if (!other.gameObject.CompareTag("PlantSoil")) return;

        if (!(_timer >= PlantTime)) return;
        _seedingMeter++;
        _timer = 0;

        if (_seedingMeter < _seedRequired) return;
        _seedingMeter = 0;
        _soilIsFilled = true;
    }
}