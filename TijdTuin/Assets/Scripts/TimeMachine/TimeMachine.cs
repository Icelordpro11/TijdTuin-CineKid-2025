using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

/// <summary>
/// <c>TimeMachine</c> class handles the interaction with the time machine lever,
/// </summary>
public class TimeMachine : MonoBehaviour
{
    [Header("References")]
    private ZonesManager[] _zonesManager;
    private OvergrownStateChanger _assetStateScript;
    private WateringSoil[] _soilPatches;
    
    [Header("Seeds and Plants")]
    public GameObject[] activeSeeds;
    public List<GameObject> plantedPlants = new List<GameObject>();
    
    [Header("Plant Animators and Particle System")]
    [SerializeField] private List<Animator> _plantAnimators = new List<Animator>();
    [SerializeField] private GameObject _sparkParticleObject;
    private ParticleSystem _sparkParticles;

    [Header("State Bools")]
    public bool leverDown = false;
    public bool ambienceChange = false;
    public bool BuboHasEnded = false;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Head"))
        {

            ZonesManager[] allZonesManagers = FindObjectsByType<ZonesManager>(FindObjectsSortMode.None);
            _zonesManager = allZonesManagers;
            _sparkParticles = _sparkParticleObject.GetComponent<ParticleSystem>();
            _assetStateScript = GetComponent<OvergrownStateChanger>();
            _sparkParticles.Play();
            

            if (BuboHasEnded == false)
            {
                OwlDisplacer.instance.EndBubo();
                BuboHasEnded = true;
            }
            

            plantedPlants.Clear();
            _plantAnimators.Clear();
            
            // Update plant states in all zones so the models are getting enabled
            foreach (var zone in _zonesManager) 
            {
                zone.UpdatePlantState(true);
            }

            GameObject[] plants = GameObject.FindGameObjectsWithTag("Plant");
            WateringSoil[] allSoilPatches = FindObjectsByType<WateringSoil>(FindObjectsSortMode.None);
            _soilPatches = allSoilPatches;
            
            for (int x = 0; x < activeSeeds.Length; x++)
            {
                activeSeeds[x] = GameObject.FindWithTag("Seed");
            }
            _assetStateScript.UpdateOvergrownState();
            foreach (GameObject plant in plants)
            {
                plant.SetActive(true);
                Animator plantAnimator = plant.GetComponent<Animator>();
                if (plantAnimator != null)
                {
                    bool isOnWateredSoil = IsPlantOnWateredSoil(plant);

                    if (isOnWateredSoil)
                    {
                        // Enable plant mesh renderers in all zones so the plants can grow
                        foreach (var meshRenderer in _zonesManager)
                        {
                            meshRenderer.EnablePlantMesh(true);
                        }
                        plantedPlants.Add(plant);
                        _plantAnimators.Add(plantAnimator);
                        plantAnimator.SetBool("IsGrowing", true);
                    }
                    else
                    {
                        foreach (var meshRenderer in _zonesManager)
                        {
                            meshRenderer.EnablePlantMesh(false);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"No Animator component found on plant: {plant.name}");
                }
                

            }
        }

        bool IsPlantOnWateredSoil(GameObject plant)
        {
            foreach (WateringSoil soil in _soilPatches)
            {
                if (soil == null) continue;
                float distance = Vector3.Distance(plant.transform.position, soil.transform.position);
                
                if(soil.fullyWatered)
                {
                    Collider soilCollider = soil.GetComponent<Collider>();
                    if (soilCollider != null)
                    {
                        leverDown = true;
                        ambienceChange = true;
                        Vector3 plantPos = plant.transform.position;
                        Bounds bounds = soilCollider.bounds;

                        bounds.Expand(2f);

                        if (bounds.Contains(plantPos))
                        {
                            return true;
                        }
                    }
                    if (distance <= 5f)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Head"))
        {
            leverDown = false;
        }
    }
}