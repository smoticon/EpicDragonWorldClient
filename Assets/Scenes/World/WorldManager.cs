using System;
using System.Collections;
using UnityEngine;

/**
 * @author Pantelis Andrianakis
 */
public class WorldManager : MonoBehaviour
{
    [HideInInspector]
    public GameObject playerCharacter;
    public GameObject playerMale;
    public GameObject playerFemale;
    public GameObject cameraMale;
    public GameObject cameraFemale;
    [HideInInspector]
    public static WorldManager instance;
    [HideInInspector]
    ArrayList gameObjects = ArrayList.Synchronized(new ArrayList());
    [HideInInspector]
    private static readonly int visibilityRadius = 10000; // This is the maximum allowed visibility radius.

    private void Start()
    {
        if (NetworkManager.instance == null) // Offline mode.
        {
            // Set player model to male.
            playerMale.SetActive(true);
            cameraMale.SetActive(true);
            playerCharacter = playerMale;

            // Set position.
            playerCharacter.transform.position = new Vector3(9945.9f, 9.2f, 10534.9f); // Spawn location.
        }
        else // Online mode.
        {
            // Return if account name is empty.
            if (PlayerManager.instance == null || PlayerManager.instance.accountName == null)
            {
                return; // Return to login?
            }

            // Set instance.
            instance = this;

            // Change music.
            MusicManager.instance.PlayMusic(MusicManager.instance.EnterWorld);

            // Set player model.
            if (PlayerManager.instance.selectedCharacterData.GetClassId() == 0) // Male.
            {
                playerMale.SetActive(true);
                cameraMale.SetActive(true);
                playerCharacter = playerMale;
            }
            if (PlayerManager.instance.selectedCharacterData.GetClassId() == 1) // Female.
            {
                playerFemale.SetActive(true);
                cameraFemale.SetActive(true);
                playerCharacter = playerFemale;
            }

            // Set position.
            playerCharacter.transform.position = new Vector3(PlayerManager.instance.selectedCharacterData.GetX(), PlayerManager.instance.selectedCharacterData.GetY(), PlayerManager.instance.selectedCharacterData.GetZ());

            // Set heading.
            Quaternion curHeading = transform.localRotation;
            Vector3 curvAngle = curHeading.eulerAngles;
            curvAngle.y = PlayerManager.instance.selectedCharacterData.GetHeading();
            curHeading.eulerAngles = curvAngle;
            transform.localRotation = curHeading;

            // Request world info from server.
            NetworkManager.instance.ChannelSend(new EnterWorldRequest(PlayerManager.instance.selectedCharacterData.GetName()));
        }
    }

    public void UpdateObject(long objectId, int classId, string name, float posX, float posY, float posZ, float heading)
    {
        // Check for existing objects.
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.GetComponent<WorldObject>().objectId == objectId)
            {
                // TODO: Update object info.
                return;
            }
        }

        // Object is out of sight.
        Vector3 position = new Vector3(posX, posY, posZ);
        if (CalculateDistance(position) > visibilityRadius)
        {
            return;
        }

        // Object does not exist. Instantiate.
        GameObject obj = Instantiate(GameObjectManager.instance.gameObjectList[classId], position, Quaternion.identity) as GameObject;

        // Assign object id.
        obj.AddComponent<WorldObject>();
        obj.GetComponent<WorldObject>().objectId = objectId;

        // Add RigidBody.
        Rigidbody rigidBody = obj.AddComponent<Rigidbody>();
        rigidBody.mass = 1;
        rigidBody.angularDrag = 0.05f;
        rigidBody.freezeRotation = true;

        // Set heading.
        Quaternion curHeading = transform.localRotation;
        Vector3 curvAngle = curHeading.eulerAngles;
        curvAngle.y = heading;
        curHeading.eulerAngles = curvAngle;
        transform.localRotation = curHeading;

		// TODO: Show name.

        // TODO: Proper appearance.

        // Add to game object list.
        gameObjects.Add(obj);
    }

    public void MoveObject(long objectId, float posX, float posY, float posZ, float heading, int animState, int waterState)
    {
        Vector3 position = new Vector3(posX, posY, posZ);
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.GetComponent<WorldObject>().objectId == objectId)
            {
                if (CalculateDistance(position) > visibilityRadius) // Moved out of sight.
                {
                    DeleteObject(gameObject);
                }
                else
                {
                    gameObject.GetComponent<WorldObject>().PlayAnimation(position, heading, animState, waterState);
                }
                return;
            }
        }

        // Request unknown object info from server.
        if (CalculateDistance(position) <= visibilityRadius)
        {
            NetworkManager.instance.ChannelSend(new ObjectInfoRequest(objectId));
        }
    }

    private void DeleteObject(GameObject gameObject)
    {
        // Remove from objects list.
        gameObjects.Remove(gameObject);

        // Delete game object from world.
        Destroy(gameObject);
    }

    public void DeleteObject(long objectId)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.GetComponent<WorldObject>().objectId == objectId)
            {
                DeleteObject(gameObject);
                return;
            }
        }
    }

    // Calculate distance between player and a Vector3 location.
    public double CalculateDistance(Vector3 vector)
    {
        return Math.Pow(playerCharacter.transform.position.x - vector.x, 2) + Math.Pow(playerCharacter.transform.position.y - vector.y, 2) + Math.Pow(playerCharacter.transform.position.z - vector.z, 2);
    }
}
