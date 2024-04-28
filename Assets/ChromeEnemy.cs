using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChromeEnemy : MonoBehaviour
{
    public Transform[] desPoint;
    bool _isDamaged = false;
    int count = 0;
    private Vector3 originPos;
    private void Awake()
    {
        originPos = transform.position;
    }
    private void OnEnable()
    {
        transform.position = originPos;
        StartCoroutine(StartChrome());
    }
    IEnumerator StartChrome()
    {
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(ChromeMove(40));
        yield return StartCoroutine(RestartChrome());
        yield return StartCoroutine(ChromeMove(40));
        yield return StartCoroutine(RestartChrome());
        yield return StartCoroutine(ChromeMove(40));
        yield return StartCoroutine(RestartChrome());
        yield return StartCoroutine(ChromeMove(40));
        yield return StartCoroutine(RestartChrome());
        yield return StartCoroutine(ChromeMove(40));
        yield return StartCoroutine(RestartChrome());
        yield return StartCoroutine(ChromeMove(40));
    }
    private void Update()
    {
        transform.rotation *= Quaternion.Euler(0f, 0f, -100f * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!_isDamaged)
            {
                collision.gameObject.GetComponent<NewPlayerController>().currentHp -= 50;
                collision.gameObject.GetComponent<NewPlayerController>().hpUI.takeDamage(50);
                StartCoroutine(OnDamage());
            }
                
        }
    }

    IEnumerator OnDamage()
    {
        _isDamaged = true;
        yield return new WaitForSeconds(1.0f);
        _isDamaged = false;
    }

    IEnumerator ChromeMove(int speed)
    {
        
        while (count < desPoint.Length)
        {
            while (Vector3.Distance(transform.position, desPoint[count].position) > 1.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, desPoint[count].position, speed * Time.deltaTime);
                yield return null;
            }
            count++;
        }
    }
    
    IEnumerator RestartChrome()
    {
        yield return new WaitForSeconds(0.1f);
        transform.position = desPoint[0].position;
        count = 0;
        
    }
}
