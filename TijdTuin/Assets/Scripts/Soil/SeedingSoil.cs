using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary>
/// <c>SeedingSoil</c> is a class that checks how much seed is on a specific soil patch.
/// </summary>
public class SeedingSoil : MonoBehaviour
{
    [Header("Seed Meter")]
    [SerializeField] private int _seedMeter;
    [SerializeField] private int _seedRequired;
    
    [Header("Events")]
    [SerializeField] private float _timer;

    private float _plantTime = 2f;
    public bool fullySeeded;

    private void Update()
    {   
        _timer += Time.deltaTime;
    }
    
    private void OnParticleCollision(GameObject other)
    {
        if (!other.gameObject.CompareTag("SeedParticle")) return;

        if (_timer <= _plantTime) return;
        _seedMeter++; 
        _timer = 0; 
        
        if (_seedMeter <= _seedRequired) return; 
        _seedMeter = 0; 
        fullySeeded = true;
    }
}