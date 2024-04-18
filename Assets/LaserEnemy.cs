using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class LaserEnemy : MonoBehaviour
{
    public GameObject targetPosition1;
    public GameObject targetPosition2;
    public float speed = 1.0f;
    public float stoppingDistance = 0.5f;
    private bool isMoving = true;
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgent 컴포넌트 가져오기
        StartCoroutine(FirstPos());
    }
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        
    }

    IEnumerator FirstPos()
    {
        yield return new WaitForSeconds(1.0f);
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, targetPosition2.transform.position, -1, path);

        


        if(path.corners.Length > 0 )
        {
            while (Vector3.Distance(transform.position, path.corners[1]) > 1.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, path.corners[1], speed * Time.deltaTime);
                yield return null;
            }
        }
        

    }



    void SpawnLaserEnemy()
    {
        while (Vector3.Distance(transform.position, targetPosition1.transform.position) > 1.5f)
        {
            GetComponent<Animator>().SetBool("isMoving", true);
            transform.position = Vector3.Lerp(transform.position, targetPosition1.transform.position, speed * Time.deltaTime);
        }
        GetComponent<Animator>().SetBool("isMoving", false);
    }
    void OnMovementComplete()
    {
        isMoving = false;
    }

    IEnumerator LaserPattern1()
    {
        while (Vector3.Distance(transform.position, targetPosition2.transform.position) > 1.5f)
        {
            GetComponent<Animator>().SetBool("isMoving", true);
            transform.position = Vector3.Lerp(transform.position, targetPosition2.transform.position, speed * Time.deltaTime);
            yield return null;
        }
        GetComponent<Animator>().SetBool("isMoving", false);
    }

}
