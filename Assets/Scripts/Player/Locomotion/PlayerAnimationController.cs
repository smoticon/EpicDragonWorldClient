using System.Collections;
using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: September 25th 2018
 */
public class PlayerAnimationController : MonoBehaviour
{
    private bool isJump = false;
    private Animator animController;
    private float jumpDelayTime = 0.7f;
    public PL_MOVE_ANIM_STATE animState = PL_MOVE_ANIM_STATE.PL_IDLE;

    private void Start()
    {
        if (animController == null)
        {
            animController = transform.gameObject.GetComponent<Animator>();
        }
    }

    public void SetAnimState(PL_MOVE_ANIM_STATE mState)
    {
        if (isJump)
        {
            return;
        }

        switch (mState)
        {
            case PL_MOVE_ANIM_STATE.PL_A:
                animController.SetBool("IsWalkingBackwards", false);
                animController.SetBool("IsIdle", false);
                animController.SetBool("IsStandingJump", false);
                animController.SetBool("IsFarJump", false);
                animController.SetBool("IsRightTurning", false);
                animController.SetBool("IsLeftTurning", false);
                animController.SetBool("IsRunning", false);
                animController.SetBool("IsNE", false);
                animController.SetBool("IsSW", false);
                animController.SetBool("IsSE", false);
                animController.SetBool("IsE", false);
                animController.SetBool("IsNW", false);
                animController.SetBool("IsSwimming", false);
                animController.SetBool("IsSwimmingIdle", false);
                animController.Play("W");
                animController.SetBool("IsW", true);
                animState = mState;
                break;

            case PL_MOVE_ANIM_STATE.PL_S:
                animController.SetBool("IsIdle", false);
                animController.SetBool("IsStandingJump", false);
                animController.SetBool("IsFarJump", false);
                animController.SetBool("IsRightTurning", false);
                animController.SetBool("IsLeftTurning", false);
                animController.SetBool("IsRunning", false);
                animController.SetBool("IsNE", false);
                animController.SetBool("IsW", false);
                animController.SetBool("IsSW", false);
                animController.SetBool("IsSE", false);
                animController.SetBool("IsE", false);
                animController.SetBool("IsNW", false);
                animController.SetBool("IsSwimming", false);
                animController.SetBool("IsSwimmingIdle", false);
                animController.Play("Walking Backward");
                animController.SetBool("IsWalkingBackwards", true);
                animState = mState;
                break;

            case PL_MOVE_ANIM_STATE.PL_D:
                animController.SetBool("IsWalkingBackwards", false);
                animController.SetBool("IsIdle", false);
                animController.SetBool("IsStandingJump", false);
                animController.SetBool("IsFarJump", false);
                animController.SetBool("IsRightTurning", false);
                animController.SetBool("IsLeftTurning", false);
                animController.SetBool("IsRunning", false);
                animController.SetBool("IsNE", false);
                animController.SetBool("IsW", false);
                animController.SetBool("IsSW", false);
                animController.SetBool("IsSE", false);
                animController.SetBool("IsNW", false);
                animController.SetBool("IsSwimming", false);
                animController.SetBool("IsSwimmingIdle", false);
                animController.Play("E");
                animController.SetBool("IsE", true);
                animState = mState;
                break;

            case PL_MOVE_ANIM_STATE.PL_W:
                animController.SetBool("IsWalkingBackwards", false);
                animController.SetBool("IsIdle", false);
                animController.SetBool("IsStandingJump", false);
                animController.SetBool("IsFarJump", false);
                animController.SetBool("IsRightTurning", false);
                animController.SetBool("IsLeftTurning", false);
                animController.SetBool("IsNE", false);
                animController.SetBool("IsW", false);
                animController.SetBool("IsSW", false);
                animController.SetBool("IsSE", false);
                animController.SetBool("IsE", false);
                animController.SetBool("IsNW", false);
                animController.SetBool("IsSwimming", false);
                animController.SetBool("IsSwimmingIdle", false);
                animController.Play("Run");
                animController.SetBool("IsRunning", true);
                animState = mState;
                break;

            case PL_MOVE_ANIM_STATE.PL_AW:
                animController.SetBool("IsWalkingBackwards", false);
                animController.SetBool("IsIdle", false);
                animController.SetBool("IsStandingJump", false);
                animController.SetBool("IsFarJump", false);
                animController.SetBool("IsRightTurning", false);
                animController.SetBool("IsLeftTurning", false);
                animController.SetBool("IsRunning", false);
                animController.SetBool("IsNE", false);
                animController.SetBool("IsW", false);
                animController.SetBool("IsSW", false);
                animController.SetBool("IsSE", false);
                animController.SetBool("IsE", false);
                animController.SetBool("IsSwimming", false);
                animController.SetBool("IsSwimmingIdle", false);
                animController.Play("NW1");
                animController.SetBool("IsNW", true);
                animState = mState;
                break;

            case PL_MOVE_ANIM_STATE.PL_WD:
                animController.SetBool("IsWalkingBackwards", false);
                animController.SetBool("IsIdle", false);
                animController.SetBool("IsStandingJump", false);
                animController.SetBool("IsFarJump", false);
                animController.SetBool("IsRightTurning", false);
                animController.SetBool("IsLeftTurning", false);
                animController.SetBool("IsRunning", false);
                animController.SetBool("IsW", false);
                animController.SetBool("IsSW", false);
                animController.SetBool("IsSE", false);
                animController.SetBool("IsE", false);
                animController.SetBool("IsNW", false);
                animController.SetBool("IsSwimming", false);
                animController.SetBool("IsSwimmingIdle", false);
                animController.Play("NE");
                animController.SetBool("IsNE", true);
                animState = mState;
                break;

            case PL_MOVE_ANIM_STATE.PL_DS:
                animController.SetBool("IsWalkingBackwards", false);
                animController.SetBool("IsIdle", false);
                animController.SetBool("IsStandingJump", false);
                animController.SetBool("IsFarJump", false);
                animController.SetBool("IsRightTurning", false);
                animController.SetBool("IsLeftTurning", false);
                animController.SetBool("IsRunning", false);
                animController.SetBool("IsNE", false);
                animController.SetBool("IsW", false);
                animController.SetBool("IsSW", false);
                animController.SetBool("IsE", false);
                animController.SetBool("IsNW", false);
                animController.SetBool("IsSwimming", false);
                animController.SetBool("IsSwimmingIdle", false);
                animController.Play("SE");
                animController.SetBool("IsSE", true);
                animState = mState;
                break;

            case PL_MOVE_ANIM_STATE.PL_SA:
                animController.SetBool("IsWalkingBackwards", false);
                animController.SetBool("IsIdle", false);
                animController.SetBool("IsStandingJump", false);
                animController.SetBool("IsFarJump", false);
                animController.SetBool("IsRightTurning", false);
                animController.SetBool("IsLeftTurning", false);
                animController.SetBool("IsRunning", false);
                animController.SetBool("IsNE", false);
                animController.SetBool("IsW", false);
                animController.SetBool("IsSE", false);
                animController.SetBool("IsE", false);
                animController.SetBool("IsNW", false);
                animController.SetBool("IsSwimming", false);
                animController.SetBool("IsSwimmingIdle", false);
                animController.Play("SW");
                animController.SetBool("IsSW", true);
                animState = mState;
                break;

            case PL_MOVE_ANIM_STATE.PL_JUMP:
                if (isJump)
                {
                    return;
                }
                isJump = true;
                animController.SetBool("IsWalkingBackwards", false);
                animController.SetBool("IsIdle", false);
                animController.SetBool("IsStandingJump", false);
                animController.SetBool("IsFarJump", true);
                animController.SetBool("IsRightTurning", false);
                animController.SetBool("IsLeftTurning", false);
                animController.SetBool("IsRunning", false);
                animController.SetBool("IsNE", false);
                animController.SetBool("IsW", false);
                animController.SetBool("IsSW", false);
                animController.SetBool("IsSE", false);
                animController.SetBool("IsE", false);
                animController.SetBool("IsNW", false);
                animController.SetBool("IsSwimming", false);
                animController.SetBool("IsSwimmingIdle", false);
                animController.Play("Jump");
                animState = mState;
                StartCoroutine("StopJump");
                break;

            case PL_MOVE_ANIM_STATE.PL_STAND_JUMP:
                if (isJump)
                {
                    return;
                }
                isJump = true;
                animController.SetBool("IsWalkingBackwards", false);
                animController.SetBool("IsIdle", false);
                animController.SetBool("IsStandingJump", true);
                animController.SetBool("IsFarJump", false);
                animController.SetBool("IsRightTurning", false);
                animController.SetBool("IsLeftTurning", false);
                animController.SetBool("IsRunning", false);
                animController.SetBool("IsNE", false);
                animController.SetBool("IsW", false);
                animController.SetBool("IsSW", false);
                animController.SetBool("IsSE", false);
                animController.SetBool("IsE", false);
                animController.SetBool("IsNW", false);
                animController.SetBool("IsSwimming", false);
                animController.SetBool("IsSwimmingIdle", false);
                animController.Play("Standing Jump");
                animState = mState;
                StartCoroutine("StopJump");
                break;

            case PL_MOVE_ANIM_STATE.PL_IDLE:
            default:
                if (animController == null)
                {
                    return;
                }
                animController.SetBool("IsWalkingBackwards", false);
                animController.SetBool("IsStandingJump", false);
                animController.SetBool("IsFarJump", false);
                animController.SetBool("IsRightTurning", false);
                animController.SetBool("IsLeftTurning", false);
                animController.SetBool("IsRunning", false);
                animController.SetBool("IsNE", false);
                animController.SetBool("IsW", false);
                animController.SetBool("IsSW", false);
                animController.SetBool("IsSE", false);
                animController.SetBool("IsE", false);
                animController.SetBool("IsNW", false);
                animController.SetBool("IsSwimming", false);
                animController.SetBool("IsSwimmingIdle", false);
                animController.SetBool("IsIdle", true);
                // animController.Play("Idle");
                animState = PL_MOVE_ANIM_STATE.PL_IDLE;
                break;
        }
    }

