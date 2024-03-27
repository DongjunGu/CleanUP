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
        transform.localPosition = Vector3.zero;
        detached = true;
        yield return new WaitForSeconds(7.0f); //7���Ŀ� distach

        transform.parent = null; // distach��
        

        yield return new WaitForSeconds(duration); //1�ʵ� �ٽ� attach
        detached = false;

    }
}
