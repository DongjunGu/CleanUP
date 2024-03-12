using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LinkedObject : MonoBehaviour
{
    public LayerMask mask;
    public UnityEvent obtainAct;


    // Update is called once per frame
    void Update()
    {

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if ((1 << other.gameObject.layer & mask) != 0)
    //    {
    //        Debug.Log("GOTIT??");
    //        obtainAct?.Invoke();
    //    }
    //}
    void OnTriggerStay(Collider other)
    {
        if ((1 << other.gameObject.layer & mask) != 0)
        {
            Debug.Log("GOTIT??");
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("KeyPressed");
                obtainAct?.Invoke();
            }
        }
    }
}
