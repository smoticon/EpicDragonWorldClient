using System.Collections;
using UnityEngine;

/**
* @author Pantelis Andrianakis
*/
public class PlayerAnimationController : MonoBehaviour
{
    public Animator animController;
    public bool isJump = false;
    float jumpDelayTime = 0.7f;
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
        switch (mState)
        {
            case PL_MOVE_ANIM_STATE.PL_A:
                if (isJump)
                {
                    return;
                }
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
                animController.Play("E");
                animController.SetBool("IsE", true);
                animState = mState;
                break;

            case PL_MOVE_ANIM_STATE.PL_W:
                if (isJump)
                {
                    return;
                }
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
                animController.Play("SW");
                animController.SetBool("IsSW", true);
                animState = mState;
                break;

            case PL_MOVE_ANIM_STATE.PL_IDLE:
                StopMoving();
                break;

            default:
                StopMoving();
                break;
        }
    }

    public void MoveForward()
    {
        if (animController == null || isJump)
        {
            return;
        }
        animController.SetBool("IsRunning", true);
        animController.SetBool("IsIdle", false);
        animController.SetBool("IsStandingJump", false);
        animController.SetBool("IsFarJump", false);
        animController.SetBool("IsRightTurning", false);
        animController.SetBool("IsLeftTurning", false);
        animController.SetBool("IsWalkingBackwards", false);
        animController.SetBool("IsNE", false);
        animController.SetBool("IsW", false);
        animController.SetBool("IsSW", false);
        animController.SetBool("IsSE", false);
        animController.SetBool("IsE", false);
        animController.SetBool("IsNW", false);
        animController.Play("Run");
        animState = PL_MOVE_ANIM_STATE.PL_W;
    }

    public void MoveBackWards()
    {
        if (animController == null || isJump)
        {
            return;
        }
        animController.SetBool("IsWalkingBackwards", true);
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
        animController.Play("Walking Backward");
        animState = PL_MOVE_ANIM_STATE.PL_S;
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
                animController.Play("Swimming");
                animController.SetBool("IsSwimming", true);
                animState = mState;
                break;

            case PL_MOVE_ANIM_STATE.PL_W:
                animController.SetBool("IsWalkingBackwards", false);
                animController.SetBool("IsIdle", false);
                animController.SetBool("IsStandingJump", false);
                animController.SetBool("IsFarJump", false);
                animController.SetBool("IsRightTurning", false);
                animController.SetBool("IsLeftTurning", false);
                animController.SetBool("IsRunning", false);
                animController.SetBool("IsSwimmingIdle", false);
                animController.SetBool("IsNE", false);
                animController.SetBool("IsW", false);
                animController.SetBool("IsSW", false);
                animController.SetBool("IsSE", false);
                animController.SetBool("IsE", false);
                animController.SetBool("IsNW", false);
                animController.Play("Swimming");
                animController.SetBool("IsSwimming", true);
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
                animController.Play("Treading Water");
                animController.SetBool("IsSwimmingIdle", true);
                animState = mState;
                break;

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
                animController.Play("Swimming");
                animController.SetBool("IsSwimming", true);
                animState = mState;
                break;
        }
    }

    public void StopMoving()
    {
        if (animController == null || isJump)
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
        if (animState != PL_MOVE_ANIM_STATE.PL_IDLE || !animController.GetBool("IsIdle"))
        {
            if (animState == PL_MOVE_ANIM_STATE.PL_W)
            {
                animController.Play("Standing Run Forward Stop");
            }
            else
            {
                animController.Play("Idle");
            }
            animController.SetBool("IsIdle", true);
        }
        animState = PL_MOVE_ANIM_STATE.PL_IDLE;
    }

    private IEnumerator StopMovement()
    {
        animController.Play("Standing Run Forward Stop");
        yield return new WaitForSeconds(0.4f);
    }

    public void Jump(bool jumpState)
    {
        if (isJump)
        {
            return;
        }
        isJump = true;

        animController.SetBool("IsWalkingBackwards", false);
        animController.SetBool("IsIdle", false);
        animController.SetBool("IsRightTurning", false);
        animController.SetBool("IsLeftTurning", false);
        animController.SetBool("IsRunning", false);
        if (jumpState)
        {
            animController.SetBool("IsFarJump", true);
            animController.SetBool("IsStandingJump", false);
            animController.Play("Jump");
        }
        else
        {
            animController.SetBool("IsFarJump", false);
            animController.SetBool("IsStandingJump", true);
            animController.Play("Standing Jump");
        }
        StartCoroutine("StopJumping");
    }

    private IEnumerator StopJump()
    {
        if (animController.GetBool("IsStandingJump"))
        {
            jumpDelayTime = 0.7f;
        }
        else
        {
            jumpDelayTime = 0.5f;
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
