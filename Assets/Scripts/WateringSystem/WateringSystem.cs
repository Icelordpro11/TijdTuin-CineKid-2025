using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class WateringSystem : MonoBehaviour
{
    [Header("Water Meter")]
    [SerializeField] private int _waterMeter;
    [SerializeField] private int _waterRequired;
    [SerializeField] private UnityEvent<bool> _isFullyWatered;
    [Header("SoilCollector")]

    [SerializeField] private float timer;
    private void Update()
    {
        timer += Time.deltaTime;
    }
    private void OnParticleCollision(GameObject other)
    {
        if (!other.gameObject.CompareTag("PlantSoil")) return;
        
        if (timer <= 4) return;
        _waterMeter++;
        timer = 0;

        if (_waterMeter < _waterRequired) return;
        _waterMeter = 0;
        _isFullyWatered?.Invoke(true);
        gameObject.SetActive(false);
    }
}