using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test123 : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                StopAllCoroutines();
                StartCoroutine(MovingToPos(hit.point));
            }
        }
    }

    IEnumerator MovingToPos(Vector3 pos)
    {
        Vector3 dir = pos - transform.position;
        float dist = dir.magnitude;
        dir.Normalize();
        while(dist > 0.0f)
        {
            float delta = Time.deltaTime * 2.0f;
            if(dist < delta) 
            {
                delta = dist;
            }
            dist -= delta;
            transform.Translate(dir * delta, Space.World);
            yield return null;
        }
    }
}
