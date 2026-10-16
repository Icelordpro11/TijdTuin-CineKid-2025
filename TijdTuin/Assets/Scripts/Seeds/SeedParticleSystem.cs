using System;
using Meta.WitAi.Attributes;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// <c>SeedParticleSystem</c> is a Unity MonoBehaviour that controls a particle system and audio source based on the orientation of the GameObject.
/// </summary>
public class SeedParticleSystem : MonoBehaviour
{
    [FormerlySerializedAs("threshold")]
    [Header("Seeding")]
    [Range(-1f, 1f), Tooltip("The threshold at which to start spawning particles. 1 = Down, -1 = Up")]
    [SerializeField] private float _threshold;
    
    [FormerlySerializedAs("particleSystem")]
    [Header("References")]
    [SerializeField] private new GameObject _particleSystem;
    private AudioSource _audioSource;


    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        float dot = Vector3.Dot(transform.up, Vector3.down);

        if (dot > _threshold)
        {
            _particleSystem.SetActive(true);
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }
        else
        {
            _particleSystem.SetActive(false);
            _audioSource.Stop();
        }

    }
}