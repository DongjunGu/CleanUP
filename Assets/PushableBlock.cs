using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PushableBlock : MonoBehaviour
{
    public UnityEvent<bool> crashAct;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("pushable"))
        {
            crashAct?.Invoke(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("pushable"))
        {
            crashAct?.Invoke(false);
        }
    }
}
