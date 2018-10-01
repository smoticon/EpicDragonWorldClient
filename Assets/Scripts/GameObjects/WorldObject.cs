using System.Collections;
using UnityEngine;

/**
* @author Pantelis Andrianakis
*/
public class WorldObject : MonoBehaviour
{
    public long objectId;
    public Vector3 targetPos;
    public int curAnimState = 0;
    public int curVelocity = 0;
    Animator characAnimator;
    public float forwardVel = 4f;
    public bool isJump = false;
    int isWater = 0;
    float jumpDelayTime = 0.7f;
    public bool isInsidewater = false;
    PL_MOVE_ANIM_STATE animState = PL_MOVE_ANIM_STATE.PL_IDLE;

    private void Start()
    {
        if (characAnimator == null)
        {
            characAnimator = gameObject.GetComponent<Animator>();
        }
        targetPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(targetPos.x, targetPos.z)) > 0.1f)
        {
            if (animState == PL_MOVE_ANIM_STATE.PL_W)
            {
                transform.LookAt(targetPos);
            }
            Vector3 angle = transform.eulerAngles;
            angle.x = 0;
            angle.z = 0;
            transform.eulerAngles = angle;
            float step = forwardVel * Time.deltaTime;
            gameObject.GetComponent<Rigidbody>().MovePosition(Vector3.MoveTowards(transform.position, targetPos, step));
        }
        else if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(targetPos.x, targetPos.z)) <= 0.1f && (animState == PL_MOVE_ANIM_STATE.PL_IDLE && !characAnimator.GetBool("IsIdle")))
        {
            if (isWater > 0)
            {
                StopSwimming();
            }
            else if (!isJump)
            {
                StopMove();
            }
        }
    }

    public void SetSwimmingState(PL_MOVE_ANIM_STATE mState)
    {
        switch (mState)
        {
            case PL_MOVE_ANIM_STATE.PL_S:
                characAnimator.SetBool("IsWalkingBackwards", false);
                characAnimator.SetBool("IsIdle", false);
                characAnimator.SetBool("IsStandingJump", false);
                characAnimator.SetBool("IsFarJump", false);
                characAnimator.SetBool("IsRightTurning", false);
                characAnimator.SetBool("IsLeftTurning", false);
                characAnimator.SetBool("IsRunning", false);
                characAnimator.SetBool("IsNE", false);
                characAnimator.SetBool("IsW", false);
                characAnimator.SetBool("IsSW", false);
                characAnimator.SetBool("IsSE", false);
                characAnimator.SetBool("IsE", false);
                characAnimator.SetBool("IsNW", false);
                characAnimator.SetBool("IsSwimmingIdle", false);
                if (animState != mState || !characAnimator.GetBool("IsSwimming"))
                {
                    characAnimator.Play("Swimming");
                    characAnimator.SetBool("IsSwimming", true);
                    animState = mState;
                }
                break;

            case PL_MOVE_ANIM_STATE.PL_W:
                characAnimator.SetBool("IsWalkingBackwards", false);
                characAnimator.SetBool("IsIdle", false);
                characAnimator.SetBool("IsStandingJump", false);
                characAnimator.SetBool("IsFarJump", false);
                characAnimator.SetBool("IsRightTurning", false);
                characAnimator.SetBool("IsLeftTurning", false);
                characAnimator.SetBool("IsRunning", false);
                characAnimator.SetBool("IsSwimmingIdle", false);
                characAnimator.SetBool("IsNE", false);
                characAnimator.SetBool("IsW", false);
                characAnimator.SetBool("IsSW", false);
                characAnimator.SetBool("IsSE", false);
                characAnimator.SetBool("IsE", false);
                characAnimator.SetBool("IsNW", false);
                if (animState != mState || !characAnimator.GetBool("IsSwimming"))
                {
                    characAnimator.Play("Swimming");
                    characAnimator.SetBool("IsSwimming", true);
                    animState = mState;
                }
                break;

            case PL_MOVE_ANIM_STATE.PL_IDLE:
                characAnimator.SetBool("IsWalkingBackwards", false);
                characAnimator.SetBool("IsIdle", false);
                characAnimator.SetBool("IsStandingJump", false);
                characAnimator.SetBool("IsFarJump", false);
                characAnimator.SetBool("IsRightTurning", false);
                characAnimator.SetBool("IsLeftTurning", false);
                characAnimator.SetBool("IsRunning", false);
                characAnimator.SetBool("IsNE", false);
                characAnimator.SetBool("IsW", false);
                characAnimator.SetBool("IsSW", false);
                characAnimator.SetBool("IsSE", false);
                characAnimator.SetBool("IsE", false);
                characAnimator.SetBool("IsNW", false);
                characAnimator.SetBool("IsSwimming", false);
                if (animState != mState || !characAnimator.GetBool("IsSwimmingIdle"))
                {
                    characAnimator.Play("Treading Water");
                    characAnimator.SetBool("IsSwimmingIdle", true);
                    animState = mState;
                }
                break;

            default:
                characAnimator.SetBool("IsWalkingBackwards", false);
                characAnimator.SetBool("IsIdle", false);
                characAnimator.SetBool("IsStandingJump", false);
                characAnimator.SetBool("IsFarJump", false);
                characAnimator.SetBool("IsRightTurning", false);
                characAnimator.SetBool("IsLeftTurning", false);
                characAnimator.SetBool("IsRunning", false);
                characAnimator.SetBool("IsNE", false);
                characAnimator.SetBool("IsW", false);
                characAnimator.SetBool("IsSW", false);
                characAnimator.SetBool("IsSE", false);
                characAnimator.SetBool("IsE", false);
                characAnimator.SetBool("IsNW", false);
                characAnimator.SetBool("IsSwimmingIdle", false);
                if (animState != mState || !characAnimator.GetBool("IsSwimmingIdle"))
                {
                    characAnimator.Play("Swimming");
                    characAnimator.SetBool("IsSwimming", true);
                    animState = mState;
                }
                break;
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
                characAnimator.SetBool("IsWalkingBackwards", false);
                characAnimator.SetBool("IsIdle", false);
                characAnimator.SetBool("IsStandingJump", false);
                characAnimator.SetBool("IsFarJump", false);
                characAnimator.SetBool("IsRightTurning", false);
                characAnimator.SetBool("IsLeftTurning", false);
                characAnimator.SetBool("IsRunning", false);
                characAnimator.SetBool("IsNE", false);
                characAnimator.SetBool("IsSW", false);
                characAnimator.SetBool("IsSE", false);
                characAnimator.SetBool("IsE", false);
                characAnimator.SetBool("IsNW", false);
                characAnimator.SetBool("IsSwimming", false);
                characAnimator.SetBool("IsSwimmingIdle", false);
                if (animState != mState || !characAnimator.GetBool("IsW"))
                {
                    characAnimator.Play("W");
                    animState = mState;
                    characAnimator.SetBool("IsW", true);
                }
                break;

            case PL_MOVE_ANIM_STATE.PL_S:
                characAnimator.SetBool("IsIdle", false);
                characAnimator.SetBool("IsStandingJump", false);
                characAnimator.SetBool("IsFarJump", false);
                characAnimator.SetBool("IsRightTurning", false);
                characAnimator.SetBool("IsLeftTurning", false);
                characAnimator.SetBool("IsRunning", false);
                characAnimator.SetBool("IsNE", false);
                characAnimator.SetBool("IsW", false);
                characAnimator.SetBool("IsSW", false);
                characAnimator.SetBool("IsSE", false);
                characAnimator.SetBool("IsE", false);
                characAnimator.SetBool("IsNW", false);
                characAnimator.SetBool("IsSwimming", false);
                characAnimator.SetBool("IsSwimmingIdle", false);
                if (animState != mState || !characAnimator.GetBool("IsWalkingBackwards"))
                {
                    characAnimator.Play("Walking Backward");
                    characAnimator.SetBool("IsWalkingBackwards", true);
                    animState = mState;
                }
                break;

            case PL_MOVE_ANIM_STATE.PL_D:
                characAnimator.SetBool("IsWalkingBackwards", false);
                characAnimator.SetBool("IsIdle", false);
                characAnimator.SetBool("IsStandingJump", false);
                characAnimator.SetBool("IsFarJump", false);
                characAnimator.SetBool("IsRightTurning", false);
                characAnimator.SetBool("IsLeftTurning", false);
                characAnimator.SetBool("IsRunning", false);
                characAnimator.SetBool("IsNE", false);
                characAnimator.SetBool("IsW", false);
                characAnimator.SetBool("IsSW", false);
                characAnimator.SetBool("IsSE", false);
                characAnimator.SetBool("IsNW", false);
                characAnimator.SetBool("IsSwimming", false);
                characAnimator.SetBool("IsSwimmingIdle", false);
                if (animState != mState || !characAnimator.GetBool("IsE"))
                {
                    characAnimator.Play("E");
                    animState = mState;
                    characAnimator.SetBool("IsE", true);
                }
                break;

            case PL_MOVE_ANIM_STATE.PL_W:
                if (isJump)
                {
                    return;
                }
                transform.LookAt(targetPos);
                characAnimator.SetBool("IsWalkingBackwards", false);
                characAnimator.SetBool("IsIdle", false);
                characAnimator.SetBool("IsStandingJump", false);
                characAnimator.SetBool("IsFarJump", false);
                characAnimator.SetBool("IsRightTurning", false);
                characAnimator.SetBool("IsLeftTurning", false);
                characAnimator.SetBool("IsNE", false);
                characAnimator.SetBool("IsW", false);
                characAnimator.SetBool("IsSW", false);
                characAnimator.SetBool("IsSE", false);
                characAnimator.SetBool("IsE", false);
                characAnimator.SetBool("IsNW", false);
                characAnimator.SetBool("IsSwimming", false);
                characAnimator.SetBool("IsSwimmingIdle", false);
                if (animState != mState || !characAnimator.GetBool("IsRunning"))
                {
                    characAnimator.Play("Run");
                    characAnimator.SetBool("IsRunning", true);
                    animState = mState;
                }
                break;

            case PL_MOVE_ANIM_STATE.PL_AW:
                characAnimator.SetBool("IsWalkingBackwards", false);
                characAnimator.SetBool("IsIdle", false);
                characAnimator.SetBool("IsStandingJump", false);
                characAnimator.SetBool("IsFarJump", false);
                characAnimator.SetBool("IsRightTurning", false);
                characAnimator.SetBool("IsLeftTurning", false);
                characAnimator.SetBool("IsRunning", false);
                characAnimator.SetBool("IsNE", false);
                characAnimator.SetBool("IsW", false);
                characAnimator.SetBool("IsSW", false);
                characAnimator.SetBool("IsSE", false);
                characAnimator.SetBool("IsE", false);
                characAnimator.SetBool("IsSwimming", false);
                characAnimator.SetBool("IsSwimmingIdle", false);
                if (animState != mState || !characAnimator.GetBool("IsNW"))
                {
                    characAnimator.Play("NW1");
                    characAnimator.SetBool("IsNW", true);
                    animState = mState;
                }
                break;

            case PL_MOVE_ANIM_STATE.PL_WD:
                characAnimator.SetBool("IsWalkingBackwards", false);
                characAnimator.SetBool("IsIdle", false);
                characAnimator.SetBool("IsStandingJump", false);
                characAnimator.SetBool("IsFarJump", false);
                characAnimator.SetBool("IsRightTurning", false);
                characAnimator.SetBool("IsLeftTurning", false);
                characAnimator.SetBool("IsRunning", false);
                characAnimator.SetBool("IsW", false);
                characAnimator.SetBool("IsSW", false);
                characAnimator.SetBool("IsSE", false);
                characAnimator.SetBool("IsE", false);
                characAnimator.SetBool("IsNW", false);
                characAnimator.SetBool("IsSwimming", false);
                characAnimator.SetBool("IsSwimmingIdle", false);
                if (animState != mState || !characAnimator.GetBool("IsNE"))
                {
                    characAnimator.Play("NE");
                    characAnimator.SetBool("IsNE", true);
                    animState = mState;
                }
                break;

            case PL_MOVE_ANIM_STATE.PL_DS:
                characAnimator.SetBool("IsWalkingBackwards", false);
                characAnimator.SetBool("IsIdle", false);
                characAnimator.SetBool("IsStandingJump", false);
                characAnimator.SetBool("IsFarJump", false);
                characAnimator.SetBool("IsRightTurning", false);
                characAnimator.SetBool("IsLeftTurning", false);
                characAnimator.SetBool("IsRunning", false);
                characAnimator.SetBool("IsNE", false);
                characAnimator.SetBool("IsW", false);
                characAnimator.SetBool("IsSW", false);
                characAnimator.SetBool("IsE", false);
                characAnimator.SetBool("IsNW", false);
                characAnimator.SetBool("IsSwimming", false);
                characAnimator.SetBool("IsSwimmingIdle", false);
                if (animState != mState || !characAnimator.GetBool("IsSE"))
                {
                    characAnimator.Play("SE");
                    characAnimator.SetBool("IsSE", true);
                    animState = mState;
                }
                break;

            case PL_MOVE_ANIM_STATE.PL_SA:
                characAnimator.SetBool("IsWalkingBackwards", false);
                characAnimator.SetBool("IsIdle", false);
                characAnimator.SetBool("IsStandingJump", false);
                characAnimator.SetBool("IsFarJump", false);
                characAnimator.SetBool("IsRightTurning", false);
                characAnimator.SetBool("IsLeftTurning", false);
                characAnimator.SetBool("IsRunning", false);
                characAnimator.SetBool("IsNE", false);
                characAnimator.SetBool("IsW", false);
                characAnimator.SetBool("IsSE", false);
                characAnimator.SetBool("IsE", false);
                characAnimator.SetBool("IsNW", false);
                characAnimator.SetBool("IsSwimming", false);
                characAnimator.SetBool("IsSwimmingIdle", false);
                if (animState != mState || !characAnimator.GetBool("IsSW"))
                {
                    characAnimator.Play("SW");
                    animState = mState;
                    characAnimator.SetBool("IsSW", true);
                }
                break;

            case PL_MOVE_ANIM_STATE.PL_IDLE:
                characAnimator.SetBool("IsWalkingBackwards", false);
                characAnimator.SetBool("IsStandingJump", false);
                characAnimator.SetBool("IsFarJump", false);
                characAnimator.SetBool("IsRightTurning", false);
                characAnimator.SetBool("IsLeftTurning", false);
                characAnimator.SetBool("IsRunning", false);
                characAnimator.SetBool("IsNE", false);
                characAnimator.SetBool("IsW", false);
                characAnimator.SetBool("IsSW", false);
                characAnimator.SetBool("IsSE", false);
                characAnimator.SetBool("IsE", false);
                characAnimator.SetBool("IsNW", false);
                characAnimator.SetBool("IsSwimming", false);
                characAnimator.SetBool("IsSwimmingIdle", false);
                if (animState != mState || !characAnimator.GetBool("IsIdle"))
                {
                    if (animState == PL_MOVE_ANIM_STATE.PL_W)
                    {
                        characAnimator.Play("Standing Run Forward Stop");
                    }
                    else
                    {
                        characAnimator.Play("Idle");
                        characAnimator.SetBool("IsIdle", true);
                    }

                    animState = mState;
                }
                break;

            case PL_MOVE_ANIM_STATE.PL_JUMP:
                isJump = true;
                characAnimator.SetBool("IsWalkingBackwards", false);
                characAnimator.SetBool("IsIdle", false);
                characAnimator.SetBool("IsStandingJump", false);
                characAnimator.SetBool("IsFarJump", true);
                characAnimator.SetBool("IsRightTurning", false);
                characAnimator.SetBool("IsLeftTurning", false);
                characAnimator.SetBool("IsRunning", false);
                characAnimator.SetBool("IsNE", false);
                characAnimator.SetBool("IsW", false);
                characAnimator.SetBool("IsSW", false);
                characAnimator.SetBool("IsSE", false);
                characAnimator.SetBool("IsE", false);
                characAnimator.SetBool("IsNW", false);
                characAnimator.SetBool("IsSwimming", false);
                characAnimator.SetBool("IsSwimmingIdle", false);
                characAnimator.Play("Jump");
                animState = mState;
                StartCoroutine("StopJump");
                break;

            case PL_MOVE_ANIM_STATE.PL_STAND_JUMP:
                isJump = true;
                characAnimator.SetBool("IsWalkingBackwards", false);
                characAnimator.SetBool("IsIdle", false);
                characAnimator.SetBool("IsStandingJump", true);
                characAnimator.SetBool("IsFarJump", false);
                characAnimator.SetBool("IsRightTurning", false);
                characAnimator.SetBool("IsLeftTurning", false);
                characAnimator.SetBool("IsRunning", false);
                characAnimator.SetBool("IsNE", false);
                characAnimator.SetBool("IsW", false);
                characAnimator.SetBool("IsSW", false);
                characAnimator.SetBool("IsSE", false);
                characAnimator.SetBool("IsE", false);
                characAnimator.SetBool("IsNW", false);
                characAnimator.SetBool("IsSwimming", false);
                characAnimator.SetBool("IsSwimmingIdle", false);
                characAnimator.Play("Standing Jump");
                animState = mState;
                StartCoroutine("StopJump");
                break;

            default:
                characAnimator.SetBool("IsWalkingBackwards", false);
                characAnimator.SetBool("IsStandingJump", false);
                characAnimator.SetBool("IsFarJump", false);
                characAnimator.SetBool("IsRightTurning", false);
                characAnimator.SetBool("IsLeftTurning", false);
                characAnimator.SetBool("IsRunning", false);
                characAnimator.SetBool("IsNE", false);
                characAnimator.SetBool("IsW", false);
                characAnimator.SetBool("IsSW", false);
                characAnimator.SetBool("IsSE", false);
                characAnimator.SetBool("IsE", false);
                characAnimator.SetBool("IsNW", false);
                characAnimator.SetBool("IsSwimming", false);
                characAnimator.SetBool("IsSwimmingIdle", false);
                if (animState != mState || !characAnimator.GetBool("IsIdle"))
                {
                    if (animState == PL_MOVE_ANIM_STATE.PL_W)
                    {
                        characAnimator.Play("Standing Run Forward Stop");
                    }
                    else
                    {
                        characAnimator.Play("Idle");
                        characAnimator.SetBool("IsIdle", true);
                    }

                    animState = PL_MOVE_ANIM_STATE.PL_IDLE;
                }
                break;
        }
    }

    private void StopMove()
    {
        if (characAnimator.GetBool("IsRunning"))
        {
            characAnimator.Play("Standing Run Forward Stop");
        }
        else
        {
            characAnimator.Play("Idle");
        }
        characAnimator.SetBool("IsWalkingBackwards", false);
        characAnimator.SetBool("IsIdle", true);
        characAnimator.SetBool("IsStandingJump", false);
        characAnimator.SetBool("IsFarJump", false);
        characAnimator.SetBool("IsRightTurning", false);
        characAnimator.SetBool("IsLeftTurning", false);
        characAnimator.SetBool("IsRunning", false);
        characAnimator.SetBool("IsNE", false);
        characAnimator.SetBool("IsW", false);
        characAnimator.SetBool("IsSW", false);
        characAnimator.SetBool("IsSE", false);
        characAnimator.SetBool("IsE", false);
        characAnimator.SetBool("IsNW", false);
    }

    private void StopSwimming()
    {
        characAnimator.SetBool("IsWalkingBackwards", false);
        characAnimator.SetBool("IsIdle", false);
        characAnimator.SetBool("IsStandingJump", false);
        characAnimator.SetBool("IsFarJump", false);
        characAnimator.SetBool("IsRightTurning", false);
        characAnimator.SetBool("IsLeftTurning", false);
        characAnimator.SetBool("IsRunning", false);
        characAnimator.SetBool("IsNE", false);
        characAnimator.SetBool("IsW", false);
        characAnimator.SetBool("IsSW", false);
        characAnimator.SetBool("IsSE", false);
        characAnimator.SetBool("IsE", false);
        characAnimator.SetBool("IsNW", false);
        if (animState != PL_MOVE_ANIM_STATE.PL_IDLE || !characAnimator.GetBool("IsSwimmingIdle"))
        {
            characAnimator.Play("Treading Water");
            characAnimator.SetBool("IsSwimmingIdle", true);
            animState = PL_MOVE_ANIM_STATE.PL_IDLE;
        }
    }

    private IEnumerator StopJump()
    {
        if (characAnimator.GetBool("IsStandingJump"))
        {
            jumpDelayTime = 0.7f;
        }
        else
        {
            jumpDelayTime = 0.5f;
        }
        yield return new WaitForSeconds(jumpDelayTime);
        characAnimator.SetBool("IsWalkingBackwards", false);
        characAnimator.SetBool("IsStandingJump", false);
        characAnimator.SetBool("IsFarJump", false);
        characAnimator.SetBool("IsRightTurning", false);
        characAnimator.SetBool("IsLeftTurning", false);
        characAnimator.SetBool("IsRunning", false);
        SetAnimState(animState);
        isJump = false;
    }

    public void PlayAnimation(Vector3 movePos, float heading, int animId, int wState)
    {
        targetPos = movePos;
        if (isWater != wState)
        {
            if (wState > 0)
            {
                SetSwimmingState(PL_MOVE_ANIM_STATE.PL_IDLE);
            }
            else
            {
                SetAnimState(PL_MOVE_ANIM_STATE.PL_IDLE);
            }
        }
        isWater = wState;
        if (Mathf.Abs(heading - transform.localRotation.eulerAngles.y) > 3f)
        {
            if (animState != PL_MOVE_ANIM_STATE.PL_W)
            {
                Quaternion curHeading = transform.localRotation;
                Vector3 curvAngle = curHeading.eulerAngles;
                curvAngle.y = heading;
                curHeading.eulerAngles = curvAngle;
                transform.localRotation = curHeading;
            }
        }
        if (wState > 0)
        {
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            if ((PL_MOVE_ANIM_STATE)animId == PL_MOVE_ANIM_STATE.PL_IDLE)
            {
                animState = PL_MOVE_ANIM_STATE.PL_IDLE;
            }
            else
            {
                SetSwimmingState((PL_MOVE_ANIM_STATE)animId);
            }
        }
        else
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            if (animState != (PL_MOVE_ANIM_STATE)animId || (PL_MOVE_ANIM_STATE)animId == PL_MOVE_ANIM_STATE.PL_JUMP || (PL_MOVE_ANIM_STATE)animId == PL_MOVE_ANIM_STATE.PL_STAND_JUMP)
            {
                if ((PL_MOVE_ANIM_STATE)animId == PL_MOVE_ANIM_STATE.PL_IDLE)
                {
                    animState = PL_MOVE_ANIM_STATE.PL_IDLE;
                }
                else if ((animState == PL_MOVE_ANIM_STATE.PL_JUMP || animState == PL_MOVE_ANIM_STATE.PL_STAND_JUMP) && isJump)
                {
                    targetPos = movePos;
                }
                else
                {
                    SetAnimState((PL_MOVE_ANIM_STATE)animId);
                }
            }
        }
    }
}