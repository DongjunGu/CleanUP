using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPattern : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public float maxDistance = 70f;
    public float duration = 3f;
    public float laserWidth = 1f;
    private float elapsedTime = 0f;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;
        
    }
    private void Update()
    {
        FireLaser();
    }

    //IEnumerator FireLaser()
    //{
    //    lineRenderer.SetPosition(0, transform.position + Vector3.up * 3f);

    //    float initialDistance = Vector3.Distance(transform.position + Vector3.up * 3f, transform.position + Vector3.up * 3f + transform.forward * maxDistance);

    //    while (elapsedTime < duration)
    //    {
    //        float progress = elapsedTime / duration;
    //        float currentDistance = initialDistance * progress;
    //        Vector3 currentPosition = transform.position + Vector3.up * 3f + transform.forward * currentDistance;

    //        lineRenderer.SetPosition(1, currentPosition);

    //        elapsedTime += Time.deltaTime;

    //        yield return null;
    //    }

    //    lineRenderer.SetPosition(1, transform.position + Vector3.up * 3f + transform.forward * maxDistance);
    //}
    void FireLaser()
    {
        lineRenderer.SetPosition(0, transform.position + Vector3.up * 3f);

        float initialDistance = Vector3.Distance(transform.position + Vector3.up * 3f, transform.position + Vector3.up * 3f + transform.forward * maxDistance);

        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;
            float currentDistance = initialDistance * progress;
            Vector3 currentPosition = transform.position + Vector3.up * 3f + transform.forward * currentDistance;

            lineRenderer.SetPosition(1, currentPosition);

            elapsedTime += Time.deltaTime;

            return;
        }
        lineRenderer.SetPosition(1, transform.position + Vector3.up * 3f + transform.forward * maxDistance);

        
    }

    private void ResetLineRenderer()
    {
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);
        elapsedTime = 0;
    }
}
