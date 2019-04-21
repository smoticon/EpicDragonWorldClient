using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
    public ConcurrentDictionary<long, GameObject> gameObjects;
    [HideInInspector]
    public static readonly int VISIBILITY_RADIUS = 10000; // This is the maximum allowed visibility radius.

    static readonly object updateLock = new object();

    private void Start()
    {
        Instance = this;

        gameObjects = new ConcurrentDictionary<long, GameObject>();

        // Start music.
        MusicManager.Instance.PlayMusic(MusicManager.Instance.EnterWorld);

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
        lock (updateLock) // Use lock to avoid adding duplicate gameObjects.
        {
            // Check for existing objects.
            if (gameObjects.ContainsKey(objectId))
            {
                // TODO: Update object info.
                return;
            }

            // Object is out of sight.
            if (CalculateDistance(new Vector3(characterdata.GetX(), characterdata.GetY(), characterdata.GetZ())) > VISIBILITY_RADIUS)
            {
                return;
            }

            // Object does not exist. Instantiate.
            GameObject newObj = CharacterManager.Instance.CreateCharacter(characterdata).gameObject;

            // Add to game object list.
            gameObjects.TryAdd(objectId, newObj);

            // Assign object id and name.
            WorldObject worldObject = newObj.AddComponent<WorldObject>();
            worldObject.objectId = objectId;
            WorldObjectText worldObjectText = newObj.AddComponent<WorldObjectText>();
            worldObjectText.worldObjectName = characterdata.GetName();
            worldObjectText.attachedObject = newObj;
        }
    }

    public void MoveObject(long objectId, float posX, float posY, float posZ, float heading)
    {
        lock (updateLock)
        {
            Vector3 position = new Vector3(posX, posY, posZ);
            if (gameObjects.ContainsKey(objectId))
            {
                GameObject obj = gameObjects[objectId];
                if (obj != null)
                {
                    WorldObject worldObject = obj.GetComponent<WorldObject>();
                    if (worldObject != null)
                    {
                        if (worldObject.GetDistance() > VISIBILITY_RADIUS) // Moved out of sight.
                        {
                            // Broadcast self position, object out of sight.
                            NetworkManager.ChannelSend(new LocationUpdateRequest(MovementController.storedPosition.x, MovementController.storedPosition.y, MovementController.storedPosition.z, MovementController.storedRotation));
                            DeleteObject(obj);
                        }
                        else
                        {
                            worldObject.MoveObject(position, heading);
                        }
                    }
                }
                return;
            }

            // Request unknown object info from server.
            if (CalculateDistance(position) <= VISIBILITY_RADIUS)
            {
                NetworkManager.ChannelSend(new ObjectInfoRequest(objectId));
                // Broadcast self position, in case player is not moving.
                NetworkManager.ChannelSend(new LocationUpdateRequest(MovementController.storedPosition.x, MovementController.storedPosition.y, MovementController.storedPosition.z, MovementController.storedRotation));
            }
        }
    }

    public void AnimateObject(long objectId, float velocityX, float velocityZ, bool triggerJump, bool isInWater, bool isGrounded)
    {
        lock (updateLock)
        {
            if (gameObjects.ContainsKey(objectId))
            {
                GameObject obj = gameObjects[objectId];
                if (obj != null)
                {
                    WorldObject worldObject = obj.GetComponent<WorldObject>();
                    if (worldObject != null)
                    {
                        if (worldObject.GetDistance() <= VISIBILITY_RADIUS) // Object is in sight radius.
                        {
                            worldObject.AnimateObject(velocityX, velocityZ, triggerJump, isInWater, isGrounded);
                        }
                    }
                }
            }
        }
    }

    // Calculate distance between player and a Vector3 location.
    public double CalculateDistance(Vector3 vector)
    {
        return Math.Pow(MovementController.storedPosition.x - vector.x, 2) + Math.Pow(MovementController.storedPosition.y - vector.y, 2) + Math.Pow(MovementController.storedPosition.z - vector.z, 2);
    }

    private IEnumerator DelayedDestroy(GameObject obj)
    {
        yield return new WaitForSeconds(0.5f);

        // Delete game object from world.
        Destroy(obj.GetComponent<WorldObjectText>().nameMesh.gameObject);
        Destroy(obj);
    }

    private void DeleteObject(GameObject obj)
    {
            // Disable.
            obj.GetComponent<WorldObjectText>().nameMesh.gameObject.SetActive(false);
            obj.SetActive(false);

            // Remove from objects list.
            ((IDictionary<long, GameObject>)gameObjects).Remove(obj.GetComponent<WorldObject>().objectId);

            // Delete game object from world with a delay.
            StartCoroutine(DelayedDestroy(obj));
    }

    public void DeleteObject(long objectId)
    {
        lock (updateLock)
        {
            if (gameObjects.ContainsKey(objectId))
            {
                GameObject obj = gameObjects[objectId];
                if (obj != null)
                {
                    DeleteObject(obj);
                }
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
        foreach (GameObject obj in gameObjects.Values)
        {
            if (obj == null)
            {
                continue;
            }

            Destroy(obj.GetComponent<WorldObjectText>().nameMesh.gameObject);
            Destroy(obj);
        }
    }
}