    public void SetSwimmingState(PL_MOVE_ANIM_STATE mState)
    {
        switch (mState)
        {
            case PL_MOVE_ANIM_STATE.PL_S:
                animController.SetBool("IsWalkingBackwards", false);
                animController.SetBool("IsIdle", false);
                animController.SetBool("IsStandingJump", false);
                animController.SetBool("IsFarJump", false);
                animController.SetBool("IsRightTurning", false);
                animController.SetBool("IsLeftTurning", false);
                animController.SetBool("IsRunning", false);
                animController.SetBool("IsNE", false);
                animController.SetBool("IsW", false);
                animController.SetBool("IsSW", false);
                animController.SetBool("IsSE", false);
                animController.SetBool("IsE", false);
                animController.SetBool("IsNW", false);
                animController.SetBool("IsSwimmingIdle", false);
                animController.SetBool("IsSwimming", true);
                // animController.Play("Swimming");
                animState = mState;
                break;

            case PL_MOVE_ANIM_STATE.PL_IDLE:
                animController.SetBool("IsWalkingBackwards", false);
                animController.SetBool("IsIdle", false);
                animController.SetBool("IsStandingJump", false);
                animController.SetBool("IsFarJump", false);
                animController.SetBool("IsRightTurning", false);
                animController.SetBool("IsLeftTurning", false);
                animController.SetBool("IsRunning", false);
                animController.SetBool("IsNE", false);
                animController.SetBool("IsW", false);
                animController.SetBool("IsSW", false);
                animController.SetBool("IsSE", false);
                animController.SetBool("IsE", false);
                animController.SetBool("IsNW", false);
                animController.SetBool("IsSwimming", false);
                animController.SetBool("IsSwimmingIdle", true);
                // animController.Play("Treading Water");
                animState = mState;
                break;

            case PL_MOVE_ANIM_STATE.PL_W:
            default:
                animController.SetBool("IsWalkingBackwards", false);
                animController.SetBool("IsIdle", false);
                animController.SetBool("IsStandingJump", false);
                animController.SetBool("IsFarJump", false);
                animController.SetBool("IsRightTurning", false);
                animController.SetBool("IsLeftTurning", false);
                animController.SetBool("IsRunning", false);
                animController.SetBool("IsNE", false);
                animController.SetBool("IsW", false);
                animController.SetBool("IsSW", false);
                animController.SetBool("IsSE", false);
                animController.SetBool("IsE", false);
                animController.SetBool("IsNW", false);
                animController.SetBool("IsSwimmingIdle", false);
                animController.SetBool("IsSwimming", true);
                // animController.Play("Swimming");
                animState = mState;
                break;
        }
    }

    private IEnumerator StopJump()
    {
        if (animController.GetBool("IsStandingJump"))
        {
            jumpDelayTime = 0.65f;
        }
        else
        {
            jumpDelayTime = 0.7f;
        }
        yield return new WaitForSeconds(jumpDelayTime);
        animController.SetBool("IsWalkingBackwards", false);
        animController.SetBool("IsStandingJump", false);
        animController.SetBool("IsFarJump", false);
        animController.SetBool("IsRightTurning", false);
        animController.SetBool("IsLeftTurning", false);
        animController.SetBool("IsRunning", false);
        isJump = false;
    }
}
