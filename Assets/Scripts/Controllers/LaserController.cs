using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public LayerMask layerMask; // 레이저가 충돌을 감지할 레이어
    public GameObject target;
    private LineRenderer lineRenderer;

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
            yield return new WaitForSeconds(8f);
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

        GameObject laserDest = GameObject.Find("LaserDest");
        target = laserDest;

        Vector3 startPos = transform.position;
        Vector3 destination = (target.transform.position + Vector3.up * 1.0f) - startPos;
        float distance = destination.magnitude;

        RaycastHit hit;
        if (Physics.Raycast(startPos, destination, out hit, distance, layerMask))
        {
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, hit.point);
            NewPlayerController player = hit.transform.GetComponent<NewPlayerController>();
            if (player != null)
            {
                player.currentHp -= 50;
                player.hpUI.takeDamage(50);
            }
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
