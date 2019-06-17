using TMPro;
using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: February 9th 2019
 */
public class WorldObjectText : MonoBehaviour
{
    public GameObject attachedObject;
    public WorldObject worldObject;
    public TextMeshPro nameMesh;
    public string worldObjectName = "";
    private float currentHeight;

    private void Start()
    {
        GameObject newGameObject = new GameObject();

        nameMesh = newGameObject.AddComponent<TextMeshPro>();
        nameMesh.text = "";
        nameMesh.alignment = TextAlignmentOptions.Center;
        nameMesh.fontSize = 1.5f;
        nameMesh.color = new Color32(0, 255, 0, 255);
    }

    private void LateUpdate()
    {
        if (attachedObject == null || gameObject == null || !attachedObject.activeSelf || !gameObject.activeSelf || worldObject == null)
        {
            return;
        }

        currentHeight = 1f; // Reset in case of unknown race.
        switch (worldObject.characterData.GetRace())
        {
            case 0:
                currentHeight = 1f;
                break;

            case 1:
                currentHeight = 0.85f;
                break;
        }
        // When in water, reduce height.
        if (worldObject.isInWater)
        {
            currentHeight -= 0.3f;
        }

        nameMesh.text = " " + worldObjectName; // TODO: Check why text is slightly aligned to the left.
        nameMesh.transform.position = new Vector3(attachedObject.transform.position.x, attachedObject.transform.position.y + attachedObject.transform.lossyScale.y + currentHeight, attachedObject.transform.position.z);
        nameMesh.transform.LookAt(CameraController.Instance.transform.position);
        nameMesh.transform.Rotate(0, 180, 0);
    }
}
