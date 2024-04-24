using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastStage : MonoBehaviour
{
    public GameObject[] folders;
    public Transform target;
    private void Start()
    {
        StartCoroutine(ThrowFolder());


    }

    IEnumerator ThrowFolder()
    {
        foreach (GameObject obj in folders)
        {
            Vector3 dir = (target.position - obj.transform.position).normalized;
            obj.GetComponent<Rigidbody>().velocity = (dir * 100f);
            yield return new WaitForSeconds(2.0f);
            obj.SetActive(false);
        }
    }
}
