using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercamera : MonoBehaviour
{
    public player playerScript;
    public Transform playerTransform;
    public float sensitivity = 2.0f; // Mouse sensitivity
    public float clampAngle = 80.0f; // Maximum vertical angle
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    private void Start()
    {
        Vector3 playerRotation = playerTransform.localRotation.eulerAngles;
        rotationY = playerRotation.y;
        rotationX = transform.localRotation.eulerAngles.x;
    }

    private void Update()
    {
        // Rotate the camera based on mouse input
        if(!playerScript.isRotating){
            RotateCameraWithMouse();

            // Update the camera rotation to align with the player's orientation
            AlignCameraWithPlayer();

        }
    }

    private void RotateCameraWithMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        rotationY += mouseX;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -clampAngle, clampAngle);

        // Apply rotations to camera and player
        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
        // playerTransform.localRotation = Quaternion.Euler(0, rotationY, 0);
    }

    private void AlignCameraWithPlayer()
    {
        // Align the camera's forward direction with the player's forward direction
        transform.forward = playerTransform.forward;
    }
}
