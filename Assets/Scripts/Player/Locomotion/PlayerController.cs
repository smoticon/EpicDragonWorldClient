using System.Runtime.InteropServices;
using UnityEngine;

/**
 * @author Pantelis Andrianakis, Abdallah Azzami
 */
public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public class MoveSettings
    {
        public float forwardVel = 4;
        public float rotateVel = 100;
        public float distToGrounded = 0.5f;
        public LayerMask ground; // = LayerMask.NameToLayer("Everything")
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
    public float forwardInput;
    private float forwardInputByMouse;

    private float turnInput;
    public float jumpInput;
    private float jumpDelay;

    private float oldX = 0;
    // private float oldY = 0;
    private float oldZ = 0;
    private float oldYAngle = 0f;

    private PL_MOVE_ANIM_STATE playerMoveState = PL_MOVE_ANIM_STATE.PL_IDLE;

    private int lastMousePositionX = 0;
    private int lastMousePositionY = 0;

    public bool movementLock = false;
    public bool sideWalking = false;

    // Water settings.
    public float waterHeight = 9.0f;
    public bool isInsideWater = false;
    public bool waterTouchState = false;
    // Footstep sounds.
    public AudioSource FootstepAudioSource;
    public AudioClip[] FootstepSounds;
    public GameObject targetCamera;

    public bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, moveSetting.distToGrounded, moveSetting.ground);
    }

    private void Start()
    {
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

    private void Update()
    {
        if (ChatBoxManager.instance.isFocused)
        {
            return;
        }

        GetInput();
        Turn();

        // Hide the mouse.
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
#if UNITY_STANDALONE_WIN
            Win32Cursor.POINT point = new Win32Cursor.POINT();
            Win32Cursor.GetCursorPos(out point);
            lastMousePositionX = point.X;
            lastMousePositionY = point.Y;
#endif
            Cursor.visible = false;
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
#if UNITY_STANDALONE_WIN
            if (!Cursor.visible)
            {
                Win32Cursor.SetCursorPos(lastMousePositionX, lastMousePositionY);
            }
#endif
            Cursor.visible = true;
        }

        // TODO: Move to menu.
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
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

        if (Input.GetMouseButton(1) && turnInput != 0)
        {
            sideWalking = true;
        }
        else
        {
            sideWalking = false;
        }
    }

    private void FixedUpdate()
    {
        Run();
        Jump();

        if (isInsideWater)
        {
            if (targetCamera.transform.localRotation.eulerAngles.x > 0 && targetCamera.transform.localRotation.eulerAngles.x < 80f)
            {
                velocity.y = -0.5f;
            }
            else if ((targetCamera.transform.localRotation.eulerAngles.x < 0 || targetCamera.transform.localRotation.eulerAngles.x > 80f) && transform.position.y < 8f)
            {
                velocity.y = 0.5f;
            }
            else
            {
                velocity.y = 0f;
            }

            rBody.velocity = transform.TransformDirection(velocity);
        }
        else
        {
            rBody.velocity = transform.TransformDirection(velocity);
        }

        // Send position to server.
        if (isInsideWater)
        {
            gameObject.GetComponent<PlayerAnimationController>().SetSwimmingState(playerMoveState);
            if (NetworkManager.instance != null && (Vector2.Distance(new Vector2(oldX, oldZ), new Vector2(transform.position.x, transform.position.z)) > 0.2f || Mathf.Abs(oldYAngle - transform.localRotation.eulerAngles.y) > 3f))
            {
                NetworkManager.instance.ChannelSend(new LocationUpdate(transform.position.x, transform.position.y, transform.position.z, transform.localRotation.eulerAngles.y, (int)playerMoveState, isInsideWater));
                oldX = transform.position.x;
                // oldY = transform.position.y;
                oldZ = transform.position.z;
                oldYAngle = transform.localRotation.eulerAngles.y;
            }
        }
        else
        {
            gameObject.GetComponent<PlayerAnimationController>().SetAnimState(playerMoveState);
            if (NetworkManager.instance != null && (Vector2.Distance(new Vector2(oldX, oldZ), new Vector2(transform.position.x, transform.position.z)) > 0.2f || Mathf.Abs(oldYAngle - transform.localRotation.eulerAngles.y) > 3f) && Grounded())
            {
                NetworkManager.instance.ChannelSend(new LocationUpdate(transform.position.x, transform.position.y, transform.position.z, transform.localRotation.eulerAngles.y, (int)playerMoveState, isInsideWater));
                oldX = transform.position.x;
                // oldY = transform.position.y;
                oldZ = transform.position.z;
                oldYAngle = transform.localRotation.eulerAngles.y;
            }

            // Footstep sounds.
            if (!FootstepAudioSource.isPlaying && rBody.velocity.magnitude > 2f && (forwardInput > 0 || movementLock) && Grounded())
            {
                FootstepAudioSource.PlayOneShot(FootstepSounds[0], 1f);
            }
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
            playerMoveState = PL_MOVE_ANIM_STATE.PL_W;
            velocity.z = moveSetting.forwardVel * forwardInputByMouse;
        }
        else if (Mathf.Abs(forwardInput) > inputSetting.inputDelay)
        {
            // Move.
            if (!sideWalking)
            {
                if (forwardInput < 0)
                {
                    playerMoveState = PL_MOVE_ANIM_STATE.PL_S;
                    velocity.z = (moveSetting.forwardVel * 0.5f) * forwardInput;
                }
                else
                {
                    playerMoveState = PL_MOVE_ANIM_STATE.PL_W;
                    velocity.z = moveSetting.forwardVel * forwardInput;
                }
            }
            else if (sideWalking)
            {
                if (forwardInput < 0)
                {
                    if (turnInput > 0)
                    {
                        playerMoveState = PL_MOVE_ANIM_STATE.PL_DS;
                    }
                    else
                    {
                        playerMoveState = PL_MOVE_ANIM_STATE.PL_SA;
                    }
                    velocity.z = (moveSetting.forwardVel * 0.5f) * forwardInput;
                    velocity.x = moveSetting.forwardVel * turnInput * 0.5f;
                }
                else
                {
                    if (turnInput > 0)
                    {
                        playerMoveState = PL_MOVE_ANIM_STATE.PL_WD;
                    }
                    else
                    {
                        playerMoveState = PL_MOVE_ANIM_STATE.PL_AW;
                    }
                    velocity.z = moveSetting.forwardVel * forwardInput;
                    velocity.x = moveSetting.forwardVel * turnInput;
                }
            }
        }
        else if (Mathf.Abs(forwardInput) < inputSetting.inputDelay && sideWalking && !isInsideWater)
        {
            if (turnInput > 0)
            {
                playerMoveState = PL_MOVE_ANIM_STATE.PL_D;
            }
            else
            {
                playerMoveState = PL_MOVE_ANIM_STATE.PL_A;
            }
            velocity.x = moveSetting.forwardVel * turnInput;
        }
        else
        {
            // Zero velocity.
            velocity.z = 0;
            velocity.x = 0;
            playerMoveState = PL_MOVE_ANIM_STATE.PL_IDLE;
            NetworkManager.instance.ChannelSend(new LocationUpdate(transform.position.x, transform.position.y, transform.position.z, transform.localRotation.eulerAngles.y, (int)PL_MOVE_ANIM_STATE.PL_IDLE, gameObject.GetComponent<PlayerController>().isInsideWater));
        }
        forwardInputByMouse = 0;
    }

    private void Turn()
    {
        if (sideWalking)
        {
            return;
        }

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
    }

    private void Jump()
    {
        if (ChatBoxManager.instance.isFocused)
        {
            return;
        }

        if ((jumpInput > 0 || Input.GetKey(KeyCode.Space)) && Grounded())
        {
            // Jump.
            jumpDelay += Time.deltaTime;
            DelayJump();
        }
        else if (jumpInput == 0 && Grounded())
        {
            if (!isInsideWater)
            {
                // Zero out velocity.y
                velocity.y = 0;
            }
        }
        else
        {
            // Decrease velocity.y
            if (!isInsideWater)
            {
                velocity.y -= physSetting.downAccel;
            }
        }
    }

    private void DelayJump()
    {
        if (jumpDelay >= 0.13f && velocity.z <= 3.5f) // Jump on spot.
        {
            gameObject.GetComponent<PlayerAnimationController>().SetAnimState(PL_MOVE_ANIM_STATE.PL_STAND_JUMP);
            jumpDelay = 0;
            if (NetworkManager.instance != null)
            {
                NetworkManager.instance.ChannelSend(new LocationUpdate(transform.position.x, transform.position.y, transform.position.z, gameObject.transform.localRotation.eulerAngles.y, 32, isInsideWater));
            }
        }
        else if (jumpDelay >= 0.15f && velocity.z > 3.5f) // Moving jump.
        {
            gameObject.GetComponent<PlayerAnimationController>().SetAnimState(PL_MOVE_ANIM_STATE.PL_JUMP);
            jumpDelay = 0;
            if (NetworkManager.instance != null)
            {
                NetworkManager.instance.ChannelSend(new LocationUpdate(transform.position.x, transform.position.y, transform.position.z, gameObject.transform.localRotation.eulerAngles.y, 31, isInsideWater));
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "water" && !isInsideWater)
        {
            Animator anim = gameObject.GetComponent<Animator>();
            anim.SetBool("IsSwimming", true);
            anim.Play("Swimming");
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            isInsideWater = true;
            if (NetworkManager.instance != null)
            {
                NetworkManager.instance.ChannelSend(new LocationUpdate(transform.position.x, transform.position.y, transform.position.z, gameObject.transform.localRotation.eulerAngles.y, (int)gameObject.GetComponent<PlayerAnimationController>().animState, isInsideWater));
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "water" && isInsideWater && gameObject.transform.position.y >= 8f)
        {
            Animator anim = gameObject.GetComponent<Animator>();
            anim.SetBool("IsSwimming", false);
            anim.SetBool("IsSwimmingIdle", false);
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            isInsideWater = false;
            if (NetworkManager.instance != null)
            {
                NetworkManager.instance.ChannelSend(new LocationUpdate(transform.position.x, transform.position.y, transform.position.z, gameObject.transform.localRotation.eulerAngles.y, (int)gameObject.GetComponent<PlayerAnimationController>().animState, isInsideWater));
            }
        }
    }
}

public enum PL_MOVE_ANIM_STATE
{
    PL_W = 21,
    PL_S = 22,
    PL_A = 23,
    PL_D = 24,
    PL_AW = 25,
    PL_WD = 26,
    PL_DS = 27,
    PL_SA = 28,
    PL_IDLE = 29,
    PL_JUMP = 31,
    PL_STAND_JUMP = 32
};

// Windows only fix for locking mouse cursor position, since Unity does not support setting cursor position.
#if UNITY_STANDALONE_WIN
public class Win32Cursor
{
    [DllImport("User32.Dll")]
    public static extern long SetCursorPos(int x, int y);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetCursorPos(out POINT lpPoint);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
#endif