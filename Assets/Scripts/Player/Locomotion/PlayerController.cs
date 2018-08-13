using UnityEngine;

/**
 * @author Pantelis Andrianakis, Abdallah Azzami
 */
public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public class MoveSettings
    {
        public float forwardVel = 7;
        public float rotateVel = 100;
        public float jumpVel = 17;
        public float distToGrounded = 0.05f;
        public LayerMask ground;
    }

    [System.Serializable]
    public class PhysSettings
    {
        public float downAccel = 1f;
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

    private AnimationController animationsController;

    private bool movementLock = false;

    // Footstep sounds.
    public AudioSource FootstepAudioSource;
    public AudioClip[] FootstepSounds;

    public bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, moveSetting.distToGrounded, moveSetting.ground);
    }

    private void Start()
    {
        animationsController = GetComponent<AnimationController>();
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

        // Movement lock.
        if (Input.GetKeyDown(KeyCode.Numlock) || Input.GetMouseButtonDown(3))
        {
            movementLock = !movementLock;
        }
        if (forwardInput < 0 || (Input.GetMouseButton(0) && Input.GetMouseButton(1)))
        {
            movementLock = false;
        }
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

        // Footstep sounds.
        if (!FootstepAudioSource.isPlaying && rBody.velocity.magnitude > 2f && (forwardInput > 0 || movementLock) && Grounded())
        {
            FootstepAudioSource.PlayOneShot(FootstepSounds[0], 1f);
        }

        // Send position to server.
        if (NetworkManager.instance != null && (oldX != transform.position.x || oldY != transform.position.y || oldZ != transform.position.z))
        {
            NetworkManager.instance.ChannelSend(new LocationUpdate(transform.position.x, transform.position.y, transform.position.z));
            oldX = transform.position.x;
            oldY = transform.position.y;
            oldZ = transform.position.z;
        }
    }

    private void Run()
    {
        if ((Input.GetMouseButton(0) && Input.GetMouseButton(1)) || movementLock)
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
        bool leftMouseBtn = Input.GetMouseButton(1);
        bool rightMouseBtn = Input.GetMouseButton(0);

        // Mouse x axis, to rotate the player while holding mouse buttons.
        float mouseX = Input.GetAxis("Mouse X") * 3;

        if (Mathf.Abs(turnInput) > inputSetting.inputDelay && !rightMouseBtn)
        {
            if (forwardInput < 0)
            {
                transform.rotation *= Quaternion.AngleAxis(moveSetting.rotateVel * -turnInput * Time.deltaTime, Vector3.up);
            }
            else
            {
                transform.rotation *= Quaternion.AngleAxis(moveSetting.rotateVel * turnInput * Time.deltaTime, Vector3.up);
            }
        }
        else if (leftMouseBtn)
        {
            transform.rotation *= Quaternion.AngleAxis(moveSetting.rotateVel * mouseX * Time.deltaTime, Vector3.up);
        }

        if (rightMouseBtn && !leftMouseBtn)
        {
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
            animationsController.JumpAnimation();
            jumpDelay = 0;
        }
        else if (jumpDelay >= 0.15f && velocity.z > 3.5f)
        {
            velocity.y = moveSetting.jumpVel;
            animationsController.FarJumpAnimation();
            jumpDelay = 0;
        }
    }
}
