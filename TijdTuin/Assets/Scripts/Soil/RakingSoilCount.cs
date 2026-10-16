using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// <c>SoilScript</c> handles the behavior of soil in the game.
/// </summary>
public class RakingSoilCount : MonoBehaviour
{
    [Header("Gameobject it should change to when the needed hits are met")]
    [SerializeField] private GameObject _plantableState;
    [SerializeField] private AudioSource _audioSource;
    private GameObject _soilToRemove;

    private const int _requiredHits = 6;
    private int _timesHit = 0;
    
    private ParticleSystem _soilParticle;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _soilToRemove = gameObject;
        _soilParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        HitCounter();
    }

    private void HitCounter()
    {
        if (_timesHit < _requiredHits) return;
        _soilToRemove.gameObject.SetActive(false);
        _plantableState.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rake"))
        {
            _timesHit++;
            _soilParticle.Play();
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }
        else
        {
            _soilParticle.Stop();
            _audioSource.Stop();
        }
    }
}