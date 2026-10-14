using System;
using System.Collections;
using System.Linq;
using UnityEngine;
public class SkyboxFunctionality : MonoBehaviour
{
    [SerializeField] private GameObject _timeMachineLever;
    private Transform[] _skyboxes;
    [SerializeField] private float _minSpeed = 0.001f, _maxSpeed = 40f, _accelerationSpeed = 0.2f, _decelerationSpeed = 0.75f;
    private bool _callEnumerator = true;
    private Animator _animator;
    public GameObject cloudRingHolder;
    private CloudRotation _cloudRotation;
    private CloudRingManager _cloudRingManager;


    void Start()
    {
        /*
            Get reference to the animator in the skybox, the sun and moon, and the cloud rings
            before setting the speed of both of the animation speeds of the skybox and the
            sun and moon to 0.001.
         */
        _cloudRingManager = cloudRingHolder.GetComponent<CloudRingManager>();
        _skyboxes = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            _skyboxes[i] = transform.GetChild(i);
            _animator = _skyboxes[i].GetComponentInParent<Animator>();
            _animator.speed = 0.001f;
        }
    }

    void Update()
    {
        // checks if the time machine lever is down and checks if the enumerator has been called or not.
        // This stops one coroutine and stops the other depending on the state of the game.
        if (_timeMachineLever.GetComponent<TimeMachine>().leverDown == true && _callEnumerator == true)
        {
            StopCoroutine(SkyboxDeceleration());
            StartCoroutine(SkyboxAcceleration());
        }
        if(_timeMachineLever.GetComponent<TimeMachine>().leverDown == false && _callEnumerator == false)
        {
            StopCoroutine(SkyboxAcceleration());
            StartCoroutine(SkyboxDeceleration());
        }
    }

    // speeds up all the animations that have to do with the skyboxes
    private IEnumerator SkyboxAcceleration()
    {
        _callEnumerator = false;
        while (_timeMachineLever.GetComponent<TimeMachine>().leverDown == true)
        {
            foreach (Transform child in _cloudRingManager.cloudRings)
            {
                _cloudRotation = child.GetComponent<CloudRotation>();
                _cloudRotation.SpeedUpRotation();
            }

            for (int i = 0; i < _skyboxes.Length; i++)
            {
                _animator = _skyboxes[i].GetComponentInParent<Animator>();
                if (_animator.speed < _maxSpeed)
                {
                    _animator.speed += _accelerationSpeed;
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    // returns all animations to do with the skyboxes to their initial speed.
    private IEnumerator SkyboxDeceleration()
    {
        
        _callEnumerator = true;
        while (_timeMachineLever.GetComponent<TimeMachine>().leverDown == false) 
        {
            foreach (Transform child in _cloudRingManager.cloudRings)
            {
                _cloudRotation = child.GetComponent<CloudRotation>();
                _cloudRotation.SlowDownRotation();
            }

            for (int i = 0; i < _skyboxes.Length; i++)
            {
                _animator = _skyboxes[i].GetComponentInParent<Animator>();
                if (_animator.speed > _minSpeed + 0.1f)
                {
                    _animator.speed -= _decelerationSpeed;
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
}

