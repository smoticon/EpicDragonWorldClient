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

    public MusicManager musicManager;
    public Canvas optionsCanvas;

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
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
        PlayerPrefs.SetInt(RESOLUTION_VALUE, resolutionIndex);
    }

    public void SetVolume(float MasterVolume)
    {
        audioMixer.SetFloat(MASTER_VOLUME_VALUE, MasterVolume);
        PlayerPrefs.SetFloat(MASTER_VOLUME_VALUE, MasterVolume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt(QUALITY_VALUE, qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = !Screen.fullScreen;
        PlayerPrefs.SetString(FULLSCREEN_VALUE, isFullscreen ? TRUE_VALUE : FALSE_VALUE);
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
        musicManager.PlayUIMusic(MainManager.Instance.buildIndex);
    }

    public void HideOptionsMenu()
    {
        optionsCanvas.enabled = false;
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

    // Slider Volume Control Section
    public void MasterVolume(float MasterVolume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(MasterVolume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", Mathf.Log10(MasterVolume) * 20);
    }

    public void GameMusic(float gamemusic)
    {
        audioMixer.SetFloat("gamemusic", Mathf.Log10(gamemusic) * 20);
        PlayerPrefs.SetFloat("gamemusic", Mathf.Log10(gamemusic) * 20);
    }

    public void GameSFX(float gamesfx)
    {
        audioMixer.SetFloat("gamesfx", Mathf.Log10(gamesfx) * 20);
        PlayerPrefs.SetFloat("gamesfx", Mathf.Log10(gamesfx) * 20);
    }

    public void UiMusic(float uimusic)
    {
        audioMixer.SetFloat("uimusic", Mathf.Log10(uimusic) * 20);
        PlayerPrefs.SetFloat("uimusic", Mathf.Log10(uimusic) * 20);
    }

    public void UiSFX(float uisfx)
    {
        audioMixer.SetFloat("uisfx", Mathf.Log10(uisfx) * 20);
        PlayerPrefs.SetFloat("uisfx", Mathf.Log10(uisfx) * 20);
    }

    public void LoginMusic(float loginmusic)
    {
        audioMixer.SetFloat("loginmusic", Mathf.Log10(loginmusic) * 20);
        PlayerPrefs.SetFloat("loginmusic", Mathf.Log10(loginmusic) * 20);
    }

    public void LoadingMusic(float loadingmusic)
    {
        audioMixer.SetFloat("loadingmusic", Mathf.Log10(loadingmusic) * 20);
        PlayerPrefs.SetFloat("loadingmusic", Mathf.Log10(loadingmusic) * 20);
    }

    public void CharSelectMusic(float charselectmusic)
    {
        audioMixer.SetFloat("charselectmusic", Mathf.Log10(charselectmusic) * 20);
        PlayerPrefs.SetFloat("charselectmusic", Mathf.Log10(charselectmusic) * 20);
    }
    public void CharCreationMusic(float charcreationmusic)
    {
        audioMixer.SetFloat("charcreationmusic", Mathf.Log10(charcreationmusic) * 20);
        PlayerPrefs.SetFloat("charcreationmusic", Mathf.Log10(charcreationmusic) * 20);
    }

    // Mute Volume Section
    public void MuteMaster()
    {
        AudioListener.pause = !AudioListener.pause;
    }

    public void ClearVolume()
    {
        audioMixer.ClearFloat("mastervolume");
    }
}
