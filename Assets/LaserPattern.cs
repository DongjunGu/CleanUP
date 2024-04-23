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
    public LayerMask hitLayers;
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
    void FireLaser()
    {
        lineRenderer.SetPosition(0, transform.position + Vector3.up * 3f);

        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up * 3f, transform.forward, out hit, maxDistance))
        {
            if(hit.transform.tag == "Player")
            {
                NewPlayerController playerScript = hit.transform.GetComponent<NewPlayerController>();
                playerScript.GetLaserDamaer(2);
            }
            
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position + Vector3.up * 3f + transform.forward * maxDistance);
        }

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
