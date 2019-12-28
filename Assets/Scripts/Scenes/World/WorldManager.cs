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
    public WorldObject targetWorldObject;
    [HideInInspector]
    public bool isPlayerInWater = false;
    [HideInInspector]
    public bool isPlayerOnTheGround = false;
    [HideInInspector]
    public bool kickFromWorld = false;
    [HideInInspector]
    private bool exitingWorld = false;
    [HideInInspector]
    public ConcurrentDictionary<long, GameObject> gameObjects;
    [HideInInspector]
    public ConcurrentDictionary<long, MovementHolder> moveQueue;
    [HideInInspector]
    public ConcurrentDictionary<long, AnimationHolder> animationQueue;
    [HideInInspector]
    public ConcurrentDictionary<long, CharacterDataHolder> characterUpdateQueue;
    [HideInInspector]
    public List<long> deleteQueue;
    [HideInInspector]
    public static readonly int VISIBILITY_RADIUS = 10000; // This is the maximum allowed visibility radius.

    public static readonly object updateObjectLock = new object();
    private static readonly object updateMethodLock = new object();

    private void Start()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;

        gameObjects = new ConcurrentDictionary<long, GameObject>();
        moveQueue = new ConcurrentDictionary<long, MovementHolder>();
        animationQueue = new ConcurrentDictionary<long, AnimationHolder>();
        characterUpdateQueue = new ConcurrentDictionary<long, CharacterDataHolder>();
        deleteQueue = new List<long>();

        if (MainManager.Instance.selectedCharacterData != null)
        {
            // Create player character.
            activeCharacter = CharacterManager.Instance.CreateCharacter(MainManager.Instance.selectedCharacterData);
            // Tempfix.
            // TODO: Find why character initializes at 0, 0.
            // TODO: Try initialize with delay?
            Destroy(activeCharacter.gameObject);
            activeCharacter = CharacterManager.Instance.CreateCharacter(MainManager.Instance.selectedCharacterData);

            // Animations.
            activeCharacter.gameObject.AddComponent<AnimationController>();

            // Movement.
            activeCharacter.gameObject.AddComponent<MovementController>();

            // Add name text.
            WorldObjectText worldObjectText = activeCharacter.gameObject.AddComponent<WorldObjectText>();
            worldObjectText.worldObjectName = ""; // MainManager.Instance.selectedCharacterData.GetName();
            worldObjectText.attachedObject = activeCharacter.gameObject;

            // Update status information.
            StatusInformationManager.Instance.UpdatePlayerInformation();

            // Send enter world to Network.
            NetworkManager.ChannelSend(new EnterWorldRequest(MainManager.Instance.selectedCharacterData.GetName()));
        }
    }

    private void Update()
    {
        if (exitingWorld)
        {
            return;
        }
        if (kickFromWorld)
        {
            ExitWorld();
            MainManager.Instance.LoadScene(MainManager.LOGIN_SCENE);
            return;
        }

        lock (updateMethodLock)
        {
            // Distance check.
            foreach (GameObject obj in gameObjects.Values)
            {
                if (obj != null && CalculateDistance(obj.transform.position) > VISIBILITY_RADIUS)
                {
                    WorldObject worldObject = obj.GetComponent<WorldObject>();
                    if (worldObject != null && !deleteQueue.Contains(worldObject.objectId))
                    {
                        deleteQueue.Add(worldObject.objectId);
                    }
                }
            }

            // Delete pending objects.
            foreach (long objectId in deleteQueue)
            {
                if (gameObjects.ContainsKey(objectId))
                {
                    GameObject obj = gameObjects[objectId];
                    if (obj != null)
                    {
                        // Disable.
                        obj.GetComponent<WorldObjectText>().nameMesh.gameObject.SetActive(false);
                        obj.SetActive(false);

                        // Remove from objects list.
                        ((IDictionary<long, GameObject>)gameObjects).Remove(obj.GetComponent<WorldObject>().objectId);

                        // Delete game object from world with a delay.
                        StartCoroutine(DelayedDestroy(obj));
                    }
                }

                // If object was current target, unselect it.
                if (targetWorldObject != null && targetWorldObject.objectId == objectId)
                {
                    SetTarget(null);
                }
            }
            if (deleteQueue.Count > 0)
            {
                deleteQueue.Clear();
            }

            // Move pending objects.
            foreach (KeyValuePair<long, MovementHolder> entry in moveQueue)
            {
                Vector3 position = new Vector3(entry.Value.posX, entry.Value.posY, entry.Value.posZ);
                if (gameObjects.ContainsKey(entry.Key))
                {
                    GameObject obj = gameObjects[entry.Key];
                    if (obj != null)
                    {
                        WorldObject worldObject = obj.GetComponent<WorldObject>();
                        if (worldObject != null)
                        {
                            if (CalculateDistance(position) > VISIBILITY_RADIUS) // Moved out of sight.
                            {
                                // Broadcast self position, object out of sight.
                                NetworkManager.ChannelSend(new LocationUpdateRequest(MovementController.storedPosition.x, MovementController.storedPosition.y, MovementController.storedPosition.z, MovementController.storedRotation));
                                deleteQueue.Add(worldObject.objectId);
                            }
                            else
                            {
                                worldObject.MoveObject(position, entry.Value.heading);
                            }
                        }
                    }
                }
                // Check self teleporting.
                else if (entry.Key == 0)
                {
                    activeCharacter.transform.localPosition = position;
                    activeCharacter.GetComponent<Rigidbody>().position = position;
                }
                // Request unknown object info from server.
                else if (CalculateDistance(position) <= VISIBILITY_RADIUS)
                {
                    NetworkManager.ChannelSend(new ObjectInfoRequest(entry.Key));
                    // Broadcast self position, in case player is not moving.
                    NetworkManager.ChannelSend(new LocationUpdateRequest(MovementController.storedPosition.x, MovementController.storedPosition.y, MovementController.storedPosition.z, MovementController.storedRotation));
                }

                ((IDictionary<long, MovementHolder>)moveQueue).Remove(entry.Key);
            }

            // Animate pending objects.
            foreach (KeyValuePair<long, AnimationHolder> entry in animationQueue)
            {
                if (gameObjects.ContainsKey(entry.Key))
                {
                    GameObject obj = gameObjects[entry.Key];
                    if (obj != null)
                    {
                        WorldObject worldObject = obj.GetComponent<WorldObject>();
                        if (worldObject != null)
                        {
                            if (worldObject.GetDistance() <= VISIBILITY_RADIUS) // Object is in sight radius.
                            {
                                worldObject.AnimateObject(entry.Value.velocityX, entry.Value.velocityZ, entry.Value.triggerJump, entry.Value.isInWater, entry.Value.isGrounded);
                            }
                        }
                    }
                }

                ((IDictionary<long, AnimationHolder>)animationQueue).Remove(entry.Key);
            }

            // Update pending characters.
            foreach (KeyValuePair<long, CharacterDataHolder> entry in characterUpdateQueue)
            {
                if (gameObjects.ContainsKey(entry.Key))
                {
                    GameObject obj = gameObjects[entry.Key];
                    if (obj != null)
                    {
                        WorldObject worldObject = obj.GetComponent<WorldObject>();
                        if (worldObject != null)
                        {
                            if (worldObject.GetDistance() <= VISIBILITY_RADIUS) // Object is in sight radius.
                            {
                                DynamicCharacterAvatar avatar = obj.GetComponent<DynamicCharacterAvatar>();
                                if (avatar != null)
                                {
                                    // TODO: Manage more things than just item updates.
                                    CharacterDataHolder oldData = worldObject.characterData;
                                    CharacterDataHolder newData = entry.Value;

                                    int headItem = newData.GetHeadItem();
                                    if (headItem != oldData.GetHeadItem())
                                    {
                                        if (headItem == 0)
                                        {
                                            CharacterManager.Instance.UnEquipItem(avatar, ItemSlot.HEAD);
                                        }
                                        else
                                        {
                                            CharacterManager.Instance.EquipItem(avatar, headItem);
                                        }
                                    }

                                    int chestItem = newData.GetChestItem();
                                    if (chestItem != oldData.GetChestItem())
                                    {
                                        if (chestItem == 0)
                                        {
                                            CharacterManager.Instance.UnEquipItem(avatar, ItemSlot.CHEST);
                                        }
                                        else
                                        {
                                            CharacterManager.Instance.EquipItem(avatar, chestItem);
                                        }
                                    }

                                    int legsItem = newData.GetLegsItem();
                                    if (legsItem != oldData.GetLegsItem())
                                    {
                                        if (legsItem == 0)
                                        {
                                            CharacterManager.Instance.UnEquipItem(avatar, ItemSlot.LEGS);
                                        }
                                        else
                                        {
                                            CharacterManager.Instance.EquipItem(avatar, legsItem);
                                        }
                                    }

                                    int handsItem = newData.GetHandsItem();
                                    if (handsItem != oldData.GetHandsItem())
                                    {
                                        if (handsItem == 0)
                                        {
                                            CharacterManager.Instance.UnEquipItem(avatar, ItemSlot.HANDS);
                                        }
                                        else
                                        {
                                            CharacterManager.Instance.EquipItem(avatar, handsItem);
                                        }
                                    }

                                    int feetItem = newData.GetFeetItem();
                                    if (feetItem != oldData.GetFeetItem())
                                    {
                                        if (feetItem == 0)
                                        {
                                            CharacterManager.Instance.UnEquipItem(avatar, ItemSlot.FEET);
                                        }
                                        else
                                        {
                                            CharacterManager.Instance.EquipItem(avatar, feetItem);
                                        }
                                    }

                                    int leftHandItem = newData.GetLeftHandItem();
                                    if (leftHandItem != oldData.GetLeftHandItem())
                                    {
                                        if (leftHandItem == 0)
                                        {
                                            CharacterManager.Instance.UnEquipItem(avatar, ItemSlot.LEFT_HAND);
                                        }
                                        else
                                        {
                                            CharacterManager.Instance.EquipItem(avatar, leftHandItem);
                                        }
                                    }

                                    int rightHandItem = newData.GetRightHandItem();
                                    if (rightHandItem != oldData.GetRightHandItem())
                                    {
                                        if (rightHandItem == 0)
                                        {
                                            CharacterManager.Instance.UnEquipItem(avatar, ItemSlot.RIGHT_HAND);
                                        }
                                        else
                                        {
                                            CharacterManager.Instance.EquipItem(avatar, rightHandItem);
                                        }
                                    }

                                    // Update world object with new data.
                                    worldObject.characterData = newData;
                                }
                            }
                        }
                    }
                }

                ((IDictionary<long, CharacterDataHolder>)characterUpdateQueue).Remove(entry.Key);
            }
        }
    }

    // Calculate distance between player and a Vector3 location.
    public double CalculateDistance(Vector3 vector)
    {
        return Math.Pow(MovementController.storedPosition.x - vector.x, 2) + Math.Pow(MovementController.storedPosition.y - vector.y, 2) + Math.Pow(MovementController.storedPosition.z - vector.z, 2);
    }

    public void UpdateObject(long objectId, CharacterDataHolder characterdata)
    {
        lock (updateObjectLock) // Use lock to avoid adding duplicate gameObjects.
        {
            // Check for existing objects.
            if (gameObjects.ContainsKey(objectId))
            {
                // Update object info.
                ((IDictionary<long, CharacterDataHolder>)characterUpdateQueue).Remove(objectId);
                characterUpdateQueue.TryAdd(objectId, characterdata);
                return;
            }

            // Object is out of sight.
            if (CalculateDistance(new Vector3(characterdata.GetX(), characterdata.GetY(), characterdata.GetZ())) > VISIBILITY_RADIUS)
            {
                return;
            }

            // Add placeholder to game object list.
            gameObjects.GetOrAdd(objectId, (GameObject)null);

            // Queue creation.
            CharacterManager.Instance.characterCreationQueue.TryAdd(objectId, characterdata);
        }
    }

    private IEnumerator DelayedDestroy(GameObject obj)
    {
        yield return new WaitForSeconds(0.5f);

        // Destroy name text.
        Destroy(obj.GetComponent<WorldObjectText>().nameMesh.gameObject);
        // Finally destroy gameobject.
        Destroy(obj);
    }

    public void DeleteObject(long objectId)
    {
        lock (updateMethodLock)
        {
            if (!deleteQueue.Contains(objectId))
            {
                deleteQueue.Add(objectId);
            }
        }
    }

    public void ExitWorld()
    {
        exitingWorld = true;
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
        CharacterManager.Instance.characterCreationQueue.Clear();
    }

    public void SetTarget(WorldObject worldObject)
    {
        // Unset old target.
        if (targetWorldObject != null)
        {
            WorldObjectText previousObjectText = targetWorldObject.GetComponentInParent<WorldObjectText>();
            if (previousObjectText != null)
            {
                previousObjectText.currentColor = WorldObjectText.DEFAULT_COLOR;
            }
        }

        // Set new target.
        targetWorldObject = worldObject;
        if (targetWorldObject != null)
        {
            WorldObjectText worldObjectText = targetWorldObject.GetComponentInParent<WorldObjectText>();
            if (worldObjectText != null)
            {
                worldObjectText.currentColor = WorldObjectText.SELECTED_COLOR;
            }

            // Send new target id to server.
            NetworkManager.ChannelSend(new TargetUpdateRequest(targetWorldObject.objectId));
        }
        else // Target has been unset.
        {
            NetworkManager.ChannelSend(new TargetUpdateRequest(-1));
        }

        // Update UI target information.
        StatusInformationManager.Instance.UpdateTargetInformation(targetWorldObject);
    }
}
