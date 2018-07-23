using UnityEngine;

/**
 * @author Abdallah Azzami
 */
public class MouseCamera : MonoBehaviour
{
    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;
    public bool mouseAcive;

    private float rotY = 0.0f; // Rotation around the up/y axis.
    private float rotX = 0.0f; // Rotation around the right/x axis.

    void Awake()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        mouseAcive = true;
    }

    void Update()
    {
        if (mouseAcive && (Input.GetMouseButton(0) || Input.GetMouseButton(1)))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = -Input.GetAxis("Mouse Y");

            rotY += mouseX * mouseSensitivity * Time.deltaTime;
            rotX += mouseY * mouseSensitivity * Time.deltaTime;

            rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle + 30);

            Quaternion localRotation = Quaternion.Euler(rotX, rotY, 3.1f);
            transform.rotation = localRotation;
        }
    }

    public void DeActiveMouse()
    {
        mouseAcive = false;
    }

    public void ActiveMouse()
    {
        mouseAcive = true;
    }
}