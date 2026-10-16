using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using TMPro;

//TODO: Summary
public class MovetoTutorial : MonoBehaviour
{
    [Header("Checkpoints")]
    [SerializeField] private GameObject _waitingStand;

    [Header("Components")]
    private OwlDisplacer _owlDisplacer;
    private Animator _animator;
    [SerializeField] private Animator shedDoorAnimator;
    
    [Header("Variables")]
    internal bool tutorialTime = false;
    public Quaternion newRotation;
    private bool isMovingToTutorial = false;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _owlDisplacer = GetComponent<OwlDisplacer>();
    }

    private void Update()
    {
        if (tutorialTime && !isMovingToTutorial)
        {
            StartCoroutine(MoveAndFlyToTutorial());
        }
    }

    private IEnumerator MoveAndFlyToTutorial()
    {
        isMovingToTutorial = true;


        Vector3 direction = _waitingStand.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            float step = Mathf.Clamp01(Time.deltaTime * _owlDisplacer.rotationSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
            yield return null;
        }

        Vector3 checkpointTarget = _waitingStand.transform.position;

        Vector3 target = _waitingStand.transform.position;
        shedDoorAnimator.Play("ShedDoorOpen");

        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            float step = _owlDisplacer.speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            yield return null;
        }


        while (Vector3.Distance(transform.position, checkpointTarget) > 0.1f)
        {
            float checkpointStep = _owlDisplacer.speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, checkpointTarget, checkpointStep);
            yield return null;
        }

        transform.position = target;
        _animator.SetBool("IsFlying", false);
        _animator.SetBool("IsExplaining", false);
        transform.rotation = newRotation;
        tutorialTime = false;
        isMovingToTutorial = false;

        VoiceLinePlayer.Instance.HerhaalStappenplan(_animator, isMovingToTutorial);
    
        
      
    }
}
