using UMA.CharacterSystem;
using UnityEngine;

/**
 * Author: Paintbrush
 * Date: July 24th 2018
 */
public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    [HideInInspector]
    public Transform target;

    public float targetHeight = 1.7f;
    public float offsetFromWall = 0.1f;

    public float maxDistance = 10;
    public float minDistance = 1;
    public float speedDistance = 5;

    public float xSpeed = 200.0f;
    public float ySpeed = 200.0f;

    public int yMinLimit = -40;
    public int yMaxLimit = 80;

    public int zoomRate = 40;

    public float rotationDampening = 3.0f;
    public float zoomDampening = 5.0f;

    public LayerMask collisionLayers = -1;

    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance = 5f;
    private float desiredDistance = 5f;
    private float correctedDistance = 5f;

    private void Start()
    {
        Instance = this;

        // Make the rigid body not change rotation.
        if (gameObject.GetComponent<Rigidbody>())
        {
            gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    // Camera logic on LateUpdate to only update after all character movement logic has been handled.
    private void LateUpdate()
    {
        // Don't do anything if target is not defined.
        if (target == null)
        {
            DynamicCharacterAvatar activeCharacter = WorldManager.Instance.activeCharacter;
            if (activeCharacter != null)
            {
                // Now we can set target.
                target = activeCharacter.transform;

                // Bring camera behing player.
                xDeg = target.eulerAngles.y;
                yDeg = 10;
            }
            return;
        }

        // If either mouse buttons are down, let the mouse govern camera position.
        if (GUIUtility.hotControl == 0 && !MainManager.Instance.isDraggingWindow)
        {
            if (InputManager.LEFT_MOUSE_PRESS || (InputManager.RIGHT_MOUSE_PRESS && !InputManager.LEFT_PRESS && !InputManager.RIGHT_PRESS) || MovementController.rightSideMovement || MovementController.leftSideMovement)
            {
                xDeg += InputManager.AXIS_MOUSE_X * xSpeed * 0.02f;
                yDeg -= InputManager.AXIS_MOUSE_Y * ySpeed * 0.02f;

                if (!InputManager.LEFT_MOUSE_PRESS || InputManager.RIGHT_MOUSE_PRESS)
                {
                    if (!InputManager.LEFT_PRESS && !InputManager.RIGHT_PRESS)
                    {
                        SetPlayerRotation(transform.rotation.eulerAngles.y);
                    }

                    // Hide cursor while rotating.
                    Cursor.visible = false;
                }

                // Hide cursor while rotating.
                if (InputManager.UP_PRESS || InputManager.DOWN_PRESS || InputManager.LEFT_PRESS || InputManager.RIGHT_PRESS)
                {
                    Cursor.visible = false;
                }
            }
            // Otherwise, ease behind the target if any of the directional keys are pressed and chat is not active.
            else if (!MainManager.Instance.isChatBoxActive && (InputManager.LEFT_PRESS || InputManager.RIGHT_PRESS))
            {
                float targetRotationAngle = target.eulerAngles.y;
                float currentRotationAngle = transform.eulerAngles.y;
                xDeg = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);
            }
        }

        // When in water, move towards camera direction, if right mouse button is pressed and player is not rotating the camera.
        if (WorldManager.Instance.isPlayerInWater && InputManager.RIGHT_MOUSE_PRESS && (InputManager.LEFT_MOUSE_PRESS || InputManager.UP_PRESS || InputManager.DOWN_PRESS))
        {
            target.position += transform.forward * Time.deltaTime;
        }

        // Calculate the desired distance.
        if (!MainManager.Instance.isChatBoxActive && !MainManager.Instance.isDraggingWindow) // Do not want to intervene with chat scrolling.
        {
            desiredDistance -= InputManager.AXIS_MOUSE_SCROLLWHEEL * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance) * speedDistance;
        }
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);

        yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);

        // Det camera rotation.
        Quaternion rotation = Quaternion.Euler(yDeg, xDeg, 0);
        correctedDistance = desiredDistance;

        // Calculate desired camera position.
        Vector3 vTargetOffset = new Vector3(0, -targetHeight, 0);
        Vector3 position = target.position - (rotation * Vector3.forward * desiredDistance + vTargetOffset);

        // Check for collision using the true target's desired registration point as set by user using height.
        Vector3 trueTargetPosition = new Vector3(target.position.x, target.position.y, target.position.z) - vTargetOffset;

        // If there was a collision, correct the camera position and calculate the corrected distance.
        bool isCorrected = false;
        if (Physics.Linecast(trueTargetPosition, position, out RaycastHit collisionHit, collisionLayers.value))
        {
            // Calculate the distance from the original estimated position to the collision location,
            // subtracting out a safety "offset" distance from the object we hit.  The offset will help
            // keep the camera from being right on top of the surface we hit, which usually shows up as
            // the surface geometry getting partially clipped by the camera's front clipping plane.
            correctedDistance = Vector3.Distance(trueTargetPosition, collisionHit.point) - offsetFromWall;
            isCorrected = true;
        }

        // For smoothing, lerp distance only if either distance wasn't corrected, or correctedDistance is more than currentDistance.
        currentDistance = !isCorrected || correctedDistance > currentDistance ? Mathf.Lerp(currentDistance, correctedDistance, Time.deltaTime * zoomDampening) : correctedDistance;

        // Keep within legal limits.
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

        // Recalculate position based on the new currentDistance.
        position = target.position - (rotation * Vector3.forward * currentDistance + vTargetOffset);

        transform.rotation = rotation;
        transform.position = position;
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }

    private void SetPlayerRotation(float newRotation)
    {
        Quaternion oldHeading = target.localRotation;
        Quaternion newHeading = target.localRotation;
        Vector3 curvAngle = newHeading.eulerAngles;
        curvAngle.y = newRotation;
        newHeading.eulerAngles = curvAngle;
        target.localRotation = Quaternion.Lerp(oldHeading, newHeading, Time.deltaTime * 10); // 10 is response time.
    }
}