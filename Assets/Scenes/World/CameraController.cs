using UnityEngine;

/**
 * @author Pantelis Andrianakis, Abdallah Azzami
 */
public class CameraController : MonoBehaviour
{
    public Transform[] target;
    public float lookSmooth = 0.09f;
    public Vector3 offsetFromTarget = new Vector3(0, 6, -8);
    public float xTilt = 10;

    private Vector3 destination = Vector3.zero;
    private PlayerController charControler;
    private bool canRotate = false;
    private Transform mainTarget;

    private void Start()
    {
        SetCameraTarget(target[0]);
        mainTarget = target[0]; // The player is the main target for the camera.
    }

    // Can be used to set a target for the camera to look at.
    public void SetCameraTarget(Transform target)
    {
        if (target != null)
        {
            if (target.GetComponent<PlayerController>())
            {
                charControler = target.GetComponent<PlayerController>();
            }
            else
            {
                Debug.LogError("The camera's target needs a character controller.");
            }
        }
        else
        {
            Debug.LogError("Your camera needs a target.");
        }
    }

    private void LateUpdate()
    {
        SelectTarget();
        // Moving.
        MoveToTarget();
        // Rotating.
        LookAtTarget();
    }

    private void MoveToTarget()
    {
        MouseWheelToZoom();

        destination = charControler.TargetRotation * offsetFromTarget;
        destination += mainTarget.position;
        transform.position = destination;
    }

    private void LookAtTarget()
    {
        // Follow camera effect.
        // float eulerYAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref rotateVel, lookSmooth);
        // transform.rotation = Quaternion.Euler(transform.eulerAngles.x, eulerYAngle, 0);
        // transform.LookAt(mainTarget);

        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, mainTarget.eulerAngles.y, 0);
    }

    void SelectTarget()
    {
        target[1].position = target[0].transform.position;

        if (Input.GetMouseButtonUp(0))
        {
            canRotate = true;
        }
        else if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonUp(1))
        {
            canRotate = false;
        }

        if ((Input.GetMouseButton(0) && !Input.GetMouseButton(1)) || canRotate)
        {
            mainTarget = target[1]; // Object target is the main taeget for the camera.
        }
        else
        {
            mainTarget = target[0];  // The player is the main target for the camera.
        }
    }

    void MouseWheelToZoom()
    {
        float zoom = Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 150;
        if (offsetFromTarget.z <= -1 && offsetFromTarget.z >= -6.5f)
        {
            offsetFromTarget.z += zoom;
            if (offsetFromTarget.z >= -1)
            {
                offsetFromTarget.z = -1;
            }
            else if (offsetFromTarget.z <= -6.5f)
            {
                offsetFromTarget.z = -6.5f;
            }
        }
        if (offsetFromTarget.y <= 2.64f && offsetFromTarget.y >= 1.5f)
        {
            offsetFromTarget.y += -zoom / 5;
            if (offsetFromTarget.y >= 2.64f)
            {
                offsetFromTarget.y = 2.64f;
            }
            else if (offsetFromTarget.y <= 1.5f)
            {
                offsetFromTarget.y = 1.5f;
            }
        }
    }
}
