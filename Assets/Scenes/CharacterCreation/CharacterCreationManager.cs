using UnityEngine;
using UnityEngine.UI;

/**
 * @author Pantelis Andrianakis
 */
public class CharacterCreationManager : MonoBehaviour
{
    [HideInInspector]
    public static CharacterCreationManager instance;
    [HideInInspector]
    public int creationResult;
    private bool waitingServer;
    private GameObject characterSelected;

    //TODO: Add more customization data.
    private int classSelected = 0;

    public Button[] selectButtons;
    public Transform spawnLocation;
    public Text textMessage;
    public InputField characterName;
    public Button createButton;
    public Button backButton;

    private void Start()
    {
        // Return if account name is empty.
        if (PlayerManager.instance == null || PlayerManager.instance.accountName == null)
        {
            return;
        }

        // Save instance.
        instance = this;

        // Button listeners.
        selectButtons[0].GetComponent<Button>().onClick.AddListener(OnClickButton1);
        selectButtons[1].GetComponent<Button>().onClick.AddListener(OnClickButton2);
        selectButtons[2].GetComponent<Button>().onClick.AddListener(OnClickButton3);
        selectButtons[3].GetComponent<Button>().onClick.AddListener(OnClickButton4);
        createButton.GetComponent<Button>().onClick.AddListener(OnClickCreateButton);
        backButton.GetComponent<Button>().onClick.AddListener(OnClickBackButton);

        // Select first character model.
        characterSelected = Instantiate(GameObjectManager.instance.playerModels[0], spawnLocation.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnClickCreateButton();
        }
    }

    private void OnClickButton1()
    {
        Destroy(characterSelected);
        characterSelected = Instantiate(GameObjectManager.instance.playerModels[0], spawnLocation.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;
        classSelected = 0;
    }

    private void OnClickButton2()
    {
        Destroy(characterSelected);
        characterSelected = Instantiate(GameObjectManager.instance.playerModels[1], spawnLocation.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;
        classSelected = 1;
    }

    private void OnClickButton3()
    {
        Destroy(characterSelected);
        characterSelected = Instantiate(GameObjectManager.instance.playerModels[2], spawnLocation.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;
        classSelected = 2;
    }

    private void OnClickButton4()
    {
        Destroy(characterSelected);
        characterSelected = Instantiate(GameObjectManager.instance.playerModels[3], spawnLocation.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;
        classSelected = 3;
    }

    private void OnClickCreateButton()
    {
        // Disable buttons.
        DisableButtons();

        // Store creation information.
        string name = characterName.text;

        // No name entered.
        if (name == "")
        {
            textMessage.text = "Please enter a name.";
            EnableButtons();
            return;
        }

        // Request character creation.
        NetworkManager.instance.ChannelSend(new CharacterCreationRequest(name, classSelected));

        // Wait until server sends creation result.
        waitingServer = true;
        creationResult = -1;
        while (waitingServer)
        {
            switch (creationResult)
            {
                case 0:
                    textMessage.text = "Invalid name.";
                    waitingServer = false;
                    break;

                case 1:
                    textMessage.text = "Name is too short.";
                    waitingServer = false;
                    break;

                case 2:
                    textMessage.text = "Name already exists.";
                    waitingServer = false;
                    break;

                case 3:
                    textMessage.text = "Cannot create additional characters.";
                    waitingServer = false;
                    break;

                case 100:
                    textMessage.text = "Creation success!";
                    waitingServer = false;
                    break;
            }
        }

        // Go to player selection screen.
        if (creationResult == 100)
        {
            OnClickBackButton();
        }
        else // Enable buttons.
        {
            EnableButtons();
        }
    }

    private void OnClickBackButton()
    {
        Destroy(characterSelected); // Destroy clone object.
        SceneFader.Fade("CharacterSelection", Color.white, 0.5f);
    }

    private void DisableButtons()
    {
        textMessage.text = "Waiting for server..."; // Clean any old messages.
        selectButtons[0].enabled = false;
        selectButtons[1].enabled = false;
        selectButtons[2].enabled = false;
        selectButtons[3].enabled = false;
        createButton.enabled = false;
        backButton.enabled = false;
        characterName.enabled = false;
    }

    private void EnableButtons()
    {
        selectButtons[0].enabled = true;
        selectButtons[1].enabled = true;
        selectButtons[2].enabled = true;
        selectButtons[3].enabled = true;
        createButton.enabled = true;
        backButton.enabled = true;
        characterName.enabled = true;
    }
}
