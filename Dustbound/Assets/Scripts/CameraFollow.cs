using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target & Smoothing")]
    public Transform target;           // The player object to follow
    public float smoothTime = 0.25f;   // Lower = snappier, Higher = floatier
    public Vector3 offset = new Vector3(0f, 0f, -10f); // Keeps camera at Z = -10

    private Vector3 velocity = Vector3.zero;

    // LateUpdate runs after all Update and physics calculations
    void LateUpdate()
    {
        if (target == null) return;

        // Define the target position while maintaining the Z offset
        Vector3 targetPosition = target.position + offset;

        // Smoothly move the camera toward the target position
        transform.position = Vector3.SmoothDamp(
            transform.position, 
            targetPosition, 
            ref velocity, 
            smoothTime
        );
    }
}
