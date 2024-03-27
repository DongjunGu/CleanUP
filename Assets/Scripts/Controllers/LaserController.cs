using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public LayerMask layerMask; // 레이저가 충돌을 감지할 레이어
    public GameObject target;

    private LineRenderer lineRenderer;
    private bool laserActive = true;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        StartCoroutine(ShootLaserRepeatedly());

    }

    void Update()
    {
        
    }

    IEnumerator ShootLaserRepeatedly()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            lineRenderer.enabled = true;
            ShootLaser();
            StartCoroutine(HideLaser(1f));
        }
    }
    void ShootLaser()
    {
        //Laser발사
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.8f;
        lineRenderer.positionCount = 2;
        lineRenderer.material.color = Color.red;

        Vector3 startPos = transform.position;
        Vector3 destination = (target.transform.position + Vector3.up * 1.0f) - startPos;
        float distance = destination.magnitude;

        RaycastHit hit;
        if (Physics.Raycast(startPos, destination, out hit, distance, layerMask))
        {
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, hit.point);

        }
        else
        {
            Vector3 endPos = startPos + destination.normalized * distance;
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
        }
    }

    IEnumerator HideLaser(float delay)
    {
        yield return new WaitForSeconds(delay);
        lineRenderer.enabled = false;
    }
}
