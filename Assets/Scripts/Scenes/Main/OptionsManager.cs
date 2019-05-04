using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/**
 * Authors: NightBR
 * Date: April 29th 2019
 */
public class OptionsManager : MonoBehaviour
{
    public static OptionsManager Instance { get; private set; }

    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public AudioMixer musicAudioMixer;
    public AudioMixer sfxAudioMixer;
    public Button closeOptionsButton;
    public Button controlsButton;
    public Button chatButton;
    public Button logoutButton;
    public Button exitGameButton;
    public Toggle chatUseTimestamps;

    private Resolution[] resolutions;

    private readonly static string SETTINGS_FILE_NAME = "Settings.ini";
    private readonly static string RESOLUTION_VALUE = "Resolution";
    private readonly static string QUALITY_VALUE = "Quality";
    private readonly static string FULLSCREEN_VALUE = "Fullscreen";
    private readonly static string MUSIC_VOLUME_VALUE = "MusicVolume";
    private readonly static string SFX_VOLUME_VALUE = "SfxVolume";
    private readonly static string TRUE_VALUE = "True";
    private readonly static string FALSE_VALUE = "False";

    // Storage variables
    private static int resolutionIndexSave;
    private static int qualityIndexSave;
    private static bool isFullscreenSave;
    private static float masterVolumeSave;
    private static float gameSfxSave;
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

    public MusicManager musicManager;
    public Canvas optionsCanvas;

    private void Start()
    {
        Instance = this;

        // Set resolution after fullscreen.
        ConfigReader configReader = new ConfigReader(SETTINGS_FILE_NAME);
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
        resolutionDropdown.value = configReader.GetInt(RESOLUTION_VALUE, currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();
    }

    public void LoadConfigValues()
    {
        ConfigReader configReader = new ConfigReader(SETTINGS_FILE_NAME);
        SetResolution(configReader.GetInt(RESOLUTION_VALUE, resolutionIndexSave));
        SetQuality(configReader.GetInt(QUALITY_VALUE, QualitySettings.GetQualityLevel()));
        if (Screen.fullScreen != configReader.GetString(FULLSCREEN_VALUE, TRUE_VALUE).Equals(TRUE_VALUE))
        {
            SetFullscreen(!Screen.fullScreen);
        }
        MasterVolume(configReader.GetFloat(MUSIC_VOLUME_VALUE, masterVolumeSave));
        GameSFX(configReader.GetFloat(SFX_VOLUME_VALUE, gameSfxSave));
    }

    public void SaveConfigValues()
    {
        ConfigWriter configWriter = new ConfigWriter(SETTINGS_FILE_NAME);
        configWriter.SetInt(RESOLUTION_VALUE, resolutionIndexSave);
        configWriter.SetInt(QUALITY_VALUE, qualityIndexSave);
        configWriter.SetString(FULLSCREEN_VALUE, isFullscreenSave ? TRUE_VALUE : FALSE_VALUE);
        configWriter.SetFloat(MUSIC_VOLUME_VALUE, masterVolumeSave);
        configWriter.SetFloat(SFX_VOLUME_VALUE, gameSfxSave);
    }

    public void SetResolution(int resolutionIndex)
    {
        resolutionIndexSave = resolutionIndex;
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
    }

    public void SetVolume(float mastervolume)
    {
        masterVolumeSave = mastervolume;
        musicAudioMixer.SetFloat(MUSIC_VOLUME_VALUE, mastervolume);
    }

    public void SetQuality(int qualityIndex)
    {
        qualityIndexSave = qualityIndex;
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        isFullscreenSave = isFullscreen;
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void OnButtonLogoutClick()
    {
        ConfirmDialog.Instance.PlayerConfirm("Are you sure you want to logout?", 3);
    }

    public void OnButtonQuitClick()
    {
        ConfirmDialog.Instance.PlayerConfirm("Are you sure you want to quit the game?", 1);
    }

    public void ShowOptionsMenu()
    {
        optionsCanvas.enabled = !optionsCanvas.enabled;
        if (optionsCanvas.enabled)
        {
            // Load Config OptionsMenu values.
            LoadConfigValues();
        }
        else
        {
            // Save Config OptionsMenu Values.
            SaveConfigValues();
        }
    }

    public void HideOptionsMenu()
    {
        optionsCanvas.enabled = false;
        // Save Config OptionsMenu Values.
        SaveConfigValues();
    }

    public void NormalColorButtonSelected()
    {
        lastSelectColorButtonIndex = 0;
        chatColorPickerCanvas.gameObject.SetActive(true);
    }

    public void MessageColorButtonSelected()
    {
        lastSelectColorButtonIndex = 1;
        chatColorPickerCanvas.gameObject.SetActive(true);
    }

    public void SystemColorButtonSelected()
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

    public void OnButtonCloseClick()
    {
        ShowOptionsMenu();
    }

    // Slider Volume Control Section
    public void MasterVolume(float value)
    {
        masterVolumeSave = value;
        musicAudioMixer.SetFloat(MUSIC_VOLUME_VALUE, Mathf.Log10(value) * 20);
    }

    public void GameSFX(float value)
    {
        gameSfxSave = value;
        sfxAudioMixer.SetFloat(SFX_VOLUME_VALUE, Mathf.Log10(value) * 20);
    }

    public float GetSfxVolume()
    {
        return gameSfxSave;
    }

    // Mute Volume Section
    public void MuteMaster()
    {
        AudioListener.pause = !AudioListener.pause;
    }

    public void ClearVolume()
    {
        musicAudioMixer.ClearFloat("mastervolume");
    }
}
