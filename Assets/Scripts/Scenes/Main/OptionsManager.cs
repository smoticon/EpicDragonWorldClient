using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/**
 * Authors: Ilias Vlachos, Pantelis Andrianakis
 * Date: December 23th 2018
 */
public class OptionsManager : MonoBehaviour
{
    public static OptionsManager Instance { get; private set; }

    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public AudioMixer audioMixer;
    public Button closeOptionsButton;
    public Button controlsButton;
    public Button chatButton;
    public Button logoutButton;
    public Button exitGameButton;
    public Toggle chatUseTimestamps;

    private Resolution[] resolutions;

    private readonly string RESOLUTION_VALUE = "Resolution";
    private readonly string MASTER_VOLUME_VALUE = "MasterVolume";
    private readonly string QUALITY_VALUE = "Quality";
    private readonly string FULLSCREEN_VALUE = "Fullscreen";
    private readonly string TRUE_VALUE = "True";
    private readonly string FALSE_VALUE = "False";

    // Chat color related.
    public Button[] chatColorButtons;
    public Canvas chatColorPickerCanvas;
    private int lastSelectColorButtonIndex;
    [HideInInspector]
    public int chatColorNormalIntValue = 16777215; // Cannot use Util.ColorToInt in packet, so we store value here.
    [HideInInspector]
    public int chatColorMessageIntValue = 16711760; // Cannot use Util.ColorToInt in packet, so we store value here.
    [HideInInspector]
    public int chatColorSystemIntValue = 16739840; // Cannot use Util.ColorToInt in packet, so we store value here.
    [HideInInspector]
    public bool useChatTimestamps = false;

    private void Start()
    {
        Instance = this;

        // Load saved values.
        SetVolume(PlayerPrefs.GetFloat(MASTER_VOLUME_VALUE, 1f));
        SetQuality(PlayerPrefs.GetInt(QUALITY_VALUE, QualitySettings.GetQualityLevel()));
        SetFullscreen(PlayerPrefs.GetString(FULLSCREEN_VALUE, TRUE_VALUE).Equals(TRUE_VALUE));

        // Set resolution after fullscreen.
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> resolutionOptions = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " (" + resolutions[i].refreshRate + "Hz)";
            resolutionOptions.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height && resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = PlayerPrefs.GetInt(RESOLUTION_VALUE, currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();

        // Add listener.
        logoutButton.onClick.AddListener(OnButtonLogoutClick);
        exitGameButton.onClick.AddListener(OnButtonQuitClick);
        closeOptionsButton.onClick.AddListener(HideOptionsMenu);
        chatColorButtons[0].onClick.AddListener(NormalColorButtonSelected);
        chatColorButtons[1].onClick.AddListener(MessageColorButtonSelected);
        chatColorButtons[2].onClick.AddListener(SystemColorButtonSelected);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
        PlayerPrefs.SetInt(RESOLUTION_VALUE, resolutionIndex);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat(MASTER_VOLUME_VALUE, volume);
        PlayerPrefs.SetFloat(MASTER_VOLUME_VALUE, volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt(QUALITY_VALUE, qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetString(FULLSCREEN_VALUE, isFullscreen ? TRUE_VALUE : FALSE_VALUE);
    }

    private void OnButtonLogoutClick()
    {
        ConfirmDialog.Instance.PlayerConfirm("Are you sure you want to logout?", 3);
    }

    private void OnButtonQuitClick()
    {
        ConfirmDialog.Instance.PlayerConfirm("Are you sure you want to quit the game?", 1);
    }

    private void HideOptionsMenu()
    {
        MainManager.Instance.optionsCanvas.enabled = false;
    }

    private void NormalColorButtonSelected()
    {
        lastSelectColorButtonIndex = 0;
        chatColorPickerCanvas.gameObject.SetActive(true);
    }

    private void MessageColorButtonSelected()
    {
        lastSelectColorButtonIndex = 1;
        chatColorPickerCanvas.gameObject.SetActive(true);
    }

    private void SystemColorButtonSelected()
    {
        lastSelectColorButtonIndex = 2;
        chatColorPickerCanvas.gameObject.SetActive(true);
    }

    public void ChangeSelectedChatColor(Color color)
    {
        chatColorButtons[lastSelectColorButtonIndex].image.color = color;
        chatColorPickerCanvas.gameObject.SetActive(false);
        switch (lastSelectColorButtonIndex)
        {
            case 0:
                chatColorNormalIntValue = Util.ColorToInt(color);
                break;

            case 1:
                chatColorMessageIntValue = Util.ColorToInt(color);
                break;

            case 2:
                chatColorSystemIntValue = Util.ColorToInt(color);
                break;
        }

        // Update player options.
        NetworkManager.ChannelSend(new PlayerOptionsUpdate());
    }

    public void ResetChatColors()
    {
        lastSelectColorButtonIndex = 0;
        ChangeSelectedChatColor(new Color(255, 255, 255));
        lastSelectColorButtonIndex = 1;
        ChangeSelectedChatColor(new Color(255, 0, 80));
        lastSelectColorButtonIndex = 2;
        ChangeSelectedChatColor(new Color(255, 110, 0));

        useChatTimestamps = false;
    }

    public void ToggleTimestampUse()
    {
        useChatTimestamps = !useChatTimestamps;
        chatUseTimestamps.enabled = useChatTimestamps;
    }
}
