using System.Collections.Generic;
using UnityEngine;
using UMA;
using UMA.CharacterSystem;
using UnityEngine.UI;
using System.Collections;
using TMPro;

/**
 * Author: Ilias Vlachos, Pantelis Andrianakis
 * Date: December 28th 2018
 * Source: https://www.youtube.com/watch?v=3uPrkH59Q0c
 */
public class CharacterCreationManager : MonoBehaviour
{
    public static CharacterCreationManager Instance { get; private set; }

    public Slider heightSlider;
    public Slider bellySlider;
    public Button zoomIn;
    public Button zoomOut;
    public TMP_InputField charNameField;
    public Button createButton;
    public Text textMessage;
    public Button backButton;

    [HideInInspector]
    public int creationResult;
    private bool waitingServer;
    private DynamicCharacterAvatar avatar;
    private Dictionary<string, DnaSetter> dna;
    private CharacterDataHolder dataHolder;
    private CharacterDataHolder dataHolderMale;
    private CharacterDataHolder dataHolderFemale;
    private int currentHairMale = 0;
    private int currentHairFemale = 0;

    private void Start()
    {
        // Return if account name is empty.
        if (MainManager.Instance == null || MainManager.Instance.accountName == null)
        {
            return;
        }

        // Set instance.
        if (Instance != null)
        {
            return;
        }
        Instance = this;

        // Schedule exit to login screen.
        StartCoroutine(ExitToCharacterSelection());

        // Add button listeners.
        zoomIn.onClick.AddListener(CameraZoomIn);
        zoomOut.onClick.AddListener(CameraZoomOut);
        createButton.onClick.AddListener(OnClickCreateButton);
        backButton.onClick.AddListener(OnClickBackButton);

        // Initialize character data holders.
        dataHolderMale = new CharacterDataHolder();
        dataHolderMale.SetRace(0);
        dataHolderFemale = new CharacterDataHolder();
        dataHolderFemale.SetRace(1);
        dataHolder = dataHolderMale;

        // Initial values.
        avatar = CharacterManager.Instance.CreateCharacter(dataHolderMale, 8.28f, 0.1035156f, 20.222f, 180);
        avatar.CharacterUpdated.AddListener(Updated);
        heightSlider.onValueChanged.AddListener(HeightChange);
        bellySlider.onValueChanged.AddListener(BellyChange);

        // Camera position.
        Camera.main.transform.position = new Vector3(8.29f, 1.29f, 17.7f);
    }

    private void Update()
    {
        if (InputManager.RETURN_DOWN)
        {
            OnClickCreateButton();
        }
    }

    public void SwitchGender(bool male)
    {
        if (male && avatar.activeRace.name != "HumanMaleDCS")
        {
            dataHolder = dataHolderMale;
            Destroy(avatar.gameObject);
            avatar = CharacterManager.Instance.CreateCharacter(dataHolder, 8.28f, 0.1035156f, 20.222f, 180);
        }
        if (!male && avatar.activeRace.name != "HumanFemaleDCS")
        {
            dataHolder = dataHolderFemale;
            Destroy(avatar.gameObject);
            avatar = CharacterManager.Instance.CreateCharacter(dataHolder, 8.28f, 0.1035156f, 20.222f, 180);
        }
    }

    void Updated(UMAData data)
    {
        dna = avatar.GetDNA();
        heightSlider.value = dna["height"].Get();
        bellySlider.value = dna["belly"].Get();
    }

    public void HeightChange(float val)
    {
        dna["height"].Set(val);
        avatar.BuildCharacter();
        dataHolder.SetHeight(val);
    }

    public void BellyChange(float val)
    {
        dna["belly"].Set(val);
        avatar.BuildCharacter();
        dataHolder.SetBelly(val);
    }

    public void ChangeSkinColor(Color color)
    {
        avatar.SetColor("Skin", color);
        avatar.UpdateColors(true);
        dataHolder.SetSkinColor(Util.ColorToInt(color));
    }

