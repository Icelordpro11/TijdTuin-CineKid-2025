using UnityEngine;

[CreateAssetMenu(menuName = "Farming/Seed Data")]
[Tooltip("ScriptableObject that holds data for a seed type.")]
public class SeedSO : ScriptableObject
{
    public string seedName; // the name of the seed like a berry or something
    public GameObject plantPrefab; // the Seed prefab that is related to the seed
    public Material nameplateMat; // the nameplate material that shows what type of plant there is on a nameplate
    public Sprite seedSprite; // the  seed sprite, that shows after planting something in the soil 
}
