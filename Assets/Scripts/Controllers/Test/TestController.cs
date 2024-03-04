using UnityEngine;

public class TestController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Camera playerCamera;

    private void Start()
    {
        // Player ��ũ��Ʈ�� �߰��� GameObject�� Camera ������Ʈ ����
        playerCamera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        // �̵� �Է� ó��
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Camera�� forward ������ �������� �̵� ���� ���
        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        Vector3 moveDirectionRelativeToCamera = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized;

        // �̵� ����
        transform.Translate(moveDirectionRelativeToCamera * moveSpeed * Time.deltaTime, Space.World);

        // ȸ�� �Է� ó��
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * mouseX);
    }
}