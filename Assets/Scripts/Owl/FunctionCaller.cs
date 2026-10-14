using System;
using UnityEngine;

//TODO: Beschrijving toevoegen.
//TODO: Naam van de class veranderen.
public class FunctionCaller : MonoBehaviour
{
    private MovetoTutorial _tutorialMovement;
    public bool FinishedTalk = false;
    private Animator _animator;

    private const float _awaitBeforeFlying = 1.2f;
    private const float _experienceTime = 5f;
    private const string _isFlying = "IsFlying";
    private const string _isReminder = "IsReminding";

    private void Start()
    {
        _tutorialMovement = GetComponentInParent<MovetoTutorial>();
        _animator = GetComponent<Animator>();
    }

    public void GoTutorial()
    {
        AsyncTimerManager.Timer(TimeSpan.FromSeconds(_awaitBeforeFlying), () =>
        {
            _animator.SetBool(_isFlying, true);
            FinishedTalk = true;
        }, () => { _tutorialMovement.tutorialTime = true; });
    }
    
    public void StopAnimation()
    {
        _animator.SetBool(_isReminder, false);
    }
    
    /// <summary>
    /// Starts a timer that runs for the set experience duration, then triggers EndExperience when finished.
    /// </summary>
    public void StartTimer()
    {
        AsyncTimerManager.Timer(TimeSpan.FromSeconds(_experienceTime), null, EndExperience);
    }

    private void EndExperience() => MapResetSystem.Instance.EndExperience();
}
