using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class PlayerMovementController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Camera playerCamera;
    private Rigidbody rb;

    [Header("Parameters")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float linearDamping;
    [SerializeField] private float sensitivityScaler;

    //Current player and camera rot
    private float cameraRotX;
    private float cameraRotY;
    private float playerRotY;

    //Current player movement input
    private float inputX;
    private float inputY;

    //Current player mouse input
    private float mouseInputX;
    private float mouseInputY;


    //////////////////////////////////////////////////////////////////////////////
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.linearDamping = linearDamping;
    }

    //////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        //Updates input and rotates player if necessary
        if (GameManager.instance.stateOfGame == GameManager.States.InGame && Time.timeScale != 0) //Disabled whilst paused
        {
            GetInput();
            RotateCameraAndPlayer();
            LimitMoveSpeed();
        }
    }


    //////////////////////////////////////////////////////////////////////////////
    private void FixedUpdate()
    {
        //Moves player
        MovePlayer();
    }

    //////////////////////////////////////////////////////////////////////////////
    private void GetInput()
    {
        //Updates input variables
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");

        mouseInputX = Input.GetAxisRaw("Mouse X");
        mouseInputY = -Input.GetAxisRaw("Mouse Y");
    }

    //////////////////////////////////////////////////////////////////////////////
    private void MovePlayer()
    {
        //Moves player based on movement speed and input
        Vector3 movementDir = (transform.forward * inputY) + (transform.right * inputX);
        rb.AddForce(movementDir.normalized * 100, ForceMode.Force);
    }

    //////////////////////////////////////////////////////////////////////////////
    private void RotateCameraAndPlayer()
    {
        //Updates new values for rotation of player and camera based on input
        cameraRotX += mouseInputY * SettingsManager.instance.Sensitivity;
        cameraRotY += mouseInputX * SettingsManager.instance.Sensitivity;
        playerRotY += mouseInputX * SettingsManager.instance.Sensitivity;

        //Clamps camera rot
        cameraRotX = Mathf.Clamp(cameraRotX, -90, 90);

        //Assigns rotation based on updates variable values
        playerCamera.transform.rotation = Quaternion.Euler(cameraRotX, cameraRotY, playerCamera.transform.rotation.z);
        transform.rotation = Quaternion.Euler(transform.rotation.x, playerRotY, playerCamera.transform.rotation.z);

    }

    //////////////////////////////////////////////////////////////////////////////
    private void LimitMoveSpeed()
    {
        //Limits the player's movement speed respectively
        Vector3 currentVelocity = new Vector3(rb.linearVelocity.x,0,rb.linearVelocity.z);

        if (currentVelocity.magnitude > movementSpeed)
        {
            Vector3 newVelocity = currentVelocity.normalized * movementSpeed;
            rb.linearVelocity = new Vector3(newVelocity.x,0,newVelocity.z);
        }

    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
