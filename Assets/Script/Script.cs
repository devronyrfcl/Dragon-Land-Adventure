using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        // Nothing needed here for this example
    }

    // Update is called once per frame
    void Update()
    {
        // Nothing needed here for this example
    }

    // OnCollisionEnter is called when the GameObject with this script collides with another GameObject
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided GameObject has the "Tree" tag
        if (collision.gameObject.CompareTag("Tree"))
        {
            // Show debug output
            Debug.Log("Player collided with the Tree!");
        }
    }
}