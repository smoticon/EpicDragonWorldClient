using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: January 15th 2019
 */
public class AnimationController : MonoBehaviour
{
    // Static values.
    private readonly float MAX_VELOCITY = 10;
    private readonly float VELOCITY_STEP = 2;
    public readonly static string VELOCITY_X_VALUE = "VelocityX";
    public readonly static string VELOCITY_Z_VALUE = "VelocityZ";
    public readonly static string IS_GROUNDED_VALUE = "IsGrounded";
    public readonly static string IS_IN_WATER_VALUE = "IsInWater";
    public readonly static string TRIGGER_JUMP_VALUE = "TriggerJump";
    // Non-static values.
    private Animator animator;
    private AudioSource audioSource;
    private float currentVelocityX;
    private float currentVelocityZ;
    private bool lockedMovement = false;
    private bool sideMovement = false;
    // Values for comparison.
    private float previousVelocityX = 0;
    private float previousVelocityZ = 0;
    private bool triggerJump = false;
    private bool lastWaterState = false;
    private bool lastGroundedState = false;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.applyRootMotion = true;
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void LateUpdate()
    {
        // Update values.
        animator.SetBool(IS_GROUNDED_VALUE, WorldManager.Instance.isPlayerOnTheGround);
        animator.SetBool(IS_IN_WATER_VALUE, WorldManager.Instance.isPlayerInWater);

        // Do nothing when chat is active.
        if (MainManager.Instance.isChatBoxActive)
        {
            if (lockedMovement)
            {
                currentVelocityZ = Mathf.Min(MAX_VELOCITY, currentVelocityZ + VELOCITY_STEP);
            }
            else
            {
                currentVelocityZ = 0;
            }
            currentVelocityX = 0;
            animator.SetFloat(VELOCITY_Z_VALUE, currentVelocityZ);
            animator.SetFloat(VELOCITY_X_VALUE, currentVelocityX);
        }
        else
        {
            // Update values.
            currentVelocityX = animator.GetFloat(VELOCITY_X_VALUE);
            currentVelocityZ = animator.GetFloat(VELOCITY_Z_VALUE);

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
                    // Nothing planned for now.
                }
                else if (WorldManager.Instance.isPlayerOnTheGround)
                {
                    animator.SetTrigger(TRIGGER_JUMP_VALUE);
                    triggerJump = true;
                }
            }

            // Front.
            if (InputManager.UP_PRESS || lockedMovement || sideMovement)
            {
                currentVelocityZ = Mathf.Min(MAX_VELOCITY, currentVelocityZ + VELOCITY_STEP);
            }
            // Back.
            else if (InputManager.DOWN_PRESS)
            {
                currentVelocityZ = Mathf.Max(-MAX_VELOCITY, currentVelocityZ - VELOCITY_STEP);
            }
            else // Reduce VelocityZ speed.
            {
                if (currentVelocityZ > 0)
                {
                    currentVelocityZ -= VELOCITY_STEP;
                }
                else if (currentVelocityZ < 0)
                {
                    currentVelocityZ += VELOCITY_STEP;
                }
            }
            // Set VelocityZ value.
            animator.SetFloat(VELOCITY_Z_VALUE, currentVelocityZ);

            // Left.
            if (InputManager.LEFT_PRESS && (InputManager.UP_PRESS || InputManager.DOWN_PRESS))
            {
                currentVelocityX = Mathf.Max(MAX_VELOCITY, currentVelocityX - VELOCITY_STEP);
            }
            // Right.
            else if (InputManager.RIGHT_PRESS && (InputManager.UP_PRESS || InputManager.DOWN_PRESS))
            {
                currentVelocityX = Mathf.Min(-MAX_VELOCITY, currentVelocityX + VELOCITY_STEP);
            }
            else // Reduce VelocityX speed.
            {
                if (currentVelocityX > 0)
                {
                    currentVelocityX -= VELOCITY_STEP;
                }
                else if (currentVelocityX < 0)
                {
                    currentVelocityX += VELOCITY_STEP;
                }
            }
            // Set VelocityX value.
            animator.SetFloat(VELOCITY_X_VALUE, currentVelocityX);
        }

        // Send changes to network.
        if (previousVelocityX != currentVelocityX //
            || previousVelocityZ != currentVelocityZ //
            || triggerJump //
            || lastWaterState != WorldManager.Instance.isPlayerInWater //
            || lastGroundedState != WorldManager.Instance.isPlayerOnTheGround)
        {
            NetworkManager.ChannelSend(new AnimatorUpdateRequest(currentVelocityX, currentVelocityZ, triggerJump, WorldManager.Instance.isPlayerInWater, WorldManager.Instance.isPlayerOnTheGround));
            // Store last sent values.
            previousVelocityX = currentVelocityX;
            previousVelocityZ = currentVelocityZ;
            triggerJump = false;
            lastWaterState = WorldManager.Instance.isPlayerInWater;
            lastGroundedState = WorldManager.Instance.isPlayerOnTheGround;
        }
    }

    // Triggered by animation run_fwd at frames 7 and 16.
    private void StepSound()
    {
        audioSource.volume = OptionsManager.Instance.GetSfxVolume();
        audioSource.PlayOneShot(SoundManager.Instance.FOOTSTEP_SOUND, 1);
    }
}
