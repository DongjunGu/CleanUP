using UnityEngine;

public class TestCamera : MonoBehaviour
{
    public float rotationSpeed = 2f;

    private void Update()
    {
        // ���콺 ȸ�� �Է� ó��
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Camera�� ȸ�� ���� (���� ������ ������ �� ����)
        transform.Rotate(Vector3.up * mouseX * rotationSpeed);
        transform.Rotate(Vector3.left * mouseY * rotationSpeed);

        // ���� ȸ�� ����
        float currentRotationX = transform.eulerAngles.x;
        if (currentRotationX > 80f && currentRotationX < 280f)
        {
            if (currentRotationX > 180f)
            {
                currentRotationX = Mathf.Clamp(currentRotationX, 280f, 360f);
            }
            else
            {
                currentRotationX = Mathf.Clamp(currentRotationX, 0f, 80f);
            }

            transform.eulerAngles = new Vector3(currentRotationX, transform.eulerAngles.y, 0f);
        }
    }
}
