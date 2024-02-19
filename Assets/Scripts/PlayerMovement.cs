using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Set the Rigidbody to be kinematic initially
        rb.isKinematic = true;
    }

    void Update()
    {
        // Get input from the keyboard
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Move the player
        MovePlayer(movement);
    }

    void MovePlayer(Vector3 direction)
    {
        // Move the player using Rigidbody
        Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;
        rb.MovePosition(newPosition);
    }
}