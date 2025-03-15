using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // The player's transform
    public float smoothSpeed = 0.1f;  // Adjusted for smoother motion
    public Vector3 offset;  // Offset distance from the player
    public float distanceThreshold = 0.5f;  // How far the player must move before camera starts adjusting

    private Vector3 targetPosition;  // The camera’s target position

    void Start()
    {
        targetPosition = transform.position;  // Start at the camera's initial position
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        desiredPosition.z = -10f;  // Lock Z position

        // Check if the player has moved beyond the threshold
        if (Vector3.Distance(transform.position, desiredPosition) > distanceThreshold)
        {
            targetPosition = desiredPosition;  // Update target position when player moves far enough
        }

        // Smoothly transition towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
}
