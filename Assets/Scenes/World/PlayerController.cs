using UnityEngine;

/**
 * @author Pantelis Andrianakis, Abdallah Azzami
 */
public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public class MoveSettings
    {
        public float forwardVel = 12;
        public float rotateVel = 100;
        public float jumpVel = 25;
        public float distToGrounded = 0.1f;
        public LayerMask ground;
    }

    [System.Serializable]
    public class PhysSettings
    {
        public float downAccel = 0.75f;
    }

    [System.Serializable]
    public class InputSettings
    {
        public float inputDelay = 0.1f;
        public string FORWARD_AXIS = "Vertical";
        public string TURN_AXIS = "Horizontal";
        public string JUMP_AXIS = "Jump";
    }

    public MoveSettings moveSetting = new MoveSettings();
    public PhysSettings physSetting = new PhysSettings();
    public InputSettings inputSetting = new InputSettings();
    public GameObject objectTarget;

    Vector3 velocity = Vector3.zero;
    private Rigidbody rBody;
    private float forwardInput;
    private float forwardInputByMouse;

    private float turnInput;
    private float jumpInput;
    private float jumpDelay;

    private float oldX = 0;
    private float oldY = 0;
    private float oldZ = 0;
    private Movement movement;
    private bool canRotate = false;

    public Quaternion TargetRotation { get; private set; }

    public bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, moveSetting.distToGrounded, moveSetting.ground);
    }

    private void Start()
    {
        movement = GetComponent<Movement>();
        TargetRotation = transform.rotation;
        if (GetComponent<Rigidbody>())
        {
            rBody = GetComponent<Rigidbody>();
        }
        else
        {
            Debug.LogError("The character needs a rigidbody.");
        }
        forwardInput = 0;
        turnInput = 0;
        jumpInput = 0;
    }

    private void GetInput()
    {
        forwardInput = Input.GetAxis(inputSetting.FORWARD_AXIS); // Interpolated.
        turnInput = Input.GetAxis(inputSetting.TURN_AXIS); // Interpolated.
        jumpInput = Input.GetAxisRaw(inputSetting.JUMP_AXIS); // Non-interpolated.
    }

    private void Update()
    {
        GetInput();
        Turn();
    }

    private void FixedUpdate()
    {
        Run();
        Jump();

        rBody.velocity = transform.TransformDirection(velocity);

        // Send position to server.
        if (oldX != transform.position.x || oldY != transform.position.y || oldZ != transform.position.z)
        {
            NetworkManager.instance.ChannelSend(new LocationUpdate(transform.position.x, transform.position.y, transform.position.z));
            oldX = transform.position.x;
            oldY = transform.position.y;
            oldZ = transform.position.z;
        }
    }

    private void Run()
    {
        if ((Input.GetMouseButton(0) && Input.GetMouseButton(1)))
        {
            velocity.z = moveSetting.forwardVel * forwardInputByMouse;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                forwardInputByMouse += 0.5f;
            }
            else
            {
                forwardInputByMouse += 1f;
            }
            velocity.z = moveSetting.forwardVel * forwardInputByMouse;
        }
        else if (Mathf.Abs(forwardInput) > inputSetting.inputDelay)
        {
            // Move.
            velocity.z = moveSetting.forwardVel * forwardInput;
        }
        else
        {
            // Zero velocity.
            velocity.z = 0;
        }
        forwardInputByMouse = 0;
    }

    private void Turn()
    {
        // Check if the mouse buttons are clicked, and assign it to boolean var.
        bool leftMouseBtn = (Input.GetMouseButton(1));
        bool rightMouseBtn = (Input.GetMouseButton(0));

        // Mouse x axis, to rotate the player while holding mouse buttons.
        float mouseX = Input.GetAxis("Mouse X") * 3;

        if (Mathf.Abs(turnInput) > inputSetting.inputDelay && !rightMouseBtn)
        {
            if (forwardInput < 0)
            {
                TargetRotation *= Quaternion.AngleAxis(moveSetting.rotateVel * -turnInput * Time.deltaTime, Vector3.up);
            }
            else
            {
                TargetRotation *= Quaternion.AngleAxis(moveSetting.rotateVel * turnInput * Time.deltaTime, Vector3.up);
            }
        }
        else if (leftMouseBtn || rightMouseBtn)
        {
            TargetRotation *= Quaternion.AngleAxis(moveSetting.rotateVel * mouseX * Time.deltaTime, Vector3.up);
        }

        ChangeTarget();

        if ((rightMouseBtn && !leftMouseBtn) || canRotate)
        {
            objectTarget.transform.rotation = TargetRotation;

            // Rotation while walking and holding the right mouse button.
            float xRotaion = Input.GetAxis("Horizontal");
            float yRotaion = Input.GetAxis("Vertical");
            if (xRotaion != 0 && yRotaion != 0)
            {
                if (forwardInput < 0) // When walk back rotation will be in the opposite direction.
                {
                    transform.Rotate(transform.rotation.x, transform.rotation.y - xRotaion * Time.deltaTime * 170, transform.rotation.z);
                }
                else
                {
                    transform.Rotate(transform.rotation.x, transform.rotation.y + xRotaion * Time.deltaTime * 170, transform.rotation.z);
                }
            }
        }
        else
        {
            transform.rotation = TargetRotation;
        }
    }

    private void ChangeTarget()
    {
        if (Input.GetMouseButtonUp(0))
        {
            canRotate = true;
        }
        else if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonUp(1))
        {
            canRotate = false;
        }
    }

    private void Jump()
    {
        if ((jumpInput > 0 || Input.GetKey(KeyCode.Space)) && Grounded())
        {
            // Jump.
            jumpDelay += Time.deltaTime;
            DelayJump();
        }
        else if (jumpInput == 0 && Grounded())
        {
            // Zero out velocity.y
            velocity.y = 0;
        }
        else
        {
            // Decrease velocity.y
            velocity.y -= physSetting.downAccel;
        }
    }

    private void DelayJump()
    {
        if (jumpDelay >= 0.13f && velocity.z <= 3.5f)
        {
            velocity.y = moveSetting.jumpVel;
            movement.JumpAnimation();
            jumpDelay = 0;
        }
        else if (jumpDelay >= 0.15f && velocity.z > 3.5f)
        {
            velocity.y = moveSetting.jumpVel;
            movement.FarJumpAnimation();
            jumpDelay = 0;
        }
    }
}
