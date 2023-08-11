using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cameraOrientation : MonoBehaviour
{

    // public CinemachineFreeLook wallFreeLookCamera;
    // public Transform playerTransform;

    // private void Start()
    // {
    //     if (wallFreeLookCamera == null)
    //     {
    //         Debug.LogError("WallCameraController: Wall FreeLookCamera is not assigned!");
    //         return;
    //     }

    //     if (playerTransform == null)
    //     {
    //         Debug.LogError("WallCameraController: Player Transform is not assigned!");
    //         return;
    //     }
    // }

    // private void Update()
    // {
    //     // Calculate the direction from the player to the camera (camera looks at the player).
    //     Vector3 lookDirection = playerTransform.position - wallFreeLookCamera.transform.position;

    //     // Remove the vertical component from the lookDirection to ensure rotation only happens around the y-axis.
    //     lookDirection.y = 0f;

    //     // If the lookDirection is not zero, rotate the camera to look at the player.
    //     if (lookDirection != Vector3.zero)
    //     {
    //         Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
    //         wallFreeLookCamera.transform.rotation = targetRotation;
    //     }

    //     // Calculate the offset vector to adjust the Follow position.
    //     Vector3 offset = playerTransform.up * 2f + playerTransform.forward * -4f;

    //     // Update the Follow and LookAt positions of the wall camera.
    //     wallFreeLookCamera.Follow = playerTransform.position + offset;
    //     wallFreeLookCamera.LookAt = playerTransform.position;
    // }
}
