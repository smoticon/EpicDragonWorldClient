using System;
using System.Collections.Concurrent;
using UMA.CharacterSystem;
using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: January 7th 2019
 */
public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance { get; private set; }

    [HideInInspector]
    public DynamicCharacterAvatar activeCharacter;
    [HideInInspector]
    public bool isPlayerInWater = false;
    [HideInInspector]
    public bool isPlayerOnTheGround = false;
    [HideInInspector]
    BlockingCollection<GameObject> gameObjects = new BlockingCollection<GameObject>();
    [HideInInspector]
    private static readonly int visibilityRadius = 10000; // This is the maximum allowed visibility radius.

    private void Start()
    {
        Instance = this;

        if (MainManager.Instance.selectedCharacterData != null)
        {
            // Create player character.
            activeCharacter = CharacterManager.Instance.CreateCharacter(MainManager.Instance.selectedCharacterData);

            // Set camera target.
            CameraController.Instance.target = activeCharacter.transform;

            // Animations.
            activeCharacter.gameObject.AddComponent<AnimationController>();

            // Movement.
            activeCharacter.gameObject.AddComponent<MovementController>();

            // Add name text.
            WorldObjectText worldObjectText = activeCharacter.gameObject.AddComponent<WorldObjectText>();
            worldObjectText.worldObjectName = ""; // MainManager.Instance.selectedCharacterData.GetName();
            worldObjectText.attachedObject = activeCharacter.gameObject;

            // Send enter world to Network.
            NetworkManager.ChannelSend(new EnterWorldRequest(MainManager.Instance.selectedCharacterData.GetName()));
        }
    }

    public void UpdateObject(long objectId, CharacterDataHolder characterdata)
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
        if (CalculateDistance(new Vector3(characterdata.GetX(), characterdata.GetY(), characterdata.GetZ())) > visibilityRadius)
        {
            return;
        }

        // Object does not exist. Instantiate.
        GameObject obj = CharacterManager.Instance.CreateCharacter(characterdata).gameObject;

        // Assign object id and name.
        WorldObject worldObject = obj.AddComponent<WorldObject>();
        worldObject.objectId = objectId;
        WorldObjectText worldObjectText = obj.AddComponent<WorldObjectText>();
        worldObjectText.worldObjectName = characterdata.GetName();
        worldObjectText.attachedObject = obj;

        // Add to game object list.
        gameObjects.Add(obj);
    }

    public void MoveObject(long objectId, float posX, float posY, float posZ, float heading)
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
                    gameObject.GetComponent<WorldObject>().MoveObject(position, heading);
                }
                return;
            }
        }

        // Request unknown object info from server.
        if (CalculateDistance(position) <= visibilityRadius)
        {
            NetworkManager.ChannelSend(new ObjectInfoRequest(objectId));
        }
    }

    public void AnimateObject(long objectId, float velocityX, float velocityZ, bool triggerJump, bool isInWater, bool isGrounded)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            WorldObject worldObject = gameObject.GetComponent<WorldObject>();
            if (worldObject.objectId == objectId)
            {
                if (CalculateDistance(gameObject.transform.position) <= visibilityRadius) // Object is in sight radius.
                {
                    worldObject.AnimateObject(velocityX, velocityZ, triggerJump, isInWater, isGrounded);
                }
                return;
            }
        }
    }

    // Calculate distance between player and a Vector3 location.
    public double CalculateDistance(Vector3 vector)
    {
        return Math.Pow(activeCharacter.transform.position.x - vector.x, 2) + Math.Pow(activeCharacter.transform.position.y - vector.y, 2) + Math.Pow(activeCharacter.transform.position.z - vector.z, 2);
    }

    private void DeleteObject(GameObject gameObject)
    {
        // Remove from objects list.
        gameObjects.TryTake(out gameObject);

        // Delete game object from world.
        Destroy(gameObject.GetComponent<WorldObjectText>().nameMesh.gameObject);
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

    public void ExitWorld()
    {
        if (activeCharacter != null)
        {
            Destroy(activeCharacter.GetComponent<WorldObjectText>().nameMesh.gameObject);
            Destroy(activeCharacter.gameObject);
        }
        isPlayerInWater = false;
        isPlayerOnTheGround = false;
        foreach (GameObject gameObject in gameObjects)
        {
            Destroy(gameObject.GetComponent<WorldObjectText>().nameMesh.gameObject);
            Destroy(gameObject);
        }
    }
}
