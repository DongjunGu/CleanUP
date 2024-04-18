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
        lineRenderer.enabled = false;
        Invoke("ActiveLaser", 5f);
    }
    private void Update()
    {
        ShootLaser();

    }
    //IEnumerator ShootLaserCoroutine()
    //{
    //    lineRenderer.SetPosition(0, transform.position);
    //    Vector3 targetPosition = target.transform.position;
    //    Vector3 newDir = Vector3.forward;
    //    //Vector3 direction = targetPosition - transform.position;
    //    //float distance = direction.magnitude;
    //    float distance = newDir.magnitude * 10f;
    //    //targetPosition = transform.position + direction.normalized * distance;
    //    targetPosition = transform.position + newDir.normalized * distance;
    //    lineRenderer.SetPosition(1, targetPosition);
    //    yield return null;
    //}

    void ShootLaser()
    {
        lineRenderer.SetPosition(0, transform.position + Vector3.up * 3f);
        lineRenderer.SetPosition(1, transform.position + Vector3.up * 3f + transform.forward * 100f);
    }
    void ActiveLaser()
    {
        lineRenderer.enabled = true;
    }

}
