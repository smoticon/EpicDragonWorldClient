using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: January 10th 2019
 */
public class MovementController : MonoBehaviour
{
    // Configs.
    public float speed = 1.0f;
    public float speedRotation = 8.0f;
    public float speedWater = 0.999f;
    public float speedJump = 5.5f;
    public float jumpPower = 7.5f;
    public float distToGround = 0.1f;
    public float waterLevel = 63.2f;
    // Static values.
    private readonly string LAYER_GROUND_VALUE = "Everything";
    private readonly string WATER_TAG_VALUE = "Water";
    // Non-static values.
    private Rigidbody rigidBody;
    private LayerMask layerGround;
    private float speedCurrent = 0;
    public static bool leftSideMovement = false;
    public static bool rightSideMovement = false;
    public static bool lockedMovement = false;
    public static float storedRotation = 0;
    public static Vector3 storedPosition = Vector3.zero;

    private void Start()
    {
        layerGround = LayerMask.NameToLayer(LAYER_GROUND_VALUE);
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.useGravity = !WorldManager.Instance.isPlayerInWater;
        storedPosition = transform.position;
        storedRotation = transform.localRotation.eulerAngles.y;
    }

    private void Update()
    {
        // Set player grounded state.
        WorldManager.Instance.isPlayerOnTheGround = Physics.Raycast(rigidBody.transform.position, Vector3.down, distToGround, layerGround);

        // Calculate current speed.
        speedCurrent = WorldManager.Instance.isPlayerInWater ? speedWater : WorldManager.Instance.isPlayerOnTheGround ? speed : speedJump;

        // Do nothing when chat is active.
        if (MainManager.Instance.isChatBoxActive)
        {
            if (lockedMovement)
            {
                transform.localPosition += transform.forward * speedCurrent * Time.deltaTime;
            }
        }
        else
        {
            // Check for locked movement.
            if (InputManager.NUMLOCK_DOWN || InputManager.SIDE_MOUSE_DOWN || (!lockedMovement && InputManager.LEFT_MOUSE_PRESS && InputManager.RIGHT_MOUSE_PRESS) || (lockedMovement && (InputManager.RIGHT_MOUSE_UP || InputManager.UP_PRESS || InputManager.DOWN_PRESS)))
            {
                lockedMovement = !lockedMovement;
            }

            // Jump.
            if (InputManager.SPACE_PRESS)
            {
                if (WorldManager.Instance.isPlayerInWater)
                {
                    // TODO: Check if player goes upper than water level.
                    // if (Physics.Raycast(rBody.transform.position, Vector3.up, distToGround, layerWater))
                    if (transform.position.y <= waterLevel)
                    {
                        transform.localPosition += transform.up * speedCurrent * Time.deltaTime;
                    }
                }
                else if (WorldManager.Instance.isPlayerOnTheGround)
                {
                    speedCurrent = speedJump;
                    rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpPower, rigidBody.velocity.y);
                }
            }

            // Front.
            if (InputManager.UP_PRESS || lockedMovement)
            {
                transform.localPosition += transform.forward * speedCurrent * Time.deltaTime;
            }

            // Back.
            if (InputManager.DOWN_PRESS)
            {
                transform.localPosition -= transform.forward * (speedCurrent * 0.66f) * Time.deltaTime;
            }

            // Check for side movement.
            if (!InputManager.RIGHT_MOUSE_PRESS)
            {
                leftSideMovement = false;
                rightSideMovement = false;
            }

            // Left.
            if (InputManager.LEFT_PRESS && !InputManager.RIGHT_PRESS)
            {
                if (InputManager.RIGHT_MOUSE_PRESS && !lockedMovement && !InputManager.UP_PRESS && !InputManager.DOWN_PRESS && !InputManager.LEFT_MOUSE_PRESS)
                {
                    if (!leftSideMovement)
                    {
                        leftSideMovement = true;
                        SetPlayerRotation(CameraController.Instance.transform.rotation.eulerAngles.y - 90);
                    }
                    transform.localPosition += transform.forward * speedCurrent * Time.deltaTime;
                }
                else if (!leftSideMovement)
                {
                    rightSideMovement = false;
                    if (InputManager.LEFT_MOUSE_PRESS || (!(InputManager.LEFT_MOUSE_PRESS && !InputManager.DOWN_PRESS)))
                    {
                        SetPlayerRotation(transform.rotation.eulerAngles.y - (!InputManager.LEFT_MOUSE_PRESS ? speedRotation : speedRotation * 0.66f));
                    }
                    else
                    {
                        SetPlayerRotationWithLerp(CameraController.Instance.transform.rotation.eulerAngles.y);
                    }
                }
            }
            else
            {
                leftSideMovement = false;
            }

            // Right.
            if (InputManager.RIGHT_PRESS && !InputManager.LEFT_PRESS)
            {
                if (InputManager.RIGHT_MOUSE_PRESS && !lockedMovement && !InputManager.UP_PRESS && !InputManager.DOWN_PRESS && !InputManager.LEFT_MOUSE_PRESS)
                {
                    if (!rightSideMovement)
                    {
                        rightSideMovement = true;
                        SetPlayerRotation(CameraController.Instance.transform.rotation.eulerAngles.y + 90);
                    }
                    transform.localPosition += transform.forward * speedCurrent * Time.deltaTime;
                }
                else if (!rightSideMovement)
                {
                    leftSideMovement = false;
                    if (InputManager.LEFT_MOUSE_PRESS || (!(InputManager.LEFT_MOUSE_PRESS && !InputManager.DOWN_PRESS)))
                    {
                        SetPlayerRotation(transform.rotation.eulerAngles.y + (!InputManager.LEFT_MOUSE_PRESS ? speedRotation : speedRotation * 0.66f));
                    }
                    else
                    {
                        SetPlayerRotationWithLerp(CameraController.Instance.transform.rotation.eulerAngles.y);
                    }
                }
            }
            else
            {
                rightSideMovement = false;
            }
        }

        // Send changes to network.
        if (storedRotation != transform.localRotation.eulerAngles.y
            || storedPosition.x != transform.position.x //
            || storedPosition.y != transform.position.y //
            || storedPosition.z != transform.position.z)
        {
            NetworkManager.ChannelSend(new LocationUpdateRequest(transform.position.x, transform.position.y, transform.position.z, transform.localRotation.eulerAngles.y));
            storedPosition = transform.position;
            storedRotation = transform.localRotation.eulerAngles.y;
        }
    }

    private void SetPlayerRotation(float newRotation)
    {
        Quaternion curHeading = transform.localRotation;
        Vector3 curvAngle = curHeading.eulerAngles;
        curvAngle.y = newRotation;
        curHeading.eulerAngles = curvAngle;
        transform.localRotation = curHeading;
    }

    private void SetPlayerRotationWithLerp(float newRotation)
    {
        Quaternion oldHeading = transform.localRotation;
        Quaternion newHeading = transform.localRotation;
        Vector3 curvAngle = newHeading.eulerAngles;
        curvAngle.y = newRotation;
        newHeading.eulerAngles = curvAngle;
        transform.localRotation = Quaternion.Lerp(oldHeading, newHeading, Time.deltaTime * 10);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(WATER_TAG_VALUE) && !WorldManager.Instance.isPlayerInWater)
        {
            WorldManager.Instance.isPlayerInWater = true;
            rigidBody.useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals(WATER_TAG_VALUE) && WorldManager.Instance.isPlayerInWater)
        {
            WorldManager.Instance.isPlayerInWater = false;
            rigidBody.useGravity = true;
        }
    }
}
