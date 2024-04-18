using UnityEngine;
using UnityEngine.AI;

public class testttt : MonoBehaviour
{
    public Transform target; // 이동할 목표 지점

    private NavMeshAgent agent; // NavMeshAgent 컴포넌트

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgent 컴포넌트 가져오기
        CalculatePath(); // 경로 계산 함수 호출
    }

    void CalculatePath()
    {
        NavMeshPath path = new NavMeshPath(); // NavMeshPath 객체 생성

        // NavMesh 경로 계산
        if (NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path))
        {
            agent.SetPath(path); // NavMeshAgent에 경로 설정
            MoveToTarget(); // 이동 시작 함수 호출
        }
        else
        {
            Debug.Log("경로를 찾을 수 없습니다.");
        }
    }

    void MoveToTarget()
    {
        agent.isStopped = false; // NavMeshAgent의 정지 해제
        agent.destination = target.position; // NavMeshAgent의 목적지 설정
    }
}
