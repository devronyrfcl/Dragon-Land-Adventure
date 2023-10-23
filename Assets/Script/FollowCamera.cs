using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public float distance = 10f; // Distance from the target
    public Vector3 offset = new Vector3(0f, 2f, 0f); // Offset from the target
    public float smoothSpeed = 0.125f; // Smoothing speed for camera movement

    private Vector3 velocity = Vector3.zero;
    private Transform target;

    private bool isShaking = false;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.1f;
    public float currentShakeDuration = 0f;

    void Start()
    {
        // Find the enabled object with the "Player" tag in the scene and set it as the initial target
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // Assuming you've tagged your player object as "Player" in the Unity editor
        if (player != null && player.activeInHierarchy)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogWarning("No active object with the 'Player' tag found in the scene.");
        }
    }

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is not set for the FollowCamera script.");
            return;
        }

        if (isShaking)
        {
            ShakeCamera();
        }

        // Calculate the desired camera position
        Vector3 desiredPosition = target.position - target.forward * distance + offset;

        // Smoothly move the camera towards the desired position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

        // Make the camera look at the target
        transform.LookAt(target);
    }

    // Function to initiate a camera shake with a specified duration and magnitude
    public void StartShake(float duration, float magnitude)
    {
        isShaking = true;
        currentShakeDuration = duration;
        shakeMagnitude = magnitude;
    }

    // Function to handle the camera shake
    public void ShakeCamera()
    {
        if (currentShakeDuration > 0)
        {
            Vector2 shakeOffset = Random.insideUnitCircle * shakeMagnitude;
            transform.position += new Vector3(shakeOffset.x, shakeOffset.y, 0);
            currentShakeDuration -= Time.deltaTime;
        }
        else
        {
            isShaking = false;
            currentShakeDuration = 0f;
            // Reset the camera position to the target's position
            transform.position = target.position - target.forward * distance + offset;
        }
    }
}
