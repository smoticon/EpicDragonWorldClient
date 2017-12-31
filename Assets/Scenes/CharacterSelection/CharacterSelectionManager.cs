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
    public bool waitingServer;
    [HideInInspector]
    GameObject characterSelected;

    public Transform spawnLocation;
    public GameObject[] characterModels;
    public Button[] selectButtons;
    public Button[] moveUpButtons;
    public Button[] moveDownButtons;
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

        // Disable character select buttons.
        selectButtons[0].gameObject.SetActive(false);
        selectButtons[1].gameObject.SetActive(false);
        selectButtons[2].gameObject.SetActive(false);
        selectButtons[3].gameObject.SetActive(false);
        selectButtons[4].gameObject.SetActive(false);

        moveUpButtons[0].gameObject.SetActive(false);
        moveUpButtons[1].gameObject.SetActive(false);
        moveUpButtons[2].gameObject.SetActive(false);
        moveUpButtons[3].gameObject.SetActive(false);
        moveUpButtons[4].gameObject.SetActive(false);

        moveDownButtons[0].gameObject.SetActive(false);
        moveDownButtons[1].gameObject.SetActive(false);
        moveDownButtons[2].gameObject.SetActive(false);
        moveDownButtons[3].gameObject.SetActive(false);
        moveDownButtons[4].gameObject.SetActive(false);

        // Request info.
        waitingServer = true;
        NetworkManager.instance.ChannelSend(new CharacterSelectionInfoRequest());
        // Wait until server sends existing player data.
        while (waitingServer)
        {
            // Make sure information from the server is received.
        }

        // Show last selected character.
        if (NetworkManager.instance.characterList.Count > 0)
        {
            for (int i = 0; i < NetworkManager.instance.characterList.Count; i++)
            {
                // Get current character data.
                CharacterDataHolder characterData = (CharacterDataHolder)NetworkManager.instance.characterList[i];

                // Set select button text to character name.
                selectButtons[i].gameObject.SetActive(true);
                selectButtons[i].GetComponentInChildren<Text>().text = characterData.GetName();
                // Enable move up / down buttons.
                moveUpButtons[i].gameObject.SetActive(true);
                moveDownButtons[i].gameObject.SetActive(true);

                if (characterData.IsSelected())
                {
                    NetworkManager.instance.selectedCharacterData = characterData;
                    // Model 0-3 id is set from character class id.
                    // characterModels[characterData.GetClassId()]
                    characterSelected = Instantiate(characterModels[characterData.GetClassId()], spawnLocation.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
                    // TODO: Restore appearance when support is made.
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

        selectButtons[0].GetComponent<Button>().onClick.AddListener(OnClickSelectCharacter1Button);
        selectButtons[1].GetComponent<Button>().onClick.AddListener(OnClickSelectCharacter2Button);
        selectButtons[2].GetComponent<Button>().onClick.AddListener(OnClickSelectCharacter3Button);
        selectButtons[3].GetComponent<Button>().onClick.AddListener(OnClickSelectCharacter4Button);
        selectButtons[4].GetComponent<Button>().onClick.AddListener(OnClickSelectCharacter5Button);

        moveUpButtons[1].GetComponent<Button>().onClick.AddListener(OnClickMoveUp2Button);
        moveUpButtons[2].GetComponent<Button>().onClick.AddListener(OnClickMoveUp3Button);
        moveUpButtons[3].GetComponent<Button>().onClick.AddListener(OnClickMoveUp4Button);
        moveUpButtons[4].GetComponent<Button>().onClick.AddListener(OnClickMoveUp5Button);

        moveDownButtons[0].GetComponent<Button>().onClick.AddListener(OnClickMoveDown1Button);
        moveDownButtons[1].GetComponent<Button>().onClick.AddListener(OnClickMoveDown2Button);
        moveDownButtons[2].GetComponent<Button>().onClick.AddListener(OnClickMoveDown3Button);
        moveDownButtons[3].GetComponent<Button>().onClick.AddListener(OnClickMoveDown4Button);

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

    private void OnClickSelectCharacter1Button()
    {
        // Send selected character update packet to server.
        NetworkManager.instance.ChannelSend(new CharacterSelectUpdate(1));
        // Change selected character.
        Destroy(characterSelected);
        NetworkManager.instance.selectedCharacterData = (CharacterDataHolder)NetworkManager.instance.characterList[0];
        characterSelected = Instantiate(characterModels[NetworkManager.instance.selectedCharacterData.GetClassId()], spawnLocation.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
    }

    private void OnClickSelectCharacter2Button()
    {
        // Send selected character update packet to server.
        NetworkManager.instance.ChannelSend(new CharacterSelectUpdate(2));
        // Change selected character.
        Destroy(characterSelected);
        NetworkManager.instance.selectedCharacterData = (CharacterDataHolder)NetworkManager.instance.characterList[1];
        characterSelected = Instantiate(characterModels[NetworkManager.instance.selectedCharacterData.GetClassId()], spawnLocation.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
    }

    private void OnClickSelectCharacter3Button()
    {
        // Send selected character update packet to server.
        NetworkManager.instance.ChannelSend(new CharacterSelectUpdate(3));
        // Change selected character.
        Destroy(characterSelected);
        NetworkManager.instance.selectedCharacterData = (CharacterDataHolder)NetworkManager.instance.characterList[2];
        characterSelected = Instantiate(characterModels[NetworkManager.instance.selectedCharacterData.GetClassId()], spawnLocation.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
    }

    private void OnClickSelectCharacter4Button()
    {
        // Send selected character update packet to server.
        NetworkManager.instance.ChannelSend(new CharacterSelectUpdate(4));
        // Change selected character.
        Destroy(characterSelected);
        NetworkManager.instance.selectedCharacterData = (CharacterDataHolder)NetworkManager.instance.characterList[3];
        characterSelected = Instantiate(characterModels[NetworkManager.instance.selectedCharacterData.GetClassId()], spawnLocation.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
    }

    private void OnClickSelectCharacter5Button()
    {
        // Send selected character update packet to server.
        NetworkManager.instance.ChannelSend(new CharacterSelectUpdate(5));
        // Change selected character.
        Destroy(characterSelected);
        NetworkManager.instance.selectedCharacterData = (CharacterDataHolder)NetworkManager.instance.characterList[4];
        characterSelected = Instantiate(characterModels[NetworkManager.instance.selectedCharacterData.GetClassId()], spawnLocation.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
    }

    private void OnClickMoveUp2Button()
    {
        // Send character slot update packet to server.
        NetworkManager.instance.ChannelSend(new CharacterSlotUpdate(2, 1));
        // Update stored data.
        CharacterDataHolder temp = (CharacterDataHolder)NetworkManager.instance.characterList[0];
        NetworkManager.instance.characterList[0] = NetworkManager.instance.characterList[1];
        NetworkManager.instance.characterList[1] = temp;
        // Update button name.
        string name = selectButtons[0].GetComponentInChildren<Text>().text;
        selectButtons[0].GetComponentInChildren<Text>().text = selectButtons[1].GetComponentInChildren<Text>().text;
        selectButtons[1].GetComponentInChildren<Text>().text = name;
        // Select character.
        OnClickSelectCharacter1Button();
    }

    private void OnClickMoveUp3Button()
    {
        // Send character slot update packet to server.
        NetworkManager.instance.ChannelSend(new CharacterSlotUpdate(3, 2));
        // Update stored data.
        CharacterDataHolder temp = (CharacterDataHolder)NetworkManager.instance.characterList[1];
        NetworkManager.instance.characterList[1] = NetworkManager.instance.characterList[2];
        NetworkManager.instance.characterList[2] = temp;
        // Update button name.
        string name = selectButtons[1].GetComponentInChildren<Text>().text;
        selectButtons[1].GetComponentInChildren<Text>().text = selectButtons[2].GetComponentInChildren<Text>().text;
        selectButtons[2].GetComponentInChildren<Text>().text = name;
        // Select character.
        OnClickSelectCharacter2Button();
    }

    private void OnClickMoveUp4Button()
    {
        // Send character slot update packet to server.
        NetworkManager.instance.ChannelSend(new CharacterSlotUpdate(4, 3));
        // Update stored data.
        CharacterDataHolder temp = (CharacterDataHolder)NetworkManager.instance.characterList[2];
        NetworkManager.instance.characterList[2] = NetworkManager.instance.characterList[3];
        NetworkManager.instance.characterList[3] = temp;
        // Update button name.
        string name = selectButtons[2].GetComponentInChildren<Text>().text;
        selectButtons[2].GetComponentInChildren<Text>().text = selectButtons[3].GetComponentInChildren<Text>().text;
        selectButtons[3].GetComponentInChildren<Text>().text = name;
        // Select character.
        OnClickSelectCharacter3Button();
    }

    private void OnClickMoveUp5Button()
    {
        // Send character slot update packet to server.
        NetworkManager.instance.ChannelSend(new CharacterSlotUpdate(5, 4));
        // Update stored data.
        CharacterDataHolder temp = (CharacterDataHolder)NetworkManager.instance.characterList[3];
        NetworkManager.instance.characterList[3] = NetworkManager.instance.characterList[4];
        NetworkManager.instance.characterList[4] = temp;
        // Update button name.
        string name = selectButtons[3].GetComponentInChildren<Text>().text;
        selectButtons[3].GetComponentInChildren<Text>().text = selectButtons[4].GetComponentInChildren<Text>().text;
        selectButtons[4].GetComponentInChildren<Text>().text = name;
        // Select character.
        OnClickSelectCharacter4Button();
    }

    private void OnClickMoveDown1Button()
    {
        // Check if character bellow exists.
        if (NetworkManager.instance.characterList.Count == 1)
        {
            return;
        }
        // Send character slot update packet to server.
        NetworkManager.instance.ChannelSend(new CharacterSlotUpdate(1, 2));
        // Update stored data.
        CharacterDataHolder temp = (CharacterDataHolder)NetworkManager.instance.characterList[1];
        NetworkManager.instance.characterList[1] = NetworkManager.instance.characterList[0];
        NetworkManager.instance.characterList[0] = temp;
        // Update button name.
        string name = selectButtons[1].GetComponentInChildren<Text>().text;
        selectButtons[1].GetComponentInChildren<Text>().text = selectButtons[0].GetComponentInChildren<Text>().text;
        selectButtons[0].GetComponentInChildren<Text>().text = name;
        // Select character.
        OnClickSelectCharacter2Button();
    }

    private void OnClickMoveDown2Button()
    {
        // Check if character bellow exists.
        if (NetworkManager.instance.characterList.Count == 2)
        {
            return;
        }
        // Send character slot update packet to server.
        NetworkManager.instance.ChannelSend(new CharacterSlotUpdate(2, 3));
        // Update stored data.
        CharacterDataHolder temp = (CharacterDataHolder)NetworkManager.instance.characterList[2];
        NetworkManager.instance.characterList[2] = NetworkManager.instance.characterList[1];
        NetworkManager.instance.characterList[1] = temp;
        // Update button name.
        string name = selectButtons[2].GetComponentInChildren<Text>().text;
        selectButtons[2].GetComponentInChildren<Text>().text = selectButtons[1].GetComponentInChildren<Text>().text;
        selectButtons[1].GetComponentInChildren<Text>().text = name;
        // Select character.
        OnClickSelectCharacter3Button();
    }

    private void OnClickMoveDown3Button()
    {
        // Check if character bellow exists.
        if (NetworkManager.instance.characterList.Count == 3)
        {
            return;
        }
        // Send character slot update packet to server.
        NetworkManager.instance.ChannelSend(new CharacterSlotUpdate(3, 4));
        // Update stored data.
        CharacterDataHolder temp = (CharacterDataHolder)NetworkManager.instance.characterList[3];
        NetworkManager.instance.characterList[3] = NetworkManager.instance.characterList[2];
        NetworkManager.instance.characterList[2] = temp;
        // Update button name.
        string name = selectButtons[3].GetComponentInChildren<Text>().text;
        selectButtons[3].GetComponentInChildren<Text>().text = selectButtons[2].GetComponentInChildren<Text>().text;
        selectButtons[2].GetComponentInChildren<Text>().text = name;
        // Select character.
        OnClickSelectCharacter4Button();
    }

    private void OnClickMoveDown4Button()
    {
        // Check if character bellow exists.
        if (NetworkManager.instance.characterList.Count == 4)
        {
            return;
        }
        // Send character slot update packet to server.
        NetworkManager.instance.ChannelSend(new CharacterSlotUpdate(4, 5));
        // Update stored data.
        CharacterDataHolder temp = (CharacterDataHolder)NetworkManager.instance.characterList[4];
        NetworkManager.instance.characterList[4] = NetworkManager.instance.characterList[3];
        NetworkManager.instance.characterList[3] = temp;
        // Update button name.
        string name = selectButtons[4].GetComponentInChildren<Text>().text;
        selectButtons[4].GetComponentInChildren<Text>().text = selectButtons[3].GetComponentInChildren<Text>().text;
        selectButtons[3].GetComponentInChildren<Text>().text = name;
        // Select character.
        OnClickSelectCharacter5Button();
    }

    private void OnClickCreateButton()
    {
        SceneFader.Fade("CharacterCreation", Color.white, 0.5f);
    }

    private void OnClickDeleteButton()
    {
        // Get current character data.
        CharacterDataHolder characterData = NetworkManager.instance.selectedCharacterData;

        // Return if no character is selected.
        if (characterData == null)
        {
            return;
        }

        // Disable buttons.
        createButton.enabled = false;
        deleteButton.enabled = false;
        enterWorldButton.enabled = false;
        exitButton.enabled = false;

        // Set text message to deleting character.
        textMessage.text = "Deleting character " + characterData.GetName() + "...";

        // Request info.
        waitingServer = true;
        NetworkManager.instance.ChannelSend(new CharacterDeletionRequest(characterData.GetSlot()));

        // Wait until server deletes the character.
        while (waitingServer)
        {
            // Make sure server has deleted the character.
        }

        // Reload everything.
        SceneFader.Fade("CharacterSelection", Color.white, 0.5f);
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
            // TODO: Enter world.
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
