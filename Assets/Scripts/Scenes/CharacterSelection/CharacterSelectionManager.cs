using System.Collections;
using TMPro;
using UMA.CharacterSystem;
using UnityEngine;
using UnityEngine.UI;

/**
 * Author: Pantelis Andrianakis
 * Date: December 26th 2017
 */
public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance { get; private set; }

    public TextMeshProUGUI textMessage;
    public Button nextCharButton;
    public Button previousCharButton;
    public Button createCharButton;
    public Button deleteCharButton;
    public Button exitToLoginButton;
    public Button enterWorldButton;
    public TextMeshProUGUI characterName;

    [HideInInspector]
    public bool waitingServer;
    private bool characterSelected = false;
    private int characterSelectedSlot = 0;
    private DynamicCharacterAvatar avatar;

    private void Start()
    {
        // Set instance.
        if (Instance != null)
        {
            return;
        }
        Instance = this;

        // In case player logouts underwater.
        RenderSettings.fogColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        RenderSettings.fogDensity = 0.01f;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogStartDistance = 500;
        RenderSettings.fogEndDistance = 1200;

        // Restore Camera Position
        Camera.main.transform.position = new Vector3(8.29f, 1.29f, 17.7f);

        // Schedule exit to login screen.
        StartCoroutine(ExitToLoginScreen());

        // Show retrieving information message.
        textMessage.text = "Retrieving character information.";

        // Request info.
        waitingServer = true;
        NetworkManager.ChannelSend(new CharacterSelectionInfoRequest());
        // Wait until server sends existing player data.
        while (waitingServer)
        {
            // Make sure information from the server is received.
        }

        // Show last selected character.
        if (MainManager.Instance.characterList.Count > 0)
        {
            for (int i = 0; i < MainManager.Instance.characterList.Count; i++)
            {
                // Get current character data.
                MainManager.Instance.selectedCharacterData = (CharacterDataHolder)MainManager.Instance.characterList[i];
                if (MainManager.Instance.selectedCharacterData.IsSelected() || i == MainManager.Instance.characterList.Count - 1)
                {
                    avatar = CharacterManager.Instance.CreateCharacter(MainManager.Instance.selectedCharacterData, 8.28f, 0.1035156f, 20.222f, 180);
                    characterName.text = MainManager.Instance.selectedCharacterData.GetName();
                    characterSelectedSlot = i;
                    characterSelected = true;
                    break;
                }
            }
        }
        else // In case of character deletion.
        {
            MainManager.Instance.selectedCharacterData = null;
        }

        // Click listeners.
        nextCharButton.onClick.AddListener(OnClickNextButton);
        previousCharButton.onClick.AddListener(OnClickPreviousButton);
        createCharButton.onClick.AddListener(OnClickCreateButton);
        deleteCharButton.onClick.AddListener(OnClickDeleteButton);
        exitToLoginButton.onClick.AddListener(OnClickExitButton);
        enterWorldButton.onClick.AddListener(OnEnterWorldButton);

        // Hide retrieving information message.
        if (!characterSelected)
        {
            textMessage.text = "Click the create button to make a new character.";
            deleteCharButton.gameObject.SetActive(false);
            Destroy(avatar);
        }
        else
        {
            enterWorldButton.Select(); // Be ready to enter via keyboard.
            textMessage.text = "";
        }

        // Hide previous and next buttons if caharcter count is less than 2.
        if (MainManager.Instance.characterList.Count < 2)
        {
            previousCharButton.gameObject.SetActive(false);
            nextCharButton.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (InputManager.RETURN_DOWN)
        {
            OnEnterWorldButton();
        }
    }

    private IEnumerator ExitToLoginScreen()
    {
        yield return new WaitForSeconds(900); // Wait 15 minutes.
        OnClickExitButton();
    }

    private void OnClickExitButton()
    {
        if (avatar != null)
        {
            Destroy(avatar.gameObject);
        }
        MainManager.Instance.LoadScene(MainManager.LOGIN_SCENE);
    }

    private void OnClickCreateButton()
    {
        if (avatar != null)
        {
            Destroy(avatar.gameObject);
        }
        MainManager.Instance.LoadScene(MainManager.CHARACTER_CREATION_SCENE);
    }

    private void OnClickDeleteButton()
    {
        if (characterSelected)
        {
            ConfirmDialog.Instance.PlayerConfirm("Delete character " + MainManager.Instance.selectedCharacterData.GetName() + "?", 2);
        }
    }

    public void DeleteCharacter()
    {
        // Get current character data.
        CharacterDataHolder characterData = MainManager.Instance.selectedCharacterData;

        // Return if no character is selected.
        if (characterData == null)
        {
            return;
        }

        // Set text message to deleting character.
        textMessage.text = "Deleting character " + characterData.GetName() + "...";

        // Request info.
        waitingServer = true;
        NetworkManager.ChannelSend(new CharacterDeletionRequest(characterData.GetSlot()));

        // Wait until server deletes the character.
        while (waitingServer)
        {
            // Make sure server has deleted the character.
        }

        if (characterSelected)
        {
            Destroy(avatar.gameObject);
        }

        // Reload everything.
        MainManager.Instance.LoadScene(MainManager.CHARACTER_SELECTION_SCENE);
    }

    private void OnClickNextButton()
    {
        if (MainManager.Instance.selectedCharacterData == null || MainManager.Instance.characterList.Count <= 1)
        {
            return;
        }
        if (characterSelectedSlot >= MainManager.Instance.characterList.Count - 1)
        {
            characterSelectedSlot = -1;
        }
        characterSelectedSlot++;
        MainManager.Instance.selectedCharacterData = (CharacterDataHolder)MainManager.Instance.characterList[characterSelectedSlot];
        characterName.text = MainManager.Instance.selectedCharacterData.GetName();
        NetworkManager.ChannelSend(new CharacterSelectUpdate(characterSelectedSlot));
        Destroy(avatar.gameObject);
        avatar = CharacterManager.Instance.CreateCharacter(MainManager.Instance.selectedCharacterData, 8.28f, 0.1035156f, 20.222f, 180);
    }

    private void OnClickPreviousButton()
    {
        if (MainManager.Instance.selectedCharacterData == null || MainManager.Instance.characterList.Count <= 1)
        {
            return;
        }
        if (characterSelectedSlot <= 0)
        {
            characterSelectedSlot = MainManager.Instance.characterList.Count;
        }
        characterSelectedSlot--;
        MainManager.Instance.selectedCharacterData = (CharacterDataHolder)MainManager.Instance.characterList[characterSelectedSlot];
        characterName.text = MainManager.Instance.selectedCharacterData.GetName();
        NetworkManager.ChannelSend(new CharacterSelectUpdate(characterSelectedSlot));
        Destroy(avatar.gameObject);
        avatar = CharacterManager.Instance.CreateCharacter(MainManager.Instance.selectedCharacterData, 8.28f, 0.1035156f, 20.222f, 180);
    }

    private void OnEnterWorldButton()
    {
        if (characterSelected)
        {
            characterSelected = false;
            Destroy(avatar.gameObject);
            MainManager.Instance.LoadScene(MainManager.WORLD_SCENE);
        }
    }
}
