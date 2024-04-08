using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceFromSolHandler : MonoBehaviour
{
    public Transform origin; // Reference to the original position
    private Vector3 originalPosition; // Original position of the GameObject
    public string displayText = "Distance from sol is xxx parsecs";
    public Text text;
    public float updateInterval = 1f; // Set the delay in seconds

    // Start is called before the first frame update
    void Start()
    {
        // Save the original position
        originalPosition = origin.position;
        StartCoroutine(UpdateDisplayText());
    }

    private IEnumerator UpdateDisplayText()
    {
        // Loop infinitely
        while (true)
        {
            // Calculate the distance between current position and original position
            float distance = Vector3.Distance(origin.position, originalPosition);

            string[] parts = displayText.Split(new string[] { "xxx" }, StringSplitOptions.None);
            string output = parts[0] + distance.ToString("F2") + parts[1];
            text.text = output;

            // Wait for the specified interval before calculating distance again
            yield return new WaitForSeconds(updateInterval);
        }
    }

}
