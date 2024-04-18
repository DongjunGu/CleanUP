using UnityEngine;
using UnityEngine.AI;

public class testttt : MonoBehaviour
{
    public Transform target; // �̵��� ��ǥ ����

    private NavMeshAgent agent; // NavMeshAgent ������Ʈ

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgent ������Ʈ ��������
        CalculatePath(); // ��� ��� �Լ� ȣ��
    }

    void CalculatePath()
    {
        NavMeshPath path = new NavMeshPath(); // NavMeshPath ��ü ����

        // NavMesh ��� ���
        if (NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path))
        {
            agent.SetPath(path); // NavMeshAgent�� ��� ����
            MoveToTarget(); // �̵� ���� �Լ� ȣ��
        }
        else
        {
            Debug.Log("��θ� ã�� �� �����ϴ�.");
        }
    }

    void MoveToTarget()
    {
        agent.isStopped = false; // NavMeshAgent�� ���� ����
        agent.destination = target.position; // NavMeshAgent�� ������ ����
    }
}
