using System;
using UnityEngine;

/// <summary>
/// <c>SoilZone</c> is a class that holds information about a specific soil zone in the game.
/// </summary>
[Serializable]
public class SoilZone
{ 
    public Nameplate nameplate; 
    [NonSerialized] public SeedSO seedSO;
    public Transform zone;
    public SpriteRenderer SeedSpriteRenderer; 
    public GameObject spawnedPlant;
    public SkinnedMeshRenderer[] skinnedMeshRender;
     

    public Transform getZone => zone;
    //Calls the function that changes the material of the nameplates to their respective type.
    public void NameplateUpdater(SeedSO seedSO)
    {
        nameplate.SetMaterial(seedSO);
    }
    //Calls the function that changes the sprite of the seed slot to the right sprite.
    public void SeedUpdater(SeedSO seedSO)
    {
        SeedSpriteRenderer.sprite = seedSO.seedSprite;
    }
    //Sets the visual of the plant to be the 3D model instead of the sprite.
    public void SetPlantVisual()
    {
        if (spawnedPlant == null) return;
        spawnedPlant?.SetActive(true);
        skinnedMeshRender = spawnedPlant.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var renderer in skinnedMeshRender)
        {
            renderer.enabled = false;
        }
        SeedSpriteRenderer.enabled = false;
    }
    //Enables the plant mesh renderers, so the plant can be seen.
    public void EnablePlantMesh()
    {
        if (spawnedPlant == null) return;
        skinnedMeshRender = spawnedPlant.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var renderer in skinnedMeshRender)
        {
            renderer.enabled = true;
        }
    }
    //Disables the plant mesh renderers, so the plant cannot be seen.
    public void DisablePlantMesh()
    {
        if (spawnedPlant == null) return;
        skinnedMeshRender = spawnedPlant.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var renderer in skinnedMeshRender)
        {
            renderer.enabled = false;
        }
    }
    //Sets the visual of the plant to be the sprite instead of the 3D model.
    public void SetSpriteVisual()
    {
        if (spawnedPlant == null) return;
        spawnedPlant.SetActive(false);
        SeedSpriteRenderer.enabled = true;
    }
    
}
