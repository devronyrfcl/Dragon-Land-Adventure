using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    public float rotationSpeed = 1.0f;

    void Update()
    {
        // Get the current skybox material
        Material skyboxMaterial = RenderSettings.skybox;

        // Calculate the new rotation based on time and speed
        float rotationAngle = Time.time * rotationSpeed;

        // Set the new rotation to the skybox material
        skyboxMaterial.SetFloat("_Rotation", rotationAngle);
    }
}
