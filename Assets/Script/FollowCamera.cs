using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // The target to follow (your bird)
    public float distance = 10f; // Distance from the target
    public Vector3 offset = new Vector3(0f, 2f, 0f); // Offset from the target

    public float smoothSpeed = 0.125f; // Smoothing speed for camera movement

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is not set for the FollowCamera script.");
            return;
        }

        // Calculate the desired camera position
        Vector3 desiredPosition = target.position - target.forward * distance + offset;

        // Smoothly move the camera towards the desired position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

        // Make the camera look at the target (bird)
        transform.LookAt(target);
    }
}
