using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdControllerAssigner : MonoBehaviour
{
    public BirdController assignedBirdController; // Public variable to show the assigned BirdController
    public GameObject reviveUIObject; // Reference to the ReviveUIObject

    private void Start()
    {
        BirdController[] birdControllers = FindObjectsOfType<BirdController>();

        if (birdControllers.Length > 0)
        {
            assignedBirdController = birdControllers[0]; // Assign the first found BirdController
            Debug.Log("BirdController script assigned to this GameObject.");
        }
        else
        {
            Debug.LogError("BirdController script not found in the scene.");
        }

        CheckBirdPower(); // Check the Bird's power when the script starts
    }

    private void Update()
    {
        CheckBirdPower(); // Continuously check the Bird's power during the game
    }

    private void CheckBirdPower()
    {
        if (assignedBirdController != null)
        {
            if (assignedBirdController.GetCurrentPower() <= 0)
            {
                // Activate the ReviveUIObject when Bird's power is zero
                reviveUIObject.SetActive(true);
            }
            else
            {
                // Deactivate the ReviveUIObject when Bird's power is not zero
                reviveUIObject.SetActive(false);
            }
        }
    }

    public void ReviveBird()
    {
        if (assignedBirdController != null)
        {
            assignedBirdController.Revive(); // Call the Revive function from the assigned BirdController
        }
    }
}
