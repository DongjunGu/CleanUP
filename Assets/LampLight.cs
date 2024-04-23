using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampLight : MonoBehaviour
{
    public Transform target; // B ��ġ�� Transform

    private LineRenderer lineRenderer; // LineRenderer ������Ʈ
    void Start()
    {
        // LineRenderer ������Ʈ �������� �Ǵ� �߰��ϱ�
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // ���� �β� ���� (���� ����)
        lineRenderer.startWidth = 10f;
        lineRenderer.endWidth = 10f;

        // ���� ����Ʈ ����
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        // A ��ġ���� B ��ġ���� �� �׸���
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, target.position + Vector3.up * 3.0f);
    }
}

