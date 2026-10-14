using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Events;

public class VoiceLinePlayer : MonoBehaviour
{
    public static VoiceLinePlayer Instance;

    public AudioClip intro, timeMachine, herhalingStappenplan, exit;
    private AudioSource audioSource;
    
    public bool isTalking;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (audioSource.isPlaying)
        {
            isTalking = true;
        }
        else
        {
            isTalking = false; 
        }
    }

    void PlayIntro()
    {
        audioSource.PlayOneShot(intro);
    }

    public void HerhaalStappenplan(Animator animator, bool hasMovedToMachine)
    {
        // Samenvatting: speel na 90 seconden een coroutine af die iedere 90 seconden het stappenplan herhaalt voor de speler.

        Action action = () =>
        {
            StartCoroutine(HerhaalStappenplanLoop(animator, hasMovedToMachine));
        };  
        AsyncTimerManager.Timer(TimeSpan.FromSeconds(90), null, action);
    }

    public void TijdmachineVoiceline()
    {
        audioSource.PlayOneShot(timeMachine);
    }

    public void ExitVoiceline()
    {
        audioSource.PlayOneShot(exit);
    }

    
    public IEnumerator HerhaalStappenplanLoop(Animator animator, bool hasMovedToMachine)
    {

        while(hasMovedToMachine == false && !OwlDisplacer.instance.isAtMachine) 
        {
            animator.SetBool("IsReminding", true);
            audioSource.PlayOneShot(herhalingStappenplan);
            yield return new WaitForSeconds(90);
        }

    }


}
