using UnityEngine;
using System.Runtime.InteropServices;

/**
 * @author Abdallah Azzami
 */
public class AnimationController : MonoBehaviour
{
    [HideInInspector]
    public Animator anim;
    public float movementSpeed = 0.001f;
    public float rotationSpeed = 0.001f;
    private PlayerController playerController;
    private float translation;
    private float rotation;
    private int lastMousePositionX = 0;
    private int lastMousePositionY = 0;
    private bool movementLock = false;

    public bool canMove = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        canMove = true;
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame.
    private void Update()
    {
        if (canMove && transform.position.y >= 0)
        {
            MoveController();
            anim.SetBool("IsFalling", false);
        }
        else if (transform.position.y < -0.5f && !playerController.Grounded())
        {
            AllAnimationOff();
            anim.SetBool("IsFalling", true);
        }
        // Hide the mouse.
        HideCursor();
    }

    private void MoveController()
    {
        // Get Axis.
        translation = Input.GetAxis("Vertical") * movementSpeed;
        rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        // Mouse buttons.
        bool bothMouseBtn = Input.GetMouseButton(0) && Input.GetMouseButton(1);
        bool leftMouseBtn = Input.GetMouseButton(1);
        bool rightMouseBtn = Input.GetMouseButton(0);

        float mouseX = Input.GetAxis("Mouse X");

        // Movement lock.
        if (Input.GetKeyDown(KeyCode.Numlock) || Input.GetMouseButtonDown(3))
        {
            movementLock = !movementLock;
        }
        if (translation < 0 || bothMouseBtn)
        {
            movementLock = false;
        }

        // Running, walking, forward jump.
        if (translation > 0 || bothMouseBtn || movementLock)
        {
            // Forward.
            if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetBool("IsWalkingForward", true);
                anim.SetBool("IsWalkingBackward", false);
                anim.SetBool("IsRunning", false);
                RotateAnimationOff();
                if (!bothMouseBtn)
                {
                    playerController.moveSetting.forwardVel = 3.5f;
                }
            }
            else
            {
                anim.SetBool("IsRunning", true);
                WalkAnimationOff();
                RotateAnimationOff();
                playerController.moveSetting.forwardVel = 7;
            }
        }
        // Walking back.
        else if (translation < 0)
        {
            anim.SetBool("IsWalkingBackward", true);
            anim.SetBool("IsWalkingForward", false);
            anim.SetBool("IsRunning", false);
            RotateAnimationOff();
            playerController.moveSetting.forwardVel = 2.7f;
        }
        else if (rotation > 0 || (leftMouseBtn && mouseX > 0.1f && !rightMouseBtn))
        {
            anim.SetBool("RightTurn", true);
            anim.SetBool("LeftTurn", false);
            anim.SetBool("IsRunning", false);
            WalkAnimationOff();
        }
        else if (rotation < 0 || (leftMouseBtn && mouseX < -0.1f && !rightMouseBtn))
        {
            anim.SetBool("LeftTurn", true);
            anim.SetBool("RightTurn", false);
            anim.SetBool("IsRunning", false);
            WalkAnimationOff();
        }
        else
        {
            AllAnimationOff();
        }
    }

    private void HideCursor()
    {
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

    public void JumpAnimation()
    {
        anim.SetBool("NormalJump", true);
        anim.SetBool("IsRunning", false);
        WalkAnimationOff();
        RotateAnimationOff();
        Invoke("StopJump", 0.3f);
    }

    public void FarJumpAnimation()
    {
        anim.SetBool("FarJump", true);
        anim.SetBool("IsRunning", false);
        WalkAnimationOff();
        RotateAnimationOff();
        Invoke("StopFarJump", 0.3f);
    }

    private void StopJump()
    {
        anim.SetBool("NormalJump", false);
        canMove = true;
    }

    private void StopFarJump()
    {
        anim.SetBool("FarJump", false);
    }

    private void WalkAnimationOff()
    {
        anim.SetBool("IsWalkingForward", false);
        anim.SetBool("IsWalkingBackward", false);
    }

    private void RotateAnimationOff()
    {
        anim.SetBool("LeftTurn", false);
        anim.SetBool("RightTurn", false);
    }

    private void AllAnimationOff()
    {
        anim.SetBool("IsWalkingBackward", false);
        anim.SetBool("IsWalkingForward", false);
        anim.SetBool("LeftTurn", false);
        anim.SetBool("RightTurn", false);
        anim.SetBool("IsRunning", false);
    }
}

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