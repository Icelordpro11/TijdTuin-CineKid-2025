using System;
using System.Collections;
using System.Linq;
using UnityEngine;
public class CloudRotation : MonoBehaviour
{
    /*
    quick summary:
    You only have to put this script on the object you want to rotate, and set the speed. 
    I reccomend setting the items furthest away no less than 0.001 (_normalSpeed)
     */

    [SerializeField] private float _normalSpeed, _acceleratedSpeed, _currentSpeed;

    //You dont need to access this anywhere else. Leave this on private.
    private Quaternion _cloudRot;

    private void Start()
    {
        // set the current speed to the normal speed at the start of the game.
       _currentSpeed = _normalSpeed;
    }

    void Update()
    {
        _cloudRot = Quaternion.Euler(-89.98f, gameObject.transform.eulerAngles.y + _currentSpeed * Time.deltaTime, 0);
        gameObject.transform.localRotation = _cloudRot;
    }


    public void SpeedUpRotation()
    {
        if (_currentSpeed < _acceleratedSpeed)
        {
            _currentSpeed += 1f;
            return;
        }
        _currentSpeed = _acceleratedSpeed;
    }

    public void SlowDownRotation() 
    {
        if (_currentSpeed > _normalSpeed)
        {
            _currentSpeed -= 5f;
            return;
        }

        _currentSpeed = _normalSpeed;

    }
}
