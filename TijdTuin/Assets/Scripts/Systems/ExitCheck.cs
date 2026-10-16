using System;
using UnityEngine;

/// <summary>
/// Handles the door exit logic by scheduling the end of the current experience. 
/// Invokes the MapResetSystem after a short delay. 
/// Used on the exit door prefab and triggered through a UnityEvent.
/// </summary>
public class ExitCheck : MonoBehaviour
{
    public void EndExperience()
    {
        AsyncTimerManager.Timer(TimeSpan.FromSeconds(1), null, () =>
        {
          MapResetSystem.Instance.EndExperience();
        });
    }
}
