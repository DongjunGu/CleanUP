using UnityEngine;

public class TestController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Camera playerCamera;

    private void Start()
    {
        // Player 스크립트가 추가된 GameObject의 Camera 컴포넌트 참조
        playerCamera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        // 이동 입력 처리
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Camera의 forward 방향을 기준으로 이동 방향 계산
        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        Vector3 moveDirectionRelativeToCamera = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized;

        // 이동 적용
        transform.Translate(moveDirectionRelativeToCamera * moveSpeed * Time.deltaTime, Space.World);

        // 회전 입력 처리
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * mouseX);
    }
}