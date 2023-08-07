using UnityEngine;

public class BirdController : MonoBehaviour
{
    public float forwardSpeed = 5f;
    public float horizontalSpeed = 2f;
    public float verticalSpeed = 2f;
    public float boostMultiplier = 2f;
    public float normalFOV = 60f;
    public float boostedFOV = 75f;
    public AnimationCurve fovCurve; // AnimationCurve to control FOV change

    public Camera birdCamera; // Reference to the camera object

    private bool isBoosting = false;
    private float fovChangeStartTime;
    private float initialFOV;
    private float targetFOV;

    private void Start()
    {
        if (birdCamera == null)
        {
            Debug.LogError("Bird Camera is not assigned!");
            return;
        }

        initialFOV = birdCamera.fieldOfView;
        targetFOV = normalFOV;
    }

    private void Update()
    {
        // Move the bird forward in the -z direction
        transform.Translate(Vector3.back * forwardSpeed * (isBoosting ? boostMultiplier : 1f) * Time.deltaTime);

        // Get input for horizontal (left and right) movement
        float horizontalInput = -Input.GetAxis("Horizontal"); // Switched the sign here
        // Get input for vertical (up and down) movement
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the new position based on input and speeds
        Vector3 newPosition = new Vector3(transform.position.x + horizontalInput * horizontalSpeed * Time.deltaTime,
                                          transform.position.y + verticalInput * verticalSpeed * Time.deltaTime,
                                          transform.position.z);

        // Apply the new position
        transform.position = newPosition;

        // Speed up when right Shift is pressed
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            isBoosting = true;
            fovChangeStartTime = Time.time;
            initialFOV = birdCamera.fieldOfView;
            targetFOV = boostedFOV;
        }
        if (Input.GetKeyUp(KeyCode.RightShift))
        {
            isBoosting = false;
            fovChangeStartTime = Time.time;
            initialFOV = birdCamera.fieldOfView;
            targetFOV = normalFOV;
        }

        // Smoothly change FOV based on the curve
        float timeSinceStart = Time.time - fovChangeStartTime;
        float fovPercentage = Mathf.Clamp01(timeSinceStart / 0.5f); // Adjust the duration as needed
        float curveValue = fovCurve.Evaluate(fovPercentage);
        birdCamera.fieldOfView = Mathf.Lerp(initialFOV, targetFOV, curveValue);
    }
}
