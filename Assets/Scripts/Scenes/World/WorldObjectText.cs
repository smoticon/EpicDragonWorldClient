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
    private int raceId;
    private bool isInWater;

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
        if (attachedObject == null || gameObject == null || !attachedObject.activeSelf || !gameObject.activeSelf)
        {
            return;
        }

        // Reset information in case of unknown or changed race.
        currentHeight = 1f;
        if (worldObject == null)
        {
            raceId = MainManager.Instance.selectedCharacterData.GetRace();
            isInWater = WorldManager.Instance.isPlayerInWater;
        }
        else
        {
            raceId = worldObject.characterData.GetRace();
            isInWater = worldObject.isInWater;
        }

        // Height based on race.
        switch (raceId)
        {
            case 0:
                currentHeight = 1f;
                break;

            case 1:
                currentHeight = 0.85f;
                break;
        }

        // When in water, reduce height.
        if (isInWater)
        {
            currentHeight -= 0.3f;
        }

        nameMesh.text = worldObjectName;
        nameMesh.transform.position = new Vector3(attachedObject.transform.position.x, attachedObject.transform.position.y + attachedObject.transform.lossyScale.y + currentHeight, attachedObject.transform.position.z);
        nameMesh.transform.LookAt(CameraController.Instance.transform.position);
        nameMesh.transform.Rotate(0, 180, 0);
    }
}
