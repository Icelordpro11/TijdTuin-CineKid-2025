using System;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// <c>OvergrownStateChanger</c> is a class that manages the overgrown state of the environment based on the number of plants in all ZonesManagers.
/// </summary>
public class OvergrownStateChanger : MonoBehaviour
{
    [Header("References")] 
    private ZonesManager[] _zonesManager;
    [SerializeField] private MeshRenderer _floorMeshRenderer;

    [Header("Materials for different states of the asset.")]
    public Material[] floorMaterials;

    [Header("Other Objects to enable/animate")]
    public GameObject[] vlinders;
    public GameObject[] vines; 
    public Animator[] vinesAnimators;
    public Animator[] leafAnimators;

    [Header("State Amount")] 
    private const int _stateOneAmount = 5;
    private const int _stateTwoAmount = 10;
    private const int _stateThreeAmount = 15;

    /// <summary>
    ///  Method <method>UpdateOvergrownState</method> Checks the amount of plants in all ZonesManagers and updates the environment state 
    ///  accordingly.
    /// </summary>
    public void UpdateOvergrownState()
    {
        var _amountOfPlants = 0;
       
        _zonesManager = FindObjectsByType<ZonesManager>(FindObjectsSortMode.None);
        foreach (var zonesManager in _zonesManager)
        {
            _amountOfPlants += zonesManager.amountOfZonesFilled;
        }
        
        if (_amountOfPlants >= _stateOneAmount && _amountOfPlants < _stateTwoAmount)
        {
            _floorMeshRenderer.material = floorMaterials[0];
            vlinders[0].SetActive(true);
            vines[0].SetActive(true);
            vines[1].SetActive(true); 
            AnimatorTrigger(0, 2);
        }
        else if (_amountOfPlants >= _stateTwoAmount && _amountOfPlants < _stateThreeAmount)
        {
            _floorMeshRenderer.material = floorMaterials[1];
            vlinders[1].SetActive(true);
            vines[2].SetActive(true);
            vines[3].SetActive(true);
            AnimatorTrigger(2, 2);
        }
        else if (_amountOfPlants >= _stateThreeAmount)
        {
            _floorMeshRenderer.material = floorMaterials[2];
            vlinders[2].SetActive(true);
            vines[4].SetActive(true);
            vines[5].SetActive(true);
            AnimatorTrigger(4, 2);
        }
    }

    /// <summary>
    /// Triggers the growth animations for a specified range of vine and leaf animators.
    /// </summary>
    /// <param name="startIndex">Start index in the animator arrays.</param> 
    /// <param name="count">The current count of the animators to trigger.</param> 
    private void AnimatorTrigger(int startIndex, int count)
    {
        const string growVines = "GrowVines";
        const string growLeaves = "GrowLeaves";
        for (int i = startIndex; i < startIndex + count && i < vinesAnimators.Length; i++)
        {
            vinesAnimators[i].SetTrigger(growVines);
        }
        for (int i = startIndex; i < startIndex + count && i < leafAnimators.Length; i++)
        {
            leafAnimators[i].SetTrigger(growLeaves);
        }
    }
    
    
    
}
