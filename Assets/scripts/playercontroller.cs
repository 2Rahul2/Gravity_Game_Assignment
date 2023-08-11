using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour
{
    public enum GravityDirection{
        down,
        up,
        left,
        right,
        front,
        back,
    }
    public float moveSpeed = 5f;

    private Quaternion targetRotation;
    public float rotationSpeed;
    public bool isrotate = false;
    float angle;
    float targetAngle;
    float r=0f;

    public Animator animator;

    private Rigidbody rb;
    private BoxCollider bx;
    private GravityDirection currentGravity = GravityDirection.down;
    private Vector3 currentGravityVector = Vector3.down;
    [SerializeField] private Transform pivotPlayerPos; 

    float playerRadius;
    // Start is called before the first frame update



    // public float moveSpeed = 5f;
    // public float rotationSpeed = 10f;
    // public Transform cameraTransform; // Reference to the main camera's transform.
    public Vector3 moveDirection;
    // private Rigidbody rb;


    // public float moveSpeed = 5f;
    public Transform cam;


    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;
    public Transform FootPosition;


    private Transform playerTransform;
    private Transform mainCameraTransform;

    public bool forBack;

    void Start()
    {
        playerTransform = transform;
        mainCameraTransform = Camera.main.transform;
        bx = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        playerRadius = boxCollider.size.x / 2f;

    }

    // Update is called once per frame
    void Update()
    {
        
        Debug.DrawRay(FootPosition.position, -transform.up *groundCheckDistance , Color.green);
        if(!isrotate){
            if (Physics.Raycast(FootPosition.position, -transform.up, out RaycastHit hit, groundCheckDistance, groundLayer))
            {
                print("grounded");
                rb.isKinematic = true;
                // Player is standing on the ground. Perform actions here.
            }

        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
    
        // Get the camera's forward and right directions without considering the y-component.
        Vector3 cameraForward = mainCameraTransform.forward;
        Vector3 cameraRight = mainCameraTransform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate the movement direction based on camera forward and right, and input.
        Vector3 moveDirection = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized;

        // Move the player in the desired direction.
        // We use Time.deltaTime to make the movement frame rate independent.
        // playerTransform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        playerTransform.Translate(horizontalInput * moveSpeed * Time.deltaTime, 0, verticalInput * moveSpeed * Time.deltaTime);
    
        // float horizontalInput = Input.GetAxis("Horizontal");
        // float verticalInput = Input.GetAxis("Vertical");

        // // Calculate the movement direction based on input.
        // Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // // Move the player in the desired direction.
        // // We use Time.deltaTime to make the movement frame rate independent.
        // transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        


        if(!isrotate){
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                rb.isKinematic = true;
                SwitchGravity(GravityDirection.right);
            }else if(Input.GetKeyUp(KeyCode.LeftArrow)){
                rb.isKinematic = true;

                SwitchGravity(GravityDirection.left);
            }else if(Input.GetKeyUp(KeyCode.UpArrow)){
                rb.isKinematic = true;

                SwitchGravity(GravityDirection.up);
            }else if(Input.GetKeyUp(KeyCode.DownArrow)){
                rb.isKinematic = true;
                SwitchGravity(GravityDirection.down);
            }else if(Input.GetKeyUp(KeyCode.P)){
                rb.isKinematic = true;
                SwitchGravity(GravityDirection.front);
            }else if(Input.GetKeyUp(KeyCode.O)){
                rb.isKinematic = true;
                SwitchGravity(GravityDirection.back);
            }
        }
        if(Input.GetKeyUp(KeyCode.W)){
            print("subtract:  " + (Mathf.Abs((transform.eulerAngles.z) - targetAngle)));
            print("first value: " + (NormalizeAngle(transform.eulerAngles.z)));
            print("Target angle: " + (targetAngle-0.3f));
            print("angle: " + angle);
        }
        if(isrotate){
            if(!forBack){
                if(Mathf.Abs((transform.eulerAngles.z) -targetAngle) > 0.4f){
                    print("uh");
                    angle = Mathf.SmoothDampAngle(NormalizeAngle(transform.eulerAngles.z), targetAngle, ref r, 0.1f);
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                }else{
                    // animator.SetBool("fall", false);
                    // animator.SetBool("idle", true);
                    bx.enabled = true;
                    rb.isKinematic = false;
                    isrotate = false;
                }
            }else{
                if(Mathf.Abs((transform.eulerAngles.x) -targetAngle) > 0.4f){
                    print("uh");
                    angle = Mathf.SmoothDampAngle(NormalizeAngle(transform.eulerAngles.x), targetAngle, ref r, 0.1f);
                    transform.rotation = Quaternion.Euler(angle, 90, -90);
                }else{
                    // animator.SetBool("fall", false);
                    // animator.SetBool("idle", true);
                    bx.enabled = true;
                    rb.isKinematic = false;
                    forBack=false;
                    isrotate = false;
                }
            }
            // if (transform.rotation != targetRotation)
            // {
            //     transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            // }else{
            //     isrotate = false;
            // }
        }
    }
    float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0f)
        {
            angle += 360f;
        }

        // if(angle>=0 && angle <=0.5){
        //     angle = 360;
        // }
        return angle;
    }
    private void SwitchGravity(GravityDirection gravity){
        bx.enabled = false;

        currentGravity = gravity;
        animator.SetBool("fall", true);
        animator.SetBool("idle", false);
        forBack=false;

        switch(currentGravity){
            case GravityDirection.down:
                currentGravityVector = Vector3.down;
                rotate(0);
                break;
            case GravityDirection.up:
                currentGravityVector = Vector3.up;
                rotate(180);
                break;
            case GravityDirection.left:
                currentGravityVector = Vector3.left;
                rotate(270);
                break;
            case GravityDirection.right:
                currentGravityVector = Vector3.right;
                rotate(90);
                break;
            case GravityDirection.front:
                currentGravityVector = Vector3.forward;
                forBack=true;
                rotate(90);
                break;
            case GravityDirection.back:
                currentGravityVector = Vector3.back;
                forBack=true;
                rotate(270);
                break;
        }
        rb.velocity = Vector3.zero;
        
    }
    private void rotate(float zDegree){
        if(!forBack){
            targetAngle = zDegree;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetAngle, ref r, 0.1f);
            print("To rotate :" +zDegree);
        }else{
            targetAngle = zDegree;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.x, targetAngle, ref r, 0.1f);
        }
            isrotate = true;
            rb.isKinematic = true;
        // transform.eulerAngles = new Vector3(0f, 0f, NormalizeAngle(transform.eulerAngles.z));
        // angle = 0;
        // targetRotation = transform.rotation * Quaternion.Euler(0f, 0f, zDegree);
        // transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        // transform.Rotate(Time.deltaTime * rotationSpeed * 0f, 0f, zDegree);
        // transform.rotation = Quaternion.Lerp(transform.rotation, (0f,0f,zDegree), rotationSpeed * Time.deltaTime);

    }
    void FixedUpdate()
    {

        // Vector3 desiredVelocity = moveDirection * moveSpeed;
        // Vector3 velocityChange = (desiredVelocity - rb.velocity);
        // rb.AddForce(velocityChange, ForceMode.VelocityChange);

        // // Rotate the player towards the move direction.
        // if (moveDirection != Vector3.zero)
        // {
        //     Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        //     rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        // }




        rb.AddForce(currentGravityVector * Physics.gravity.magnitude, ForceMode.Acceleration);

        RaycastHit hit;
        float raycastDistance = 0.5f; // Adjust this distance based on your character's size.
        moveDirection = currentGravityVector;// Calculate the movement direction of the player (e.g., Input).

        // Cast a ray in the movement direction to detect the wall.
        if (Physics.Raycast(transform.position, moveDirection, out hit, raycastDistance))
        {
            // rb.isKinematic = false;
            // Get the normal of the surface the player is colliding with.
            Vector3 surfaceNormal = hit.normal;

            // Calculate the corrected position to avoid clipping.
            Vector3 correctedPosition = hit.point + (surfaceNormal * playerRadius * 0.5f);

            // Move the player to the corrected position.
            transform.position = correctedPosition;
        }else{
            // rb.isKinematic = false;
        }
    }

}
