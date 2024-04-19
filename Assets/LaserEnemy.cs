using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class LaserEnemy : MonoBehaviour
{
    public static bool IsArrived = false;
    public GameObject targetPosition1;
    public GameObject targetPosition2;
    public GameObject targetPosition3;
    public float speed = 1.0f;
    public float stoppingDistance = 0.5f;
    private bool EnemyRotate = false;
    void Start()
    {
        StartCoroutine(RobotAction());
    }
    void Update()
    {
        if(!EnemyRotate)
            transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
    }

    IEnumerator RobotAction()
    {
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(LaserPattern1());
        yield return new WaitForSeconds(3.0f);
        GetComponent<LaserPattern>().enabled = true;
        GetComponent<LineRenderer>().enabled = true;
        yield return new WaitForSeconds(1.5f);
        IsArrived = true;
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(LaserPattern2());
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(LaserPattern3());
        yield return StartCoroutine(LaserPattern4());
        yield return StartCoroutine(LaserPattern5());
        yield return StartCoroutine(LaserPattern6());
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

    IEnumerator LaserPattern1()
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, targetPosition1.transform.position, -1, path);

        if (path.corners.Length > 0)
        {
            GetComponent<Animator>().SetBool("isMoving", true);
            while (Vector3.Distance(transform.position, path.corners[1]) > 1.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, path.corners[1], speed * Time.deltaTime);
                yield return null;
            }
            GetComponent<Animator>().SetBool("isMoving", false);
        }
    }
    IEnumerator LaserPattern2()
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, targetPosition2.transform.position, -1, path);

        if (path.corners.Length > 0)
        {
            GetComponent<Animator>().SetBool("isMoving", true);
            while (Vector3.Distance(transform.position, path.corners[1]) > 1.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, path.corners[1], speed * Time.deltaTime);
                yield return null;
            }
            GetComponent<Animator>().SetBool("isMoving", false);
        }
    }
    IEnumerator LaserPattern3()
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, targetPosition1.transform.position, -1, path);

        if (path.corners.Length > 0)
        {
            GetComponent<Animator>().SetBool("isMoving", true);
            while (Vector3.Distance(transform.position, path.corners[1]) > 1.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, path.corners[1], 30 * Time.deltaTime);
                yield return null;
            }
            GetComponent<Animator>().SetBool("isMoving", false);
        }
    }
    IEnumerator LaserPattern4()
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, targetPosition2.transform.position, -1, path);

        if (path.corners.Length > 0)
        {
            GetComponent<Animator>().SetBool("isMoving", true);
            while (Vector3.Distance(transform.position, path.corners[1]) > 1.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, path.corners[1], 30 * Time.deltaTime);
                yield return null;
            }
            GetComponent<Animator>().SetBool("isMoving", false);
        }
    }

    IEnumerator LaserPattern5()
    {
        ResetLaser();
        GetComponent<LaserPattern>().enabled = false;
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, targetPosition3.transform.position, -1, path);

        if (path.corners.Length > 0)
        {
            GetComponent<Animator>().SetBool("isMoving", true);
            while (Vector3.Distance(transform.position, path.corners[1]) > 1.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, path.corners[1], 15 * Time.deltaTime);
                yield return null;
            }
            GetComponent<Animator>().SetBool("isMoving", false);
        }
        yield return new WaitForSeconds(1.0f);
        
        EnemyRotate = true;
        float elapsedTime = 0.0f;
        GetComponent<LaserPattern>().enabled = true;
        while (elapsedTime < 15f)
        {
            elapsedTime += Time.deltaTime;
            float angleToRotate = 60f * Time.deltaTime;
            transform.Rotate(0f, angleToRotate, 0f);
            yield return null;
        }
        
    }
    IEnumerator LaserPattern6()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < 15f)
        {
            elapsedTime += Time.deltaTime;
            float angleToRotate = -60f * Time.deltaTime;
            transform.Rotate(0f, angleToRotate, 0f);
            yield return null;
        }

    }
    void ResetLaser()
    {
        GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
        GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
    }
}
