using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: February 9th 2019
 */
public class WorldObjectText : MonoBehaviour
{
    public GameObject attachedObject;
    public TextMesh nameMesh;
    public string worldObjectName = "";
    private readonly float NAME_HEIGHT = 1f;

    private void Start()
    {
        GameObject newGameObject = new GameObject();

        nameMesh = newGameObject.AddComponent<TextMesh>();
        nameMesh.text = "";
        nameMesh.characterSize = 0.02f;
        nameMesh.anchor = TextAnchor.MiddleCenter;
        nameMesh.alignment = TextAlignment.Center;
        nameMesh.fontSize = 100;
        nameMesh.fontStyle = FontStyle.Bold;
        nameMesh.color = new Color32(0, 255, 0, 255);
    }

    private void LateUpdate()
    {
        if (attachedObject == null || gameObject == null || !attachedObject.activeSelf || !gameObject.activeSelf)
        {
            return;
        }

        nameMesh.text = worldObjectName;
        nameMesh.transform.position = new Vector3(attachedObject.transform.position.x, attachedObject.transform.position.y + attachedObject.transform.lossyScale.y + NAME_HEIGHT, attachedObject.transform.position.z);
        nameMesh.transform.LookAt(CameraController.Instance.transform.position);
        nameMesh.transform.Rotate(0, 180, 0);
    }
}
