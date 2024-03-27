using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distach : MonoBehaviour
{
    public Transform parent;
    private bool detached = false;

    private void Start()
    {
        parent = transform.parent;
    }

    private void Update()
    {
        if (!detached)
        {
            StartCoroutine(DetachForDuration(3f));
        }
        //else
        //{
        //    StartCoroutine(DetachForDuration(2f));
        //}
    }

    IEnumerator DetachForDuration(float duration)
    {
        transform.parent = null;
        detached = true;

        yield return new WaitForSeconds(duration);

        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        detached = false;

    }
    IEnumerator AttachToParent(float duration)
    {
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        detached = false;

        yield return new WaitForSeconds(duration);
        

    }
}
