using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    public float forwardSpeed = 5.0f; // Speed at which the bird moves forward
    public float horizontalSpeed = 3.0f; // Speed at which the bird moves left/right
    public float tiltSensitivity = 2.0f; // Sensitivity of tilt control

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Move the bird forward
        Vector3 forwardMovement = -transform.forward * forwardSpeed * Time.deltaTime; // Negative z-axis
        rb.MovePosition(rb.position + forwardMovement);

        // Tilt control for Android
        if (Input.acceleration != Vector3.zero)
        {
            Vector3 tiltInput = Quaternion.Euler(90, 0, 0) * Input.acceleration;
            Vector3 targetRotation = Quaternion.Euler(0, Mathf.Atan2(tiltInput.x, tiltInput.y) * Mathf.Rad2Deg, 0).eulerAngles; // Adjusted for y-axis tilt
            Quaternion tiltRotation = Quaternion.Slerp(rb.rotation, Quaternion.Euler(targetRotation), tiltSensitivity * Time.deltaTime);
            rb.MoveRotation(tiltRotation);
        }

        // Horizontal movement
        float horizontalInput = Input.GetAxis("Horizontal"); // Get input from keyboard or touch controls
        Vector3 horizontalMovement = transform.right * horizontalInput * horizontalSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + horizontalMovement);
    }
}
