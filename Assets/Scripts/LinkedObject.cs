using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LinkedObject : MonoBehaviour
{
    public LayerMask mask;
    public UnityEvent obtainAct;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if((1 << other.gameObject.layer & mask) != 0)
        {
            obtainAct?.Invoke();
        }
    }
}
