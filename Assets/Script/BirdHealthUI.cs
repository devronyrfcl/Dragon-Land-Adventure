using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BirdHealthUI : MonoBehaviour
{
    private BirdController birdController; // Reference to the BirdController script
    public TMP_Text powerText; // Reference to the TextMeshPro Text component
    public Slider powerSlider; // Reference to the Slider component

    private void Start()
    {
        // Find the BirdController script in the scene
        birdController = FindObjectOfType<BirdController>();

        if (birdController == null)
        {
            Debug.LogError("BirdController not found in the scene.");
            return;
        }

        // Optional: You can also set the max value of the powerSlider here if it's based on the bird's max power.
        powerSlider.maxValue = birdController.maxPower;
    }

    private void Update()
    {
        if (birdController != null)
        {
            // Update the TextMeshPro text to display the bird's current power
            powerText.text = "Power: " + birdController.GetCurrentPower().ToString();

            // Update the Slider value based on the bird's current power
            powerSlider.value = birdController.GetCurrentPower();
        }
        else
        {
            // Handle the case when the birdController reference is not found
            powerText.text = "Power: N/A";
            powerSlider.value = 0f;
        }
    }
}
