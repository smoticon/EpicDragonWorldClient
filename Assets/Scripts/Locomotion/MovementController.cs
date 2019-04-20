using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: January 10th 2019
 */
public class MovementController : MonoBehaviour
{
    // Configs.
    public float speed = 1.0f;
    public float speedRotation = 2.0f;
    public float speedRotationSide = 4.0f;
    public float speedWater = 0.999f;
    public float speedJump = 5.0f;
    public float jumpHeight = 7.0f;
    public float distToGround = 0.1f;
    // Static values.
    private readonly string LAYER_GROUND_VALUE = "Everything";
    private readonly string WATER_TAG_VALUE = "Water";
    // Non-static values.
    private Rigidbody rigidBody;
    private LayerMask layerGround;
    private float speedCurrent = 0;
    private bool lockedMovement = false;
    private bool sideMovement = false;
    private float oldRotation;
    public static Vector3 oldPosition = Vector3.zero;

    private void Start()
    {
        layerGround = LayerMask.NameToLayer(LAYER_GROUND_VALUE);
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.useGravity = !WorldManager.Instance.isPlayerInWater;
        oldPosition = transform.position;
        oldRotation = transform.localRotation.eulerAngles.y;
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

            // Check for side movement.
            sideMovement = InputManager.RIGHT_MOUSE_PRESS && !InputManager.UP_PRESS && !InputManager.DOWN_PRESS && (InputManager.LEFT_PRESS || InputManager.RIGHT_PRESS);

            // Jump.
            if (InputManager.SPACE_PRESS)
            {
                if (WorldManager.Instance.isPlayerInWater)
                {
                    // TODO: Check if player goes upper than water level.
                    // if (Physics.Raycast(rBody.transform.position, Vector3.up, distToGround, layerWater))
                    if (transform.position.y < 63.33f)
                    {
                        transform.localPosition += transform.up * speedCurrent * Time.deltaTime;
                    }
                }
                else if (WorldManager.Instance.isPlayerOnTheGround)
                {
                    speedCurrent = speedJump;
                    rigidBody.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
                }
            }

            // Front.
            if (InputManager.UP_PRESS || lockedMovement || sideMovement)
            {
                transform.localPosition += transform.forward * speedCurrent * Time.deltaTime;
            }

            // Back.
            if (InputManager.DOWN_PRESS)
            {
                transform.localPosition -= transform.forward * speedCurrent * Time.deltaTime;
            }

            // Left.
            if (InputManager.LEFT_PRESS && !InputManager.RIGHT_PRESS)
            {
                // Rotate.
                SetPlayerRotation(transform.rotation.eulerAngles.y - (sideMovement ? speedRotationSide : speedRotation));
            }

            // Right.
            if (InputManager.RIGHT_PRESS && !InputManager.LEFT_PRESS)
            {
                // Rotate.
                SetPlayerRotation(transform.rotation.eulerAngles.y + (sideMovement ? speedRotationSide : speedRotation));
            }
        }

        // Send changes to network.
        if (oldRotation != transform.localRotation.eulerAngles.y
            || oldPosition.x != transform.position.x //
            || oldPosition.y != transform.position.y //
            || oldPosition.z != transform.position.z)
        {
            NetworkManager.ChannelSend(new LocationUpdateRequest(transform.position.x, transform.position.y, transform.position.z, transform.localRotation.eulerAngles.y));
            oldPosition = transform.position;
            oldRotation = transform.localRotation.eulerAngles.y;
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
