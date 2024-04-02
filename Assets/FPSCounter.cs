using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public TextMesh fpsText; // Reference to the UI Text object to display FPS

    private int frames; // Number of frames since the last update
    private float timeSinceUpdate; // Time elapsed since the last update

    private void Start()
    {
        // Ensure fpsText is assigned in the Inspector
        if (fpsText == null)
        {
            Debug.LogError("Please assign a Text UI object to the 'fpsText' field in the Inspector for this script.");
        }
    }

    private void Update()
    {
        frames++;
        timeSinceUpdate += Time.deltaTime;

        // Update FPS at a set interval (e.g., every second)
        if (timeSinceUpdate >= 1.0f)
        {
            float fps = frames / timeSinceUpdate;
            string fpsString = $"FPS: {fps:F2}"; // Format to two decimal places

            // Update the UI Text with the formatted FPS string
            if (fpsText != null)
            {
                fpsText.text = fpsString;
            }

            // Reset for next calculation
            frames = 0;
            timeSinceUpdate = 0.0f;
        }
    }
}
