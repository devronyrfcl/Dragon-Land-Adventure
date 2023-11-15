using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BirdController : MonoBehaviour
{
    public GameObject PlayerModel;
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
    public GameObject heartParticlePrefab;
    public GameObject deathParticlePrefab;
    /*public AudioSource HayyanCollectSound;
    public AudioSource TreeCrushed;
    public AudioSource HardPropsCrushedSound;
    public AudioSource HealthTriggerSound;
    public AudioSource HeartTriggerSound;
    public AudioSource speedUpAudioSource;*/
    private DynamicJoystick JoyStick;
    public float powerDecreaseRate = 5f; // The rate at which power decreases per second
    public FollowCamera followCamera;
    public TextMeshProUGUI distanceText; // Reference to the TextMeshPro text component


    public Camera birdCamera;
    

    public int maxPower = 100; // Maximum power
    [SerializeField]
    public int currentPower; // Current power

    public LayerMask groundLayer; // Layer mask for detecting ground (set in the Unity Inspector)
    public GameObject ReviveUIPanel;

    private bool isBoosting = false;
    private bool isMovementEnabled = true; // Add this variable to control movement

    private bool isSpeedPowerUpActive = false;
    private float speedPowerUpEndTime;
    private float initialFOV;
    private ParticleSystem speedUpParticle;
    private ParticleSystem healthParticle;
    private ParticleSystem heartParticle;
    private ParticleSystem deathParticle;
    private float powerDecreaseTimer = 5f; // Timer to decrease power
    private float distanceTraveled = 0f; // Add this variable to track the distance traveled

    private void Start()
    {
        if (birdCamera == null)
        {
            Debug.LogError("Bird Camera is not assigned!");
            return;
        }

        initialFOV = birdCamera.fieldOfView;

        // Automatically find and assign the DynamicJoystick component
        JoyStick = FindObjectOfType<DynamicJoystick>();
        if (JoyStick == null)
        {
            Debug.LogError("DynamicJoystick not found in the scene!");
        }

        speedUpParticle = Instantiate(speedUpParticlePrefab, transform).GetComponent<ParticleSystem>();
        speedUpParticle.gameObject.SetActive(false);

        healthParticle = Instantiate(healthParticlePrefab, transform).GetComponent<ParticleSystem>();
        healthParticle.gameObject.SetActive(true);
        healthParticle.Stop();

        heartParticle = Instantiate(heartParticlePrefab, transform).GetComponent<ParticleSystem>();
        heartParticle.gameObject.SetActive(true);
        heartParticle.Stop();

        deathParticle = Instantiate(deathParticlePrefab, transform).GetComponent<ParticleSystem>();
        deathParticle.gameObject.SetActive(true);
        deathParticle.Stop();

        currentPower = maxPower; // Initialize power
        LoadSavedDistance();
        AudioManager.Instance.PlayMusic("BGM_Game");
    }

    private void Update()
    {
        if (isSpeedPowerUpActive && Time.time >= speedPowerUpEndTime)
        {
            isBoosting = false;
            isSpeedPowerUpActive = false;
            birdCamera.fieldOfView = normalFOV;
            //speedUpAudioSource.Stop();
            speedUpParticle.Stop();
        }
        if (!isSpeedPowerUpActive && speedUpParticle != null && speedUpParticle.isPlaying)
        {
            speedUpParticle.Stop();
        }

        // Check if movement is enabled before moving
        if (isMovementEnabled)
        {
            // Decrease power every 2 seconds
            powerDecreaseTimer -= Time.deltaTime;
            if (powerDecreaseTimer <= 0f)
            {
                TakeDamage(1); // Decrease the power by 1
                powerDecreaseTimer = 1f; // Reset the timer
            }

            float movementDistance = forwardSpeed * (isBoosting ? boostMultiplier : 1f) * Time.deltaTime;
            transform.Translate(Vector3.back * movementDistance);

            // Update the distance traveled
            distanceTraveled += movementDistance;
            if (distanceText != null) 
            {
                distanceText.text = distanceTraveled.ToString("F0"); // + " m"; // F2 formats to 2 decimal places
            }

            //float horizontalInput = -JoyStick.Horizontal;
            //float verticalInput = JoyStick.Vertical;

            
            transform.Translate(Vector3.back * forwardSpeed * (isBoosting ? boostMultiplier : 1f) * Time.deltaTime);

            float horizontalInput = -JoyStick.Horizontal;
            float verticalInput = JoyStick.Vertical;

            Vector3 newPosition = new Vector3(transform.position.x + horizontalInput * horizontalSpeed * Time.deltaTime,
                transform.position.y + verticalInput * verticalSpeed * Time.deltaTime,
                transform.position.z);

            transform.position = newPosition;

            Quaternion targetRotation = Quaternion.Euler(-verticalInput * rotationPitchAngle, transform.rotation.eulerAngles.y, -horizontalInput * rotationSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            
        }
    }


    // Function to stop bird movement
    public void StopMovement()
    {
        isMovementEnabled = false;
    }

    // Function to start bird movement
    public void StartMovement()
    {
        isMovementEnabled = true;
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
            AudioManager.Instance.PlaySFX("speedUpAudioSource");

            /*if (speedUpAudioSource != null)
            {
                speedUpAudioSource.Play();
            }*/

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
            //TreeCrushed.Play();
            AudioManager.Instance.PlaySFX("TreeCrushed");
            TakeDamage(5); // Bird takes 20 power damage when colliding with a tree
            followCamera.ShakeCamera();
        }
        else if (other.CompareTag("HardProps"))
        {
            //HardPropsCrushedSound.Play();
            AudioManager.Instance.PlaySFX("HardPropsCrushedSound");
            TakeDamage(10); // Bird takes 20 power damage when colliding with a tree
            followCamera.ShakeCamera();
        }
        else if (other.CompareTag("Power"))
        {
            IncreasePower(50); // Bird increases power by 50 when going through a power object
            Destroy(other.gameObject); // Destroy the power object after collecting it
        }
        else if (other.CompareTag("Hayyan"))
        {
            //HayyanCollectSound.Play();
            AudioManager.Instance.PlaySFX("HayyanCollectSound");
            IncreasePower(2); // Bird increases power by 50 when going through a power object
            CurrencyManager.Instance.AddHayyanCurrency(1);
            Destroy(other.gameObject);
            
        }
        else if (other.CompareTag("Health"))
        {
            IncreasePower(30);
            //HealthTriggerSound.Play();
            AudioManager.Instance.PlaySFX("Health_Trigger_Sound");
            healthParticle.Play();
            //CreateHealthParticleEffect(other.transform.position); // Create health particle effect
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("HeartCoin"))
        {
            
            //HeartTriggerSound.Play();
            AudioManager.Instance.PlaySFX("Heart_Trigger_Sound");
            heartParticle.Play();
            CurrencyManager.Instance.AddHeartCoins(1);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Terrain")) // Check for terrain collision
        {
            Die(); // Call the Die method when the bird hits terrain
            followCamera.ShakeCamera();
        }
    }

    public void TakeDamage(int damage)
    {
        currentPower -= damage;

        if (currentPower <= 0)
        {
            deathParticle.Play();
            Die();
            //gameObject.SetActive(false); // Deactivate the bird when power reaches zero
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

    public void Die()
    {
        // Set the current power (health) to zero
        currentPower = 0;

        // Deactivate the bird's game object
        PlayerModel.SetActive(false);
        deathParticle.Play();
        StopMovement();
        OnDestroy();
        ReviveUIPanel.SetActive(true);
        
        
    }

    public void Revive()
    {
        // Set the current power (health) to 100
        currentPower = 100;

        // Change the bird's position in the Y-axis by 5 units
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z + 5);
        transform.position = newPosition;

        // Activate the bird's game object
        PlayerModel.SetActive(true);
        healthParticle.Play();
        StartMovement();
        LoadSavedDistance();
    }

    private void OnDestroy()
    {
        // Save the distance traveled as a PlayerPrefs
        PlayerPrefs.SetFloat("DistanceTraveled", distanceTraveled);
        PlayerPrefs.Save();
    }
    private void LoadSavedDistance()
    {
        // Load the saved distance traveled from PlayerPrefs
        distanceTraveled = PlayerPrefs.GetFloat("DistanceTraveled", 0f);
    }

    /*public override void Teleport (Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot) {
        transform.position = pos;
        Vector3 eulerRot = rot.eulerAngles;
        float delta = Mathf.DeltaAngle (smoothYaw, eulerRot.y);
        yaw += delta;
        smoothYaw += delta;
        transform.eulerAngles = Vector3.up * smoothYaw;
        velocity = toPortal.TransformVector (fromPortal.InverseTransformVector (velocity));
        Physics.SyncTransforms ();
    }*/

}
