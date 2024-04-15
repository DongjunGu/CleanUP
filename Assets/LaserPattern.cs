using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPattern : MonoBehaviour
{
    public GameObject target;
    private LineRenderer lineRenderer;
    public float laserWidth = 0.1f;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;
    }
    private void Update()
    {
        if (target != null)
        {
            StartCoroutine(ShootLaserCoroutine());
        }
        else
        {
            lineRenderer.enabled = false;
        }
        
    }
    IEnumerator ShootLaserCoroutine()
    {
        lineRenderer.SetPosition(0, transform.position);
        Vector3 targetPosition = target.transform.position;
        Vector3 direction = targetPosition - transform.position;
        float distance = direction.magnitude;
        targetPosition = transform.position + direction.normalized * distance;
        lineRenderer.SetPosition(1, targetPosition);
        yield return null;
    }
}
