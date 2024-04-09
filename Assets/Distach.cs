using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distach : MonoBehaviour
{
    public Transform parent;
    private bool detached = false;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        detached = false;
    }
    private void FixedUpdate()
    {
        if (!detached)
        {
            StartCoroutine(DetachForDuration(1f));

        }
    }

    IEnumerator DetachForDuration(float duration)
    {
        transform.parent = parent;
        transform.localPosition = new Vector3(0f,-0.2f,0f);
        detached = true;
        yield return new WaitForSeconds(7.0f); //7초후에 distach

        transform.parent = null; // distach후
       

        yield return new WaitForSeconds(duration); //1초뒤 다시 attach
        detached = false;

    }
}
