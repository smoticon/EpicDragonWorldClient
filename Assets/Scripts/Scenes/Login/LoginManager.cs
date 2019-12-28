using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * Author: Pantelis Andrianakis
 * Date: December 22th 2018
 */
public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance { get; private set; }

    public Button loginButton;
    public Button quitButton;
    public Button optionsButton;
    public TMP_InputField accountNameField;
    public TMP_InputField passwordField;
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI versionText;

    [HideInInspector]
    public int status;
    private bool authenticating;

    private void Start()
    {
        // In case player logouts underwater.
        RenderSettings.fogColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        RenderSettings.fogDensity = 0.01f;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogStartDistance = 500;
        RenderSettings.fogEndDistance = 1200;

        // Restore Camera Position
        Camera.main.transform.position = new Vector3(0f, 1f, 0.95f);

        // Make sure options manager has set fullscreen.
        OptionsManager.Instance.CheckFullscreen();

        // If player exits to login screen, authentication must be repeated.
        NetworkManager.DisconnectFromServer();
        // In case player was forced kicked from the game.
        if (NetworkManager.forcedDisconnection)
        {
            messageText.text = "You have been kicked from the game.";
            NetworkManager.forcedDisconnection = false;
        }
        // In case connection was lost.
        if (NetworkManager.unexpectedDisconnection)
        {
            messageText.text = "Could not communicate with the server.";
            NetworkManager.unexpectedDisconnection = false;
        }

        // Set instance.
        if (Instance != null)
        {
            return;
        }
        Instance = this;

        // Display version text.
        versionText.text = "Version " + (VersionConfigurations.CLIENT_VERSION % 1 == 0 ? VersionConfigurations.CLIENT_VERSION + ".0" : VersionConfigurations.CLIENT_VERSION.ToString());

        // Button listeners.
        loginButton.onClick.AddListener(OnButtonLoginClick);
        optionsButton.onClick.AddListener(OnButtonOptionsClick);
        quitButton.onClick.AddListener(OnButtonQuitClick);

        // One time opperations.
        if (!MainManager.Instance.hasInitialized)
        {
            // In case game started with command line arguments.
            // Example: EpicDragonWorld -account peter -password 12345
            string account = CommandLineArguments.Get("-account");
            if (account != null)
            {
                accountNameField.text = account;
            }
            string password = CommandLineArguments.Get("-password");
            if (password != null)
            {
                passwordField.text = password;
            }
            // Attempt to auto connect when possible.
            if (account != null && password != null)
            {
                OnButtonLoginClick();
            }
        }

        // At this point client has initialized.
        MainManager.Instance.hasInitialized = true;
    }

    private void OnButtonLoginClick()
    {
        // Disable buttons.
        DisableButtons();

        // Store login information.
        string account = accountNameField.text;
        string password = passwordField.text;

        // Input field checks.
        if (account == "")
        {
            messageText.text = "Please enter your account name.";
            EnableButtons();
            return;
        }
        if (password == "")
        {
            messageText.text = "Please enter your password.";
            EnableButtons();
            return;
        }
        if (account.Length < 2)
        {
            messageText.text = "Account name length is too short.";
            EnableButtons();
            return;
        }
        if (password.Length < 2)
        {
            messageText.text = "Password length is too short.";
            EnableButtons();
            return;
        }

        // Try to connect to server.
        if (!NetworkManager.ConnectToServer())
        {
            messageText.text = "Could not communicate with the server.";
            EnableButtons();
            return;
        }

        // Authenticate.
        messageText.text = "Authenticating...";
        status = -1;
        NetworkManager.ChannelSend(new AccountAuthenticationRequest(account, password));

        // Wait for result.
        authenticating = true;
        while (authenticating)
        {
            switch (status)
            {
                case 0:
                    messageText.text = "Account does not exist.";
                    authenticating = false;
                    break;

                case 1:
                    messageText.text = "Account is banned.";
                    authenticating = false;
                    break;

                case 2:
                    messageText.text = "Account requires activation.";
                    authenticating = false;
                    break;

                case 3:
                    messageText.text = "Wrong password.";
                    authenticating = false;
                    break;

                case 4:
                    messageText.text = "Account is already connected.";
                    authenticating = false;
                    break;

                case 5:
                    messageText.text = "Too many online players, please try again later.";
                    authenticating = false;
                    break;

                case 6:
                    messageText.text = "Incorrect client version.";
                    authenticating = false;
                    break;

                case 7:
                    messageText.text = "Server is not available.";
                    authenticating = false;
                    break;

                case 100:
                    messageText.text = "Authenticated.";
                    authenticating = false;
                    break;
            }
        }

        // Go to player selection screen.
        if (status == 100)
        {
            MainManager.Instance.accountName = account;
            MainManager.Instance.LoadScene(MainManager.CHARACTER_SELECTION_SCENE);
        }
        else // Enable buttons.
        {
            NetworkManager.DisconnectFromServer();
            EnableButtons();
        }
    }

    private void DisableButtons()
    {
        messageText.text = "Connecting..."; // Clean any old messages.
        loginButton.enabled = false;
        accountNameField.enabled = false;
        passwordField.enabled = false;
    }

    private void EnableButtons()
    {
        loginButton.enabled = true;
        accountNameField.enabled = true;
        passwordField.enabled = true;
    }

    private void OnButtonOptionsClick()
    {
        OptionsManager.Instance.ToggleOptionsMenu();
    }

    private void OnButtonQuitClick()
    {
        ConfirmDialog.Instance.PlayerConfirm("Are you sure you want to quit the game?", 1);
    }
}
