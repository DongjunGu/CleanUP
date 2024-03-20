using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    public GameObject correctObject;
    public GameObject effect;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "pushable" && other.name == correctObject.name)
        {
            Debug.Log(correctObject.name + " Completed" );
            effect.SetActive(true);
        }
        if(other.tag == "Player" && other.name == correctObject.name)
        {
            effect.SetActive(true);
        }
    }
}