    public void ChangeHairColor(Color color)
    {
        avatar.SetColor("Hair", color);
        avatar.UpdateColors(true);
        dataHolder.SetHairColor(Util.ColorToInt(color));
    }

    public void ChangeEyesColor(Color color)
    {
        avatar.SetColor("Eyes", color);
        avatar.UpdateColors(true);
        dataHolder.SetEyeColor(Util.ColorToInt(color));
    }

    public void ChangeHair(bool plus)
    {
        if (avatar.activeRace.name == "HumanMaleDCS")
        {
            if (plus)
            {
                currentHairMale++;
            }
            else
            {
                currentHairMale--;
            }

            currentHairMale = Mathf.Clamp(currentHairMale, 0, CharacterManager.Instance.hairModelsMale.Count - 1);

            if (CharacterManager.Instance.hairModelsMale[currentHairMale] == "None")
            {
                avatar.ClearSlot("Hair");
            }
            else
            {
                avatar.SetSlot("Hair", CharacterManager.Instance.hairModelsMale[currentHairMale]);
            }

            dataHolder.SetHairType(currentHairMale);
        }

        if (avatar.activeRace.name == "HumanFemaleDCS")
        {
            if (plus)
            {
                currentHairFemale++;
            }
            else
            {
                currentHairFemale--;
            }

            currentHairFemale = Mathf.Clamp(currentHairFemale, 0, CharacterManager.Instance.hairModelsFemale.Count - 1);

            if (CharacterManager.Instance.hairModelsFemale[currentHairFemale] == "None")
            {
                avatar.ClearSlot("Hair");
            }
            else
            {
                avatar.SetSlot("Hair", CharacterManager.Instance.hairModelsFemale[currentHairFemale]);
            }

            dataHolder.SetHairType(currentHairFemale);
        }

        avatar.BuildCharacter();
    }

    public void CameraZoomIn()
    {
        if (avatar.activeRace.name == "HumanMaleDCS")
        {
            StartCoroutine(LerpFromTo(Camera.main.transform.position, new Vector3(8.3f, 1.568f, 19.491f), 1f));
        }
        if (avatar.activeRace.name == "HumanFemaleDCS")
        {
            StartCoroutine(LerpFromTo(Camera.main.transform.position, new Vector3(8.3f, 1.472f, 19.48f), 1f));
        }
    }

    public void CameraZoomOut()
    {
        StartCoroutine(LerpFromTo(Camera.main.transform.position, new Vector3(8.29f, 1.29f, 17.7f), 1f));
    }

    private IEnumerator LerpFromTo(Vector3 pos1, Vector3 pos2, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            Camera.main.transform.position = Vector3.Lerp(pos1, pos2, t / duration);
            yield return 0;
        }
        Camera.main.transform.position = pos2;
    }

    private void OnClickBackButton()
    {
        if (avatar != null)
        {
            Destroy(avatar.gameObject);
        }
        MainManager.Instance.LoadScene(MainManager.CHARACTER_SELECTION_SCENE);
    }

    private IEnumerator ExitToCharacterSelection()
    {
        yield return new WaitForSeconds(1800); // Wait 30 minutes.
        OnClickBackButton();
    }

    private void OnClickCreateButton()
    {
        // Disable buttons.
        DisableButtons();

        // Store creation information.
        string name = charNameField.text;

        // No name entered.
        if (name == "")
        {
            textMessage.text = "Please enter a name.";
            EnableButtons();
            return;
        }

        // Set name
        dataHolder.SetName(name);

        // Request character creation.
        NetworkManager.ChannelSend(new CharacterCreationRequest(dataHolder));

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

                case 4:
                    textMessage.text = "Invalid creation parameters.";
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

    private void DisableButtons()
    {
        textMessage.text = "Waiting for server..."; // Clean any old messages.
        createButton.enabled = false;
        backButton.enabled = false;
        charNameField.enabled = false;
    }

    private void EnableButtons()
    {
        createButton.enabled = true;
        backButton.enabled = true;
        charNameField.enabled = true;
    }
}
