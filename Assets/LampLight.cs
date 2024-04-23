using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampLight : MonoBehaviour
{
    public Transform target; // B 위치의 Transform

    private LineRenderer lineRenderer; // LineRenderer 컴포넌트
    void Start()
    {
        // LineRenderer 컴포넌트 가져오기 또는 추가하기
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // 선의 두께 설정 (선택 사항)
        lineRenderer.startWidth = 10f;
        lineRenderer.endWidth = 10f;

        // 선의 포인트 설정
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        // A 위치에서 B 위치까지 선 그리기
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, target.position + Vector3.up * 3.0f);
    }
}

