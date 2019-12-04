using System.Collections.Generic;
using TMPro;
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
    public Toggle fullScreenToggle;
    public Slider musicSlider;
    public Slider sfxSlider;

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
    public volatile static bool useChatTimestamps = false;
    public volatile static int chatColorNormalIntValue = 16777215; // Cannot use Util.ColorToInt in packet, so we store value here.
    public volatile static int chatColorMessageIntValue = 16711760; // Cannot use Util.ColorToInt in packet, so we store value here.
    public volatile static int chatColorSystemIntValue = 16739840; // Cannot use Util.ColorToInt in packet, so we store value here.
    // Keybind related.
    public Canvas keybindMenuCanvas;
    private int lastSelectKeyButtonIndex = -1;
    public TextMeshProUGUI keybindMenuMessageText;

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

        // Load rest of configurations.
        SetQuality(configReader.GetInt(QUALITY_VALUE, 2));
        isFullscreenSave = configReader.GetString(FULLSCREEN_VALUE, TRUE_VALUE).Equals(TRUE_VALUE);
        SetFullscreen(isFullscreenSave);
        fullScreenToggle.isOn = isFullscreenSave;

        float musicVolume = configReader.GetFloat(MUSIC_VOLUME_VALUE, 1);
        MasterVolume(musicVolume);
        musicSlider.value = musicVolume;
        float sfxVolume = configReader.GetFloat(SFX_VOLUME_VALUE, 1);
        GameSFX(sfxVolume);
        sfxSlider.value = sfxVolume;
    }

    private void Update()
    {
        if (InputManager.ESCAPE_DOWN && !ConfirmDialog.Instance.confirmDialogActive)
        {
            // If player has a target selected, cancel the target instead.
            if (MainManager.Instance.lastLoadedScene.Equals(MainManager.WORLD_SCENE) && WorldManager.Instance.targetWorldObject != null)
            {
                WorldManager.Instance.SetTarget(null);
                return;
            }
            ToggleOptionsMenu();
        }

        if (lastSelectKeyButtonIndex > -1)
        {
            foreach (KeyCode keycode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keycode) && keycode != KeyCode.Escape)
                {
                    switch (InputManager.SetKeybind(lastSelectKeyButtonIndex, keycode))
                    {
                        case 0: // Key cannot be bound.
                            keybindMenuMessageText.text = "Input cannot be bound. Try another key.";
                            break;

                        case 1: // Key already bound.
                            keybindMenuMessageText.text = "Input already bound. Try another key.";
                            break;

                        case 2: // Success.
                            HideKeybindMenu();
                            // Update player options.
                            NetworkManager.ChannelSend(new PlayerOptionsUpdate());
                            break;
                    }
                }
            }
        }
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
        qualityDropdown.value = qualityIndex;
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        isFullscreenSave = isFullscreen;
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void CheckFullscreen()
    {
        if (isFullscreenSave && !Screen.fullScreen)
        {
            SetFullscreen(true);
        }
        else if (!isFullscreenSave && Screen.fullScreen)
        {
            SetFullscreen(false);
        }
    }

    public void OnButtonLogoutClick()
    {
        ConfirmDialog.Instance.PlayerConfirm("Are you sure you want to logout?", 3);
    }

    public void OnButtonQuitClick()
    {
        ConfirmDialog.Instance.PlayerConfirm("Are you sure you want to quit the game?", 1);
    }

    public void ToggleOptionsMenu()
    {
        if (chatColorPickerCanvas.gameObject.activeSelf)
        {
            HideChatColorPicker();
            return;
        }
        if (keybindMenuCanvas.gameObject.activeSelf)
        {
            HideKeybindMenu();
            return;
        }

        optionsCanvas.enabled = !optionsCanvas.enabled;
        if (!optionsCanvas.enabled)
        {
            MainManager.Instance.isDraggingWindow = false;
        }

        bool isInWorld = MainManager.Instance.lastLoadedScene.Equals(MainManager.WORLD_SCENE);
        controlsButton.gameObject.SetActive(isInWorld);
        chatButton.gameObject.SetActive(isInWorld);
        logoutButton.gameObject.SetActive(isInWorld);
        exitGameButton.gameObject.SetActive(isInWorld);
        if (isInWorld)
        {
            chatColorButtons[0].image.color = Util.IntToColor(chatColorNormalIntValue);
            chatColorButtons[1].image.color = Util.IntToColor(chatColorMessageIntValue);
            chatColorButtons[2].image.color = Util.IntToColor(chatColorSystemIntValue);
            chatUseTimestamps.enabled = optionsCanvas.enabled;
            chatUseTimestamps.isOn = useChatTimestamps;
        }

        SaveConfigValues();
    }

    public void HideChatColorPicker()
    {
        chatColorPickerCanvas.gameObject.SetActive(false);
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
        // Update player options.
        NetworkManager.ChannelSend(new PlayerOptionsUpdate());
    }

    public void HideKeybindMenu()
    {
        keybindMenuCanvas.gameObject.SetActive(false);
        lastSelectKeyButtonIndex = -1;
    }

    public void ShowKeybindMenu(int index)
    {
        lastSelectKeyButtonIndex = index;
        keybindMenuCanvas.gameObject.SetActive(true);
    }

    public void OnButtonCloseClick()
    {
        ToggleOptionsMenu();
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
