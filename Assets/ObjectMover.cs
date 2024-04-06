using System.Collections;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public GameObject playerController; // Reference to the GameObject whose position will be the initial position
    public Vector3 targetPosition; // The target position to move towards
    public Vector3 targetRotationEulerAngles; // The target rotation in Euler angles (x, y, z)
    public float duration = 2.0f; // The duration of the movement
    public ConstellationRenderer ConstellationRenderer;
    public string highlightedConstellation = "Aquarius";

    private bool isMoving = false; // Flag to check if movement is in progress

    private Vector3 initialPosition; // Initial position of the object
    private Quaternion initialRotation; // Initial rotation of the object

    public bool activate = false;

    // aquarius -0.09776417, -1.119881, -0.0240202
    private void Start()
    {
        initialPosition = playerController.transform.position;
        initialRotation = playerController.transform.rotation;
        if (activate)
        {
            Invoke("StartMovementToTargetConstellation", 10f);
            Invoke("ResetConstellation", 20f);
            //Invoke("ResetPosition", 25f);
            //Invoke("ResetRotation", 35f);
        }
    }

    public void StartMovementToTargetConstellation()
    {
        if (!isMoving)
        {
            ConstellationRenderer.ModifyConstellation(highlightedConstellation);
            StartCoroutine(MoveObject(playerController.transform.position, playerController.transform.rotation, targetPosition, Quaternion.Euler(targetRotationEulerAngles)));
        }
    }

    public void ResetConstellation()
    {
        if (!isMoving)
        {
            ConstellationRenderer.ResetConstellation(highlightedConstellation);
            //StartCoroutine(MoveObject(playerController.transform.position, playerController.transform.rotation, targetPosition, Quaternion.Euler(targetRotationEulerAngles)));
        }
    }

    public void ResetPosition()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveObject(playerController.transform.position, playerController.transform.rotation, initialPosition, playerController.transform.rotation));
        }
    }

    public void ResetRotation()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveObject(playerController.transform.position, playerController.transform.rotation, playerController.transform.position, initialRotation));
        }
    }

    IEnumerator MoveObject(Vector3 currentPosition, Quaternion currentRotation, Vector3 targetPosition, Quaternion targetRotation)
    {
        isMoving = true; // Set flag to indicate movement is in progress

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate the interpolation ratio based on elapsed time and duration
            float t = elapsedTime / duration;

            // Use Vector3.Lerp to smoothly interpolate between initial and target positions
            playerController.transform.position = Vector3.Lerp(currentPosition, targetPosition, t);

            // Use Quaternion.Slerp to smoothly interpolate between initial and target rotations
            playerController.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, t);

            // Increment elapsed time by the time passed since the last frame
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the object reaches the target position and rotation precisely
        playerController.transform.position = targetPosition;
        playerController.transform.rotation = targetRotation;

        isMoving = false; // Reset the flag after movement is complete
    }
}
