using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonControl : MonoBehaviour
{
    public float mouseSensitivity = 200f;
    public Transform playerBody;
    //public RectTransform cursorDot; // Reference to the UI dot, set in the inspector
    float xRotation = 0f;
    public bool canRotate = true;

    public float maxXRotation = 70f;
    public float minXRotation = -50f;
    public bool useSmoothDamping = true; // Toggle smoothing on or off
    public float smoothTime = 0.08f; // Damping time for smoothing (shorter is more responsive)
    private Vector2 currentRotation; // Stores smoothed rotation values
    private Vector2 currentRotationVelocity; // Stores smoothing velocity

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // Hide the default system cursor
    }
    public void SetRotationEnabled(bool enabled)
    {
        canRotate = enabled;
    }

    void Update()
    {
        if (canRotate)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, minXRotation, maxXRotation);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            playerBody.Rotate(Vector3.up * mouseX);
        }

        // Ensure the cursor dot remains visible at the center of the screen
        /*if (cursorDot != null)
        {
            cursorDot.gameObject.SetActive(true);
        }*/
    }

    /*void Update()
    {
        if (canRotate)
        {
            // Get raw mouse input
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 0.01f; // Scaled for consistency
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 0.01f;

            // Update and clamp the vertical rotation
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, minXRotation, maxXRotation);

            if (useSmoothDamping)
            {
                // Smoothly interpolate the rotation
                currentRotation = Vector2.SmoothDamp(
                    currentRotation,
                    new Vector2(xRotation, playerBody.eulerAngles.y + mouseX),
                    ref currentRotationVelocity,
                    smoothTime
                );

                // Apply the smoothed rotation
                transform.localRotation = Quaternion.Euler(currentRotation.x, 0f, 0f);
                playerBody.localRotation = Quaternion.Euler(0f, currentRotation.y, 0f);
            }
            else
            {
                // Apply rotation directly
                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                playerBody.Rotate(Vector3.up * mouseX);
            }
        }

        // Keep the cursor dot visible if assigned
        if (cursorDot != null)
        {
            cursorDot.gameObject.SetActive(true);
        }
    }

    // Toggle cursor visibility and lock state
    public void ToggleCursor(bool isVisible)
    {
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isVisible;

        if (cursorDot != null)
        {
            cursorDot.gameObject.SetActive(isVisible);
        }
    }*/
}
