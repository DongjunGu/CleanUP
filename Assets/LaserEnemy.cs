using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy : MonoBehaviour
{
    public GameObject targetPosition;
    public float speed = 50.0f;
    public float stoppingDistance = 0.5f;
    private bool isMoving = true;

    private void Start()
    {
        transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
    }
    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.transform.position, speed * Time.deltaTime);

            float distanceToTarget = Vector3.Distance(transform.position, targetPosition.transform.position);

            if (distanceToTarget <= stoppingDistance)
            {
                OnMovementComplete();
            }
        }

    }
    private void OnMovementComplete()
    {
        isMoving = false;
    }
}
