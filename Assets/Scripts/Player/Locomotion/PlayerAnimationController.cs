using UnityEngine;
using System.Collections;

/**
 * @author DaeChol
 */
public class PlayerAnimationController : MonoBehaviour
{
    public Animator animController;
    public bool isJump = false;
    public PL_MOVE_ANIM_STATE curMoveState = PL_MOVE_ANIM_STATE.PL_IDLE;
    float jumpDelayTime = 0.7f;

    private void Start()
    {
        if (animController == null)
        {
            animController = transform.gameObject.GetComponent<Animator>();
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
        curMoveState = PL_MOVE_ANIM_STATE.PL_W;
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
        curMoveState = PL_MOVE_ANIM_STATE.PL_S;
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
        if (curMoveState != PL_MOVE_ANIM_STATE.PL_IDLE || !animController.GetBool("IsIdle"))
        {
            if (curMoveState == PL_MOVE_ANIM_STATE.PL_W)
            {
                animController.Play("Standing Run Forward Stop");
            }
            else
            {
                animController.Play("Idle");
            }
            animController.SetBool("IsIdle", true);
        }
        curMoveState = PL_MOVE_ANIM_STATE.PL_IDLE;
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

    private IEnumerator StopJumping()
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
        isJump = false;
        animController.SetBool("IsWalkingBackwards", false);
        animController.SetBool("IsStandingJump", false);
        animController.SetBool("IsFarJump", false);
        animController.SetBool("IsRightTurning", false);
        animController.SetBool("IsLeftTurning", false);
        animController.SetBool("IsRunning", false);
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
                curMoveState = mState;
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
                curMoveState = mState;
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
                curMoveState = mState;
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
                curMoveState = mState;
                break;
        }
    }

    public void SetMoveState(PL_MOVE_ANIM_STATE mState)
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
                curMoveState = mState;
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
                curMoveState = mState;
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
                curMoveState = mState;
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
                curMoveState = mState;
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
                curMoveState = mState;
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
                animController.SetBool("IsNW", true);
                animController.Play("NE");
                animController.SetBool("IsNE", true);
                curMoveState = mState;
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
                curMoveState = mState;
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
                curMoveState = mState;
                break;

            case PL_MOVE_ANIM_STATE.PL_IDLE:
                StopMoving();
                break;

            default:
                StopMoving();
                break;
        }
    }
}
