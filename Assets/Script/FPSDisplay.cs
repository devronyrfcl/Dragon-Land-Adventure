using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    public Text fpsText;

    private float updateInterval = 0.5f; // Update FPS every 0.5 seconds
    private float lastUpdateTime;
    private int frames = 0;

    private void Start()
    {
        lastUpdateTime = Time.realtimeSinceStartup;

        // Set the target frame rate to 60 FPS
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        frames++;

        float timeSinceLastUpdate = Time.realtimeSinceStartup - lastUpdateTime;
        if (timeSinceLastUpdate >= updateInterval)
        {
            float fps = frames / timeSinceLastUpdate;

            // Update the Text UI element with the FPS
            fpsText.text = "FPS: " + fps.ToString("F2");

            frames = 0;
            lastUpdateTime = Time.realtimeSinceStartup;
        }
    }
}
