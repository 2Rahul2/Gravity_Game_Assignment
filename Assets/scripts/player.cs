using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public float rotationSpeed = 90f; 
    public bool isRotating = false,frontface=false;
    float zRotation ,xRotation;
    float angle, r ,xangle ,r1;
    private Rigidbody rb;


    public Camera mainCamera;


    public Vector3 targetRotation = new Vector3(180f, 0f, 0f);
    private Quaternion startRotation;
    private Quaternion targetQuaternion;


    public float moveSpeed = 5.0f; 
    private Vector3 moveDirection; 
    public Transform childObject;


    public float sensitivity = 2.0f; 
    public float verticalSensitivity = 2.0f; 
    public float clampAngle = 80.0f; 

    private float rotationX = 0.0f;

    private Transform cameraTransform;

    public Animator animator;


    public Transform FootPosition;
    public LayerMask groundLayer;
    private void Start() {
        cameraTransform = Camera.main.transform;
        startRotation = transform.rotation;
        targetQuaternion = Quaternion.Euler(targetRotation);
        // transform.Rotate(Vector3.up, 180, Space.World);
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        // Rotate the player around the y-axis
        // transform.Rotate(Vector3.up, mouseX);
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement direction
        moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Move the player based on input
        MovePlayer();
        // Check for arrow key presses
        if(Input.GetKeyUp(KeyCode.DownArrow) && !isRotating){
            frontface = true;
            isRotating = true;
            xRotation += 90f;
            if(xRotation>360){
                xRotation = 90f;
            }
            targetQuaternion = Quaternion.Euler(new Vector3(xRotation, 0f, 0f));
            print(xRotation);
        }
        if(Input.GetKeyUp(KeyCode.UpArrow) && !isRotating){
            
            frontface = true;
            isRotating = true;
            xRotation = NormalizeAngle((xRotation - 90));
            if(xRotation<= -360){   
                xRotation = 0;
            }
            targetQuaternion = Quaternion.Euler(new Vector3(xRotation, 0f, 0f));

        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) && !isRotating)
        {
            // RotatePlayer(-90f); // Rotate clockwise
            isRotating = true;
            zRotation = NormalizeAngle((zRotation - 90));
            // print("zrotating:  " + NormalizeAngle(zRotation));
            if(zRotation<= -360){   
                zRotation = 0;
            }
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow) && !isRotating)
        {

            isRotating = true;
            zRotation += 90f;
            print(zRotation);
            if(zRotation>360){
                print("default");
                zRotation = 90;
            print(zRotation);

            }
            // RotatePlayer(90f); // Rotate counterclockwise
        }
        if(isRotating){
             animator.SetBool("fall", true);
            animator.SetBool("run", false);    
            animator.SetBool("idle", false);
            if(!frontface){
                RotatePlayer(zRotation); // Rotate counterclockwise
            }else{
                FrontRotatePlayer(xRotation);
            }
        }else{
            if (Physics.Raycast(FootPosition.position, -transform.up, out RaycastHit hit, 1f, groundLayer))
            {
                print("grounded");
                animator.SetBool("fall", false);
                if(horizontalInput != 0 || verticalInput != 0){
                // if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.D)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.S)){
                    animator.SetBool("idle", false);    
                    animator.SetBool("run", true);    

                }else{
                    animator.SetBool("idle", true);
                }
                
                
            }
            
            frontface = false;

            float mouseX = Input.GetAxis("Mouse X") * 2.0f;
            float mouseY = Input.GetAxis("Mouse Y") * verticalSensitivity;
            // print(horizontalInput);
            rotationX += mouseY;
            rotationX = Mathf.Clamp(rotationX, -clampAngle, clampAngle);

            childObject.localRotation = Quaternion.Euler(rotationX, childObject.localRotation.eulerAngles.y + mouseX, 0);
            

            rb.AddForce(-transform.up * Physics.gravity.magnitude, ForceMode.Acceleration);
            // transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x + mouseX ,transform.localRotation.eulerAngles.y , transform.localRotation.eulerAngles.z);
            // Vector3 cameraForward = cameraTransform.forward;

            // // Ignore the y component to make the player look horizontally
            // cameraForward.y = 0f;
            // cameraForward.Normalize();

            // // Set the player's rotation to match the camera's forward direction
            
            // transform.forward = cameraForward;
        }
    }
    private void MovePlayer()
    {
        // Calculate the desired movement amount
        Vector3 movementAmount = moveDirection * moveSpeed * Time.deltaTime;

        // Move the player using Transform.Translate
        transform.Translate(movementAmount);
    }
    float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0f){
            angle += 360f;
        }
        return angle;
    }
    void FrontRotatePlayer(float rotationAngle){
        // float targetRotation = transform.eulerAngles.x + rotationAngle;

        Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetQuaternion, rotationSpeed * Time.deltaTime);
        transform.rotation = newRotation;
        if (Quaternion.Angle(transform.rotation, targetQuaternion) < 1f)
        {
            transform.rotation = targetQuaternion;
            isRotating = false;
            frontface = false;
            AlignPlayerWithNewFloor();

        }

    }
    void AlignCameraWithPlayer()
    {
        // Align the camera's forward direction with the player's forward direction
        mainCamera.transform.forward = transform.forward;
    }

    void RotatePlayer(float rotationAngle)
    {
        float targetRotation = transform.eulerAngles.z + rotationAngle;
        while (Mathf.Abs(NormalizeAngle(transform.eulerAngles.z) - zRotation) > 0.5f)
        {

            angle = Mathf.SmoothDampAngle(NormalizeAngle(transform.eulerAngles.z), rotationAngle, ref r, 0.1f);
            transform.rotation = Quaternion.Euler(0, 0, angle);
            return;
        }

        // Complete the rotation
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);
        isRotating = false;

        // After rotation, adjust the player's position to align with the new "floor"
        AlignPlayerWithNewFloor();
        AlignCameraWithPlayer();
    }

    void AlignPlayerWithNewFloor()
    {
        // Calculate the new "up" direction based on the player's rotation
        Vector3 newUpDirection = transform.up;

        // Calculate the new position by moving the player slightly up
        Vector3 newPosition = transform.position + (newUpDirection * 0.1f);

        // Set the new position for the player
        transform.position = newPosition;
    }
}
