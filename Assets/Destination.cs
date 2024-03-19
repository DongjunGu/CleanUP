using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    public GameObject pushableCube;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "pushable" && other.name == pushableCube.name)
        {
            Debug.Log(pushableCube.name + " Completed" );
        }
    }
}
