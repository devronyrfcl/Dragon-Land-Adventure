using UnityEngine;

public class GarageCamera : MonoBehaviour
{
    public Transform target; // The target to follow (e.g., the car)
    public float smoothSpeed = 0.125f; // Smoothing factor for camera movement
    public Vector3 offset; // Offset from the target's position
    public float zoomSpeed = 2.0f; // Speed of zooming in and out
    public float minZoom = 5.0f; // Minimum zoom distance
    public float maxZoom = 15.0f; // Maximum zoom distance

    private float currentZoom = 10.0f;

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("No target assigned to the camera!");
            return;
        }

        // Zoom in and out based on user input (e.g., pinch gesture)
        float zoomDelta = -Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom + zoomDelta, minZoom, maxZoom);

        Vector3 desiredPosition = target.position + offset + Vector3.back * currentZoom;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}
