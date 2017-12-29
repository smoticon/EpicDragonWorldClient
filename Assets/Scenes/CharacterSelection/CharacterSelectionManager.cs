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
using UnityEngine.UI;

/**
 * @author Pantelis Andrianakis
 */
public class CharacterSelectionManager : MonoBehaviour
{
    [HideInInspector]
    public static CharacterSelectionManager instance;
    [HideInInspector]
    public bool waitingServer = true;

    public Transform spawnLocation;
    public GameObject[] characterModels;
    public Text textMessage;
    public Button createButton;
    public Button deleteButton;
    public Button enterWorldButton;
    public Button exitButton;

    private void Start()
    {
        // Return if account name is empty.
        if (NetworkManager.instance == null || NetworkManager.instance.accountName == null)
        {
            return;
        }

        // Set instance.
        instance = this;

        // Schedule exit to login screen.
        StartCoroutine(ExitToLoginScreen());

        // Show retrieving information message.
        textMessage.text = "Retrieving character information.";

        // Change music.
        MusicManager.instance.PlayMusic(MusicManager.instance.CharacterSelection);

        // Request info.
        NetworkManager.instance.ChannelSend(new CharacterSelectionInfoRequest());

        // Wait until server sends existing player data.
        while (waitingServer)
        {
            // Make sure information from the server is received.
        }

        // Show last selected character.
        GameObject characterSelected = null;
        if (NetworkManager.instance.characterList.Count > 0)
        {
            foreach (CharacterDataHolder characterData in NetworkManager.instance.characterList)
            {
                if (characterData.IsSelected())
                {
                    NetworkManager.instance.selectedCharacterData = characterData;
                    // Model id is 0-3 is set from character class id.
                    // characterModels[characterData.GetClassId()]
                    characterSelected = Instantiate(characterModels[characterData.GetClassId()], spawnLocation.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
                    // TODO: Restore appearance when support is made.
                    break;
                }
            }
        }
        else // In case of character deletion.
        {
            NetworkManager.instance.selectedCharacterData = null;
        }

        // Add button listeners.
        createButton.GetComponent<Button>().onClick.AddListener(OnClickCreateButton);
        deleteButton.GetComponent<Button>().onClick.AddListener(OnClickDeleteButton);
        enterWorldButton.GetComponent<Button>().onClick.AddListener(OnClickEnterButton);
        exitButton.GetComponent<Button>().onClick.AddListener(OnClickExitButton);

        // Hide retrieving information message.
        if (characterSelected == null)
        {
            textMessage.text = "Click the create button to make a new character.";
        }
        else
        {
            textMessage.text = "";
        }
    }

    private void OnClickCreateButton()
    {
        SceneFader.Fade("CharacterCreation", Color.white, 0.5f);
    }

    private void OnClickDeleteButton()
    {
        // TODO:
    }

    private void OnClickEnterButton()
    {
        // Check if no character exists.
        if (NetworkManager.instance.selectedCharacterData == null)
        {
            textMessage.text = "You must create a character.";
        }
        else
        {
            // TODO:
        }
    }

    private void OnClickExitButton()
    {
        SceneFader.Fade("LoginScreen", Color.white, 0.5f);
    }

    private IEnumerator ExitToLoginScreen()
    {
        yield return new WaitForSeconds(900); // Wait 15 minutes.
        OnClickExitButton();
    }
}
