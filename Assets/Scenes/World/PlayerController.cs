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
public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public class MoveSettings
    {
        public float forwardVel = 12;
        public float rotateVel = 100;
        public float jumpVel = 25;
        public float distToGrounded = 0.1f;
        public LayerMask ground;
    }

    [System.Serializable]
    public class PhysSettings
    {
        public float downAccel = 0.75f;
    }

    [System.Serializable]
    public class InputSettings
    {
        public float inputDelay = 0.1f;
        public string FORWARD_AXIS = "Vertical";
        public string TURN_AXIS = "Horizontal";
        public string JUMP_AXIS = "Jump";
    }

    public MoveSettings moveSetting = new MoveSettings();
    public PhysSettings physSetting = new PhysSettings();
    public InputSettings inputSetting = new InputSettings();

    Vector3 velocity = Vector3.zero;
    private Quaternion targetRotation;
    private Rigidbody rBody;
    private float forwardInput;
    private float turnInput;
    private float jumpInput;

    private float oldX = 0;
    private float oldY = 0;
    private float oldZ = 0;

    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }

    private bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, moveSetting.distToGrounded, moveSetting.ground);
    }

    private void Start()
    {
        targetRotation = transform.rotation;
        if (GetComponent<Rigidbody>())
        {
            rBody = GetComponent<Rigidbody>();
        }
        else
        {
            Debug.LogError("The character needs a rigidbody.");
        }
        forwardInput = 0;
        turnInput = 0;
        jumpInput = 0;
    }

    private void GetInput()
    {
        forwardInput = Input.GetAxis(inputSetting.FORWARD_AXIS); // Interpolated.
        turnInput = Input.GetAxis(inputSetting.TURN_AXIS); // Interpolated.
        jumpInput = Input.GetAxisRaw(inputSetting.JUMP_AXIS); // Non-interpolated.
    }

    private void Update()
    {
        GetInput();
        Turn();
    }

    private void FixedUpdate()
    {
        Run();
        Jump();

        rBody.velocity = transform.TransformDirection(velocity);

        // Send position to server.
        if (oldX != transform.position.x || oldY != transform.position.y || oldZ != transform.position.z)
        {
            NetworkManager.instance.ChannelSend(new LocationUpdate(transform.position.x, transform.position.y, transform.position.z));
            oldX = transform.position.x;
            oldY = transform.position.y;
            oldZ = transform.position.z;
        }
    }

    private void Run()
    {
        if (Mathf.Abs(forwardInput) > inputSetting.inputDelay)
        {
            // Move.
            velocity.z = moveSetting.forwardVel * forwardInput;
        }
        else
        {
            // Zero velocity.
            velocity.z = 0;
        }
    }

    private void Turn()
    {
        if (Mathf.Abs(turnInput) > inputSetting.inputDelay)
        {
            targetRotation *= Quaternion.AngleAxis(moveSetting.rotateVel * turnInput * Time.deltaTime, Vector3.up);
        }
        transform.rotation = targetRotation;
    }

    private void Jump()
    {
        if (jumpInput > 0 && Grounded())
        {
            // Jump.
            velocity.y = moveSetting.jumpVel;
        }
        else if (jumpInput == 0 && Grounded())
        {
            // Zero out velocity.y
            velocity.y = 0;
        }
        else
        {
            // Decrease velocity.y
            velocity.y -= physSetting.downAccel;
        }
    }
}
