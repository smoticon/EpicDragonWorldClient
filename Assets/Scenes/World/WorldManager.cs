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
using UnityEngine;

/**
 * @author Pantelis Andrianakis
 */
public class WorldManager : MonoBehaviour
{
    public GameObject[] characterModels;
    public GameObject playerCharacter;

    private void Start()
    {
        // Return if account name is empty.
        if (NetworkManager.instance == null || NetworkManager.instance.accountName == null)
        {
            return; // Return to login?
        }

        // Change music.
        MusicManager.instance.PlayMusic(MusicManager.instance.EnterWorld);

        // Set player model.
        playerCharacter.GetComponent<MeshFilter>().mesh = characterModels[NetworkManager.instance.selectedCharacterData.GetClassId()].GetComponent<MeshFilter>().mesh;
        playerCharacter.GetComponent<Renderer>().materials = characterModels[NetworkManager.instance.selectedCharacterData.GetClassId()].GetComponent<Renderer>().materials;

        // TODO: Send player info to server.
    }

    private void Update()
    {

    }
}
