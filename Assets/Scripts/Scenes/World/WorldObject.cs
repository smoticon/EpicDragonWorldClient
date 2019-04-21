using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: June 11th 2018
 */
public class WorldObject : MonoBehaviour
{
    public long objectId;
    private double distance = 0;

    private Animator animator;
    private Rigidbody rigidBody;
    private readonly float VELOCITY = 6;

    // Is grounded related.
    public volatile bool isGrounded = false;

    // Is in water related.
    public volatile bool isInWater = false;

    // Sound related.
    private AudioSource audioSource;
    private static readonly float SOUND_DISTANCE = 1000;

    private void Start()
    {
        distance = WorldManager.Instance.CalculateDistance(transform.position);
        animator = GetComponent<Animator>();
        animator.applyRootMotion = true;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void MoveObject(Vector3 newPosition, float heading)
    {
        if (gameObject == null || !gameObject.activeSelf)
        {
            return;
        }

        float step = VELOCITY * Time.deltaTime;
        rigidBody.MovePosition(Vector3.Lerp(transform.position, newPosition, step));

        Quaternion curHeading = transform.localRotation;
        Vector3 curvAngle = curHeading.eulerAngles;
        curvAngle.y = heading;
        curHeading.eulerAngles = curvAngle;
        transform.localRotation = curHeading;

        // Update distance value.
        distance = WorldManager.Instance.CalculateDistance(transform.position);

        // Set audioSource volume based on distance.
        audioSource.volume = 1 - (float)(distance / SOUND_DISTANCE);

        // Animation related sounds.
        if (distance < SOUND_DISTANCE)
        {
            // Movement footstep sounds.
            if (!audioSource.isPlaying && rigidBody.velocity.magnitude > 2 && isGrounded)
            {
                audioSource.PlayOneShot(SoundManager.Instance.FOOTSTEP_SOUND, 1);
            }
        }
    }

    public void AnimateObject(float velocityX, float velocityZ, bool triggerJump, bool isInWater, bool isGrounded)
    {
        if (gameObject == null || !gameObject.activeSelf)
        {
            return;
        }

        this.isGrounded = isGrounded;
        this.isInWater = isInWater;
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

    public bool IsObjectGrounded()
    {
        return isGrounded;
    }

    public bool IsObjectInWater()
    {
        return isInWater;
    }

    public double GetDistance()
    {
        return distance;
    }
}