using UnityEngine;

/// <summary>
/// <c>ZonesManager</c> is a class that manages the soil zones in the game.
/// </summary>
public class ZonesManager : MonoBehaviour
{
  [Header("Automated list of neccesary items.")]
  public SoilZone[] zones;
  //A index for the zone list. Do not touch.
  [SerializeField] public int amountOfZonesFilled;
  private SeedingSoil _seedingSoil;
  private WateringSoil _wateringSoil;
  public bool allZonesFilled;

  private void Start()
  {
    _seedingSoil = GetComponent<SeedingSoil>();
    _wateringSoil = GetComponent<WateringSoil>();
    
    //automatically sets up the zones and nameplates, in order for devs and designers to have to do less setup.
    for (var i = 0; i < zones.Length; i++)
    {
        zones[i].zone = transform.GetChild(i);
        zones[i].nameplate =  zones[i].zone.GetComponentInChildren<Nameplate>();
        zones[i].SeedSpriteRenderer = zones[i].zone.GetComponentInChildren<SpriteRenderer>();
    }
  }
  
  /// <summary>
  /// Method <c>UpdatePlantState</c> updates the plant state if the object is enabled or not so it can grow.
  /// </summary>
  /// <param name="state">a boolean to check if the plants should be enabled or not.</param> 
  public void UpdatePlantState(bool state)
  {
      foreach (var soilZone in zones)
      {
        if (state)
        {
          soilZone.SetPlantVisual();
        }
        else
        {
          soilZone.SetSpriteVisual();
        }
      }
  }
  
  /// <summary>
  /// Method <c>EnablePlantMesh</c> enables or disables the plant mesh renderers, only if the soil is fully watered.
  /// </summary>
  /// <param name="state">a boolean to check if the plant meshes should be enabled or not.</param> 
  public void EnablePlantMesh(bool state)
  {
      if (_wateringSoil.fullyWatered == false) return;
      foreach (var soilZone in zones)
      {
          if (state)
          {
              soilZone.EnablePlantMesh();
          }
          else
          {
              soilZone.DisablePlantMesh();
          }
      }
  }
  
  //if the collider of the particle system gets hit by a collider that has the tag "Packets,
  //it will check if the index is less or equal to 2, if it isnt, it will add 1 point to the current index."
  private void OnParticleCollision(GameObject other)
  {
      if (!other.gameObject.CompareTag("SeedParticle") || !_seedingSoil.fullySeeded) return;
      
      if (amountOfZonesFilled <= zones.Length)
      {
          allZonesFilled = false;
      }
      else if (amountOfZonesFilled >= zones.Length)
      {
          allZonesFilled = true;
      }
  }
  
}

