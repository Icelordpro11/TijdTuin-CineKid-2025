using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//TODO: Summary, Styleguide
public class OwlDisplacer : MonoBehaviour
{
    public static OwlDisplacer instance;

    [Header("OwlStats")]
    [SerializeField] internal float speed = 5f;
    internal float rotationSpeed = 5f;
    private bool CanGo = true;

    [Header("Data")]
    public int trackedPlants;
    public GameObject TimeMachineStand;
    public Animator exitDoorAnimator;
    [SerializeField] private GameObject exitTrigger;
    


    [Header("LocationData")]
    public Quaternion newRotation;
    private Animator _animator;
    private bool isFlyingRoutineStarted = false;
    private VoiceLinePlayer _voiceLines;
    public bool isAtMachine = false;

    [field: SerializeField] public int SeedCounter { get; set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _voiceLines = GetComponentInChildren<VoiceLinePlayer>();
    }

    private void Update()
    {
        if (!isFlyingRoutineStarted && SeedCounter >= 5 && CanGo)
        {
            StartCoroutine(TurnAndFlyToStand());
            isFlyingRoutineStarted = true;
            CanGo = false;
            
        }
    }

    private IEnumerator TurnAndFlyToStand()
    {
        Vector3 direction = TimeMachineStand.transform.position - transform.position;
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            //TODO: Extract naar eigen methode.
            while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
            {
                float step = Mathf.Clamp01(Time.deltaTime * rotationSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
                yield return null;
            }

            transform.rotation = targetRotation;
        }

        _animator.SetTrigger("StartFlying");

        AsyncTimerManager.Timer(TimeSpan.FromSeconds(1.2f), () =>
        {
            _animator.SetBool("IsFlying", true);
        }, null);
        
        

        Vector3 target = TimeMachineStand.transform.position;

        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            yield return null;
        }

        transform.rotation = newRotation;
        _animator.SetBool("IsFlying", false);
        _animator.SetBool("PointToLever", true);
        _voiceLines.TijdmachineVoiceline();
        isAtMachine = true;
    }

    public void StartBubo()
    {
        _animator.SetBool("IsExplaining", true);
    }
    
    public void EndBubo()
    {
        _voiceLines.ExitVoiceline();
        _animator.SetBool("PointToDoor", true);
        exitDoorAnimator.Play("ExitDoorOpen");
        exitTrigger.SetActive(true);
    }
}
