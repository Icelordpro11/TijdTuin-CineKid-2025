using System;
using UnityEngine;

/// <summary>
/// <c>Nameplate</c> is a class that manages the nameplate of a plant.
/// </summary>
public class Nameplate : MonoBehaviour
{
    /// <summary>
    /// <c>SetMaterial</c> changes the material of the nameplate based on the provided SeedSO data.
    /// </summary>
    /// <param name="seedData">the data of the specified plant</param>
    public void SetMaterial(SeedSO seedData)
    {
        //Changes the material of the nameplate to the right material.
        gameObject.GetComponent<MeshRenderer>().material = seedData.nameplateMat;
    }
}
