using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class CloudRingManager : MonoBehaviour
{
    //TODO: Benaming
    /*
    Short summary:
    This scripts collects all cloud rings in the cloudringholder. 
    Unless you are working on the clouds, you do not need to edit this script.
     */

    public Transform[] cloudRings;
    void Start()
    {
        cloudRings = new Transform[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            cloudRings[i] = transform.GetChild(i);
        }
    }


}
