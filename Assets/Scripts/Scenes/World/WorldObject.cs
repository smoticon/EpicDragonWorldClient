using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: June 11th 2018
 */
public class WorldObject : MonoBehaviour
{
    public long objectId;

    private Animator animator;
    private Rigidbody rigidBody;
    private readonly float VELOCITY = 6;

    // Is grounded related.
    public bool isGrounded = false;
    private static LayerMask layerGround;
    private static readonly float GROUND_DISTANCE = 0.1f;
    private static readonly string LAYER_GROUND_VALUE = "Everything";

    // Sound related.
    private AudioSource audioSource;
    private static readonly float SOUND_DISTANCE = 1000;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.applyRootMotion = true;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = gameObject.GetComponent<AudioSource>();
        layerGround = LayerMask.NameToLayer(LAYER_GROUND_VALUE);
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

        // Update isGrounded value.
        isGrounded = Physics.Raycast(rigidBody.transform.position, Vector3.down, GROUND_DISTANCE, layerGround);

        // Set audioSource volume based on distance.
        double distance = WorldManager.Instance.CalculateDistance(transform.position);
        audioSource.volume = 1 - (float) (distance / SOUND_DISTANCE);

        // Animation related sounds.
        if (isGrounded && rigidBody.velocity.magnitude > 2 && distance < SOUND_DISTANCE && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(SoundManager.Instance.FOOTSTEP_SOUND, 1);
        }
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

    public bool isObjectGrounded()
    {
        return isGrounded;
    }
}