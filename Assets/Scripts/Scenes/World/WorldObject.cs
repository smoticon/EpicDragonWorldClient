using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: June 11th 2018
 */
public class WorldObject : MonoBehaviour
{
    public long objectId;
    private readonly float VELOCITY = 6;
    private Animator animator;
    private Rigidbody rigidBody;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.applyRootMotion = true;
        rigidBody = GetComponent<Rigidbody>();
    }

    internal void MoveObject(Vector3 newPosition, float heading)
    {
        float step = VELOCITY * Time.deltaTime;
        rigidBody.MovePosition(Vector3.Lerp(transform.position, newPosition, step));

        Quaternion curHeading = transform.localRotation;
        Vector3 curvAngle = curHeading.eulerAngles;
        curvAngle.y = heading;
        curHeading.eulerAngles = curvAngle;
        transform.localRotation = curHeading;
    }

    internal void AnimateObject(float velocityX, float velocityZ, bool triggerJump, bool isInWater, bool isGrounded)
    {
        rigidBody.useGravity = !isInWater;
        animator.SetBool(AnimationController.IS_GROUNDED_VALUE, isGrounded);
        animator.SetBool(AnimationController.IS_IN_WATER_VALUE, isInWater);
        animator.SetFloat(AnimationController.VELOCITY_Z_VALUE, velocityZ);
        animator.SetFloat(AnimationController.VELOCITY_X_VALUE, velocityX);
        if (triggerJump)
        {
            animator.SetTrigger(AnimationController.TRIGGER_JUMP_VALUE);
        }
    }
}