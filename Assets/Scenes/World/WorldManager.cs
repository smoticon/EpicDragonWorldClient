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
using System.Collections;
using UnityEngine;

/**
 * @author Pantelis Andrianakis
 */
public class WorldManager : MonoBehaviour
{
    public GameObject playerCharacter;

    [HideInInspector]
    public static WorldManager instance;
    [HideInInspector]
    ArrayList gameObjects = new ArrayList();
    [HideInInspector]
    private static readonly int visibilityRange = 3000;

    private void Start()
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
        playerCharacter.GetComponent<MeshFilter>().mesh = GameObjectManager.instance.playerModels[PlayerManager.instance.selectedCharacterData.GetClassId()].GetComponent<MeshFilter>().mesh;
        playerCharacter.GetComponent<Renderer>().materials = GameObjectManager.instance.playerModels[PlayerManager.instance.selectedCharacterData.GetClassId()].GetComponent<Renderer>().materials;

        // Request world info from server.
        NetworkManager.instance.ChannelSend(new EnterWorldRequest(PlayerManager.instance.selectedCharacterData.GetName()));

        // Object distance forget task.
        StartCoroutine(DistanceCheck());
    }

    public void AddObject(int objectId, float posX, float posY, float posZ, int posHeading)
    {
        // Instantiate.
        GameObject obj = Instantiate(GameObjectManager.instance.gameObjectList[0], new Vector3(posX, posY, posZ), Quaternion.identity) as GameObject;

        // Assign object id.
        obj.AddComponent<WorldObject>();
        obj.GetComponent<WorldObject>().objectId = objectId;

        // TODO: Proper appearance.

        // Add RigidBody.
        Rigidbody rigidBody = obj.AddComponent<Rigidbody>();
        rigidBody.mass = 1;
        rigidBody.angularDrag = 0.05f;
        rigidBody.freezeRotation = true;

        // Add to game object list.
        gameObjects.Add(obj);
    }

    public void MoveObject(int objectId, float posX, float posY, float posZ)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.GetComponent<WorldObject>().objectId == objectId)
            {
                gameObject.GetComponent<Rigidbody>().MovePosition(Vector3.MoveTowards(gameObject.transform.position, new Vector3(posX, posY, posZ), 1));
                break;
            }
        }
    }

    private void DeleteObject(GameObject gameObject)
    {
        // Remove from objects list.
        gameObjects.Remove(gameObject);

        // Delete game object from world.
        Destroy(gameObject);
    }

    IEnumerator DistanceCheck()
    {
        while (true)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                if (Vector3.Distance(playerCharacter.transform.position, gameObject.transform.position) > visibilityRange)
                {
                    DeleteObject(gameObject);
                }
            }
            yield return new WaitForSeconds(3);
        }
    }
}
