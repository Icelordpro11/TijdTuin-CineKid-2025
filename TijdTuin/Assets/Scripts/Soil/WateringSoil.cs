using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// <c>WateringSoil</c> handles the behavior of soil when it is being watered.
/// </summary>
public class WateringSoil : MonoBehaviour
{
    private int _waterMeter;
    private const int _waterRequired = 2;

    public bool fullyWatered = false;
    
    [Header("the wet Mat to change to when fully watered")]
    [SerializeField] private Material _newMaterial;
    private Renderer _myRenderer;
    
    [Header("Maximum amount of particles needed to change the Mat")]
    [SerializeField] private float _maxHits;
    private float _currentHits;
    private void Start()
    {
        _myRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (fullyWatered)
        {
            _myRenderer.material = _newMaterial;
        }
        
    }

    private void OnParticleCollision(GameObject other)
    {
        if (!other.gameObject.CompareTag("WaterParticle")) return;
        _currentHits++;
        
        if (_currentHits <= _maxHits) return;
        _waterMeter++;
        _currentHits = 0;
        
        if (_waterMeter < _waterRequired) return;
        _waterMeter = 0;
        fullyWatered = true;
    }
}