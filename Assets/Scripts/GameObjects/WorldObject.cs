using System.Collections;
using UnityEngine;

/**
* @author Pantelis Andrianakis
*/
public class WorldObject : MonoBehaviour
{
    public long objectId;
    private bool isNew = true; // Used to avoid animation stuck on idle, when new moving object is entering visibily radius.
    private bool isJump = false;
    private int currentWaterState = 0;
    private Vector3 targetPos;
    private Animator animController;
    private float forwardVel = 6f; // PlayerController forwardVel * 1.5 to sync players.
    private float jumpDelayTime = 0.7f;
    private PL_MOVE_ANIM_STATE animState = PL_MOVE_ANIM_STATE.PL_IDLE;

    private void Start()
    {
        if (animController == null)
        {
            animController = gameObject.GetComponent<Animator>();
        }
        targetPos = transform.position;

        // Remove isNew flag.
        StartCoroutine("RemoveIsNew");
    }

    private IEnumerator RemoveIsNew()
    {
        yield return new WaitForSeconds(0.3f);
        isNew = false;
    }

    private void Update()
    {
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(targetPos.x, targetPos.z)) > 0.2f)
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
        else if (animState == PL_MOVE_ANIM_STATE.PL_IDLE && !animController.GetBool("IsIdle"))
        {
            if (currentWaterState > 0)
            {
                SetSwimmingState(PL_MOVE_ANIM_STATE.PL_IDLE);
            }
            else
            {
                SetAnimState(PL_MOVE_ANIM_STATE.PL_IDLE);
            }
        }
    }

    public void PlayAnimation(Vector3 movePos, float heading, int animId, int waterState)
    {
        targetPos = movePos;
        // Heading.
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
        // Water.
        if (currentWaterState != waterState)
        {
            if (waterState > 0)
            {
                SetSwimmingState(PL_MOVE_ANIM_STATE.PL_IDLE);
            }
            else
            {
                SetAnimState(PL_MOVE_ANIM_STATE.PL_IDLE);
            }
        }
        currentWaterState = waterState;
        if (waterState > 0)
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
        else // Normal movement.
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            if (animState != (PL_MOVE_ANIM_STATE)animId || (PL_MOVE_ANIM_STATE)animId == PL_MOVE_ANIM_STATE.PL_JUMP || (PL_MOVE_ANIM_STATE)animId == PL_MOVE_ANIM_STATE.PL_STAND_JUMP || isNew)
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