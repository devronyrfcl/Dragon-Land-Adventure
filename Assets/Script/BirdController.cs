using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdController : MonoBehaviour
{
    public float forwardSpeed = 5f;
    public float horizontalSpeed = 2f;
    public float verticalSpeed = 2f;
    public float rotationSpeed = 5f; // New rotation speed parameter
    public float rotationPitchAngle = 15f; // Angle for pitch rotation
    public float boostMultiplier = 2f;
    public float normalFOV = 60f;
    public float boostedFOV = 75f;
    public AnimationCurve fovCurve; // AnimationCurve to control FOV change
    public GameObject speedUpParticlePrefab;

    public Camera birdCamera; // Reference to the camera object
    public AudioSource speedUpAudioSource; // Reference to the external AudioSource GameObject

    private bool isBoosting = false;
    private bool isSpeedPowerUpActive = false;
    private float speedPowerUpEndTime;
    private float initialFOV;
    private ParticleSystem speedUpParticle;

    private void Start()
    {
        if (birdCamera == null)
        {
            Debug.LogError("Bird Camera is not assigned!");
            return;
        }

        initialFOV = birdCamera.fieldOfView;

        speedUpParticle = Instantiate(speedUpParticlePrefab, transform).GetComponent<ParticleSystem>();
        speedUpParticle.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Check if speed power-up is active and should end
        if (isSpeedPowerUpActive && Time.time >= speedPowerUpEndTime)
        {
            isBoosting = false;
            isSpeedPowerUpActive = false;
            birdCamera.fieldOfView = normalFOV;
            speedUpAudioSource.Stop();
            speedUpParticle.Stop();
        }

        // Disable the SpeedUp particle when boost is over
        if (!isSpeedPowerUpActive && speedUpParticle != null && speedUpParticle.isPlaying)
        {
            speedUpParticle.Stop();
        }

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

        // Rotate the bird slightly based on horizontal and vertical input
        Quaternion targetRotation = Quaternion.Euler(-verticalInput * rotationPitchAngle, transform.rotation.eulerAngles.y, -horizontalInput * rotationSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // Apply boost effect if speed power-up is active
        if (isBoosting)
        {
            birdCamera.fieldOfView = boostedFOV;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpeedPowerUp"))
        {
            isBoosting = true;
            isSpeedPowerUpActive = true;
            speedPowerUpEndTime = Time.time + 3f; // Speed up for 3 seconds
            birdCamera.fieldOfView = boostedFOV;

            // Play sound using the external AudioSource
            if (speedUpAudioSource != null)
            {
                speedUpAudioSource.Play();
            }

            // Activate particle
            if (speedUpParticle != null)
            {
                speedUpParticle.gameObject.SetActive(true);
                speedUpParticle.Play();
            }
        }
    }
}
