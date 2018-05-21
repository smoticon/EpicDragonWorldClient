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
    ArrayList characterModels = new ArrayList();

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
    }

    public void AddObject(double posX, double posY, double posZ, int posHeading)
    {
        // GameObject temp = Instantiate(GameObjectManager.instance.gameObjectList[0], new Vector3(9.824759f, -10.283f, 0.2593288f), Quaternion.identity) as GameObject;
        characterModels.Add(gameObject);
    }

    private void Update()
    {

    }
}
