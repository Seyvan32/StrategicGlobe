using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target; // Reference to the Earth model
    [SerializeField] private float distance = 700f; // Distance from the Earth
    [SerializeField] private float rotationSpeed = 12f; // Speed of camera rotation
    [SerializeField] private float zoomSpeed = 50f; // Speed of zooming
    [SerializeField] private float minDistance = 650f; // Minimum zoom distance
    [SerializeField] private float maxDistance = 800f; // Maximum zoom distance
    [SerializeField] private float dragSpeedMin = 0.012f; // Minimum drag speed
    [SerializeField] private float dragSpeedMax = 0.2f; // Maximum drag speed

    private float yaw = 0f; // Horizontal rotation
    private float pitch = 0f; // Vertical rotation

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target is not assigned to the CameraOrbit script!");
            return;
        }

        // Initialize yaw and pitch based on the initial position
        Vector3 offset = transform.position - target.position;
        yaw = Mathf.Atan2(offset.x, offset.z) * Mathf.Rad2Deg;
        pitch = Mathf.Asin(offset.y / distance) * Mathf.Rad2Deg;
    }

    void Update()
    {
        if (target == null) return;

        // Get input for horizontal and vertical rotation
        float horizontal = Input.GetAxis("Horizontal"); // A (1) and D (-1)
        float vertical = Input.GetAxis("Vertical"); // W (1) and S (-1)

        // Adjust yaw and pitch based on input
        yaw -= horizontal * rotationSpeed * Time.deltaTime;
        pitch += vertical * rotationSpeed * Time.deltaTime;

        // Handle mouse dragging for rotation
        if (Input.GetMouseButton(0)) // Left mouse button held down
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Dynamically calculate drag speed based on distance
            float dragSpeed = Mathf.Lerp(dragSpeedMin, dragSpeedMax, (distance - minDistance) / (maxDistance - minDistance));

            yaw += mouseX * dragSpeed * rotationSpeed;
            pitch -= mouseY * dragSpeed * rotationSpeed;
        }

        // Clamp pitch to prevent flipping
        pitch = Mathf.Clamp(pitch, -89f, 89f);

        // Zoom in/out using the mouse scroll wheel
        float scrollInput = Input.GetAxis("Mouse ScrollWheel"); // Scroll up (positive) and down (negative)
        distance -= scrollInput * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // Calculate the new position of the camera
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 direction = rotation * Vector3.forward;
        transform.position = target.position - direction * distance;

        // Make the camera look at the target
        transform.LookAt(target);
    }
}