using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdController : MonoBehaviour
{
    public float forwardSpeed = 5f;
    public float horizontalSpeed = 2f;
    public float verticalSpeed = 2f;
    public float rotationSpeed = 5f;
    public float rotationPitchAngle = 15f;
    public float boostMultiplier = 2f;
    public float normalFOV = 60f;
    public float boostedFOV = 75f;
    public AnimationCurve fovCurve;
    public GameObject speedUpParticlePrefab;
    public GameObject healthParticlePrefab;
    public AudioSource HayyanCollectSound;
    public AudioSource TreeCrushed;
    public AudioSource HardPropsCrushedSound;
    public AudioSource HealthTriggerSound;
    public AudioSource HeartTriggerSound;
    public FixedJoystick JoyStick;

    public Camera birdCamera;
    public AudioSource speedUpAudioSource;

    public int maxPower = 100; // Maximum power
    [SerializeField]
    public int currentPower; // Current power

    public LayerMask groundLayer; // Layer mask for detecting ground (set in the Unity Inspector)

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

        currentPower = maxPower; // Initialize power
    }

    private void Update()
    {
        // Detect the ground distance
        DetectGroundDistance();

        if (isSpeedPowerUpActive && Time.time >= speedPowerUpEndTime)
        {
            isBoosting = false;
            isSpeedPowerUpActive = false;
            birdCamera.fieldOfView = normalFOV;
            speedUpAudioSource.Stop();
            speedUpParticle.Stop();
        }

        if (!isSpeedPowerUpActive && speedUpParticle != null && speedUpParticle.isPlaying)
        {
            speedUpParticle.Stop();
        }

        transform.Translate(Vector3.back * forwardSpeed * (isBoosting ? boostMultiplier : 1f) * Time.deltaTime);

        float horizontalInput = -Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 newPosition = new Vector3(transform.position.x + horizontalInput * horizontalSpeed * Time.deltaTime,
                                          transform.position.y + verticalInput * verticalSpeed * Time.deltaTime,
                                          transform.position.z);

        transform.position = newPosition;

        Quaternion targetRotation = Quaternion.Euler(-verticalInput * rotationPitchAngle, transform.rotation.eulerAngles.y, -horizontalInput * rotationSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        if (isBoosting)
        {
            birdCamera.fieldOfView = boostedFOV;
        }
    }

    private void DetectGroundDistance()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            float groundDistance = hit.distance;

            // You can now use the groundDistance variable as needed.
            // For example, you can print it to the console:
            Debug.Log("Ground Distance: " + groundDistance);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpeedPowerUp"))
        {
            isBoosting = true;
            isSpeedPowerUpActive = true;
            speedPowerUpEndTime = Time.time + 3f;
            birdCamera.fieldOfView = boostedFOV;

            if (speedUpAudioSource != null)
            {
                speedUpAudioSource.Play();
            }

            if (speedUpParticle != null)
            {
                speedUpParticle.gameObject.SetActive(true);
                speedUpParticle.Play();
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            TakeDamage(10); // Bird takes 10 power damage when colliding with an enemy
        }
        else if (other.CompareTag("Tree"))
        {
            TreeCrushed.Play();
            TakeDamage(5); // Bird takes 20 power damage when colliding with a tree
        }
        else if (other.CompareTag("HardProps"))
        {
            HardPropsCrushedSound.Play();
            TakeDamage(20); // Bird takes 20 power damage when colliding with a tree
        }
        else if (other.CompareTag("Power"))
        {
            IncreasePower(50); // Bird increases power by 50 when going through a power object
            Destroy(other.gameObject); // Destroy the power object after collecting it
        }
        else if (other.CompareTag("Hayyan"))
        {
            HayyanCollectSound.Play();
            IncreasePower(1); // Bird increases power by 50 when going through a power object
            CurrencyManager.Instance.AddHayyanCurrency(1);
            Destroy(other.gameObject);
            
        }
        else if (other.CompareTag("Health"))
        {
            IncreasePower(30);
            HealthTriggerSound.Play();
            CreateHealthParticleEffect(other.transform.position); // Create health particle effect
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("HeartCoin"))
        {
            
            HeartTriggerSound.Play();
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        currentPower -= damage;

        if (currentPower <= 0)
        {
            gameObject.SetActive(false); // Deactivate the bird when power reaches zero
        }
    }

    public void IncreasePower(int amount)
    {
        currentPower += amount;

        // Ensure that power does not exceed the maximum
        currentPower = Mathf.Clamp(currentPower, 0, maxPower);
    }

    public int GetCurrentPower()
    {
        return currentPower;
    }

    private void CreateHealthParticleEffect(Vector3 position)
    {
        if (healthParticlePrefab != null)
        {
            Instantiate(healthParticlePrefab, position, Quaternion.identity);
        }
    }

}








