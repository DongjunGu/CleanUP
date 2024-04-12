using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShoot : MonoBehaviour
{
    public GameObject target;
    private LineRenderer lineRenderer;


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        StartCoroutine(ShootLaserCoroutine());
        
    }
    IEnumerator ShootLaserCoroutine()
    {
        lineRenderer.startWidth = 0f;
        lineRenderer.endWidth = 0f;
        lineRenderer.positionCount = 2;
        lineRenderer.material.color = Color.red;

        Vector3 startPos = transform.position;
        Vector3 destination = (target.transform.position) - startPos;
        float distance = destination.magnitude;

        RaycastHit hit;

        float duration = 0.7f; 
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            lineRenderer.startWidth = Mathf.Lerp(0f, 1f, t);
            lineRenderer.endWidth = Mathf.Lerp(0f, 1f, t);

            
            if (Physics.Raycast(startPos, destination, out hit, distance))
            {
                Vector3 hitPoint = Vector3.Lerp(startPos, hit.point, t);
                lineRenderer.SetPosition(0, startPos);
                lineRenderer.SetPosition(1, hitPoint);
            }
            else
            {
                Vector3 endPos = startPos + destination.normalized * distance;
                Vector3 endPosLerp = Vector3.Lerp(startPos, endPos, t);
                lineRenderer.SetPosition(0, startPos);
                lineRenderer.SetPosition(1, endPosLerp);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        lineRenderer.startWidth = 1f;
        lineRenderer.endWidth = 1f;
        yield return new WaitForSeconds(2.0f);
        lineRenderer.enabled = false;
    }
}
