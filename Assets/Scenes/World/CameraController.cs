/*
 * This file is part of the Epic Dragon World project.
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
 */
using UnityEngine;

/**
 * @author Pantelis Andrianakis
 */
public class CameraController : MonoBehaviour
{
    public Transform target;
    public float lookSmooth = 0.09f;
    public Vector3 offsetFromTarget = new Vector3(0, 6, -8);
    public float xTilt = 10;

    private Vector3 destination = Vector3.zero;
    private PlayerController charControler;
    // private float rotateVel = 0;

    private void Start()
    {
        SetCameraTarget(target);
    }

    // Can be used to set a target for the camera to look at.
    public void SetCameraTarget(Transform t)
    {
        target = t;
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
        // Moving.
        MoveToTarget();
        // Rotating.
        LookAtTarget();
    }

    private void MoveToTarget()
    {
        destination = charControler.TargetRotation * offsetFromTarget;
        destination += target.position;
        transform.position = destination;
    }

    private void LookAtTarget()
    {
        // Follow camera effect.
        // float eulerYAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref rotateVel, lookSmooth);
        // transform.rotation = Quaternion.Euler(transform.eulerAngles.x, eulerYAngle, 0);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, target.eulerAngles.y, 0);
    }
}
