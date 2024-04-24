using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastStage : MonoBehaviour
{
    public GameObject[] folders;
    public Transform target;
    public GameObject chrome;
    private void Start()
    {
        StartCoroutine(StartLastStage());

    }
    IEnumerator StartLastStage()
    {
        yield return StartCoroutine(ThrowFolder());
        yield return StartCoroutine(ActiveChrome());
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

    IEnumerator ActiveChrome()
    {
        chrome.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        chrome.GetComponent<ChromeEnemy>().enabled = true;
    }
}
