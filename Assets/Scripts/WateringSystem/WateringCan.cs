using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// <c>WateringCan</c> class handles the` watering can functionality,
/// </summary>
public class WateringCan : MonoBehaviour
{
    [Header("Watering Can Angles")]
    [SerializeField] private float _minAngleZ;
    [SerializeField] private float _maxAngleZ;
    
    [Header("Water particles")]
    [SerializeField] private new GameObject particleSystem;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    { 
        var currentAngleZ = transform.eulerAngles.z;

        if (currentAngleZ <= _minAngleZ && currentAngleZ >= _maxAngleZ)
        {
            particleSystem.SetActive(true);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
            particleSystem.SetActive(false);
        }
        
    }
}