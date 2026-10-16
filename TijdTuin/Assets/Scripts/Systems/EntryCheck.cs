using UnityEngine;

/// <summary>
/// Called when the player enters the door trigger. 
/// Invokes the OwlDisplacer to begin the Bubo sequence. 
/// Used on the door prefab and triggered through a UnityEvent EnterTrigger.
/// </summary>
public class EntryCheck : MonoBehaviour
{
    public void EnterDoor()
    {
        OwlDisplacer.instance.StartBubo();
    }
}
