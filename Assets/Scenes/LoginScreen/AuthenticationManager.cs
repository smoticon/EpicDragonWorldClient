using UnityEngine;
using UnityEngine.UI;

/**
 * @author Pantelis Andrianakis
 */
public class AuthenticationManager : MonoBehaviour
{
    public Button loginButton;
    public InputField accountNameField;
    public InputField passwordField;
    public Text messageText;

    public static AuthenticationManager instance;
    public int status;
    private bool authenticating;

    private static readonly double clientVersion = 1.0;

    private void Start()
    {
        instance = this;
        NetworkManager.instance.DisconnectFromServer(); // In case player exits to login screen.
        loginButton.GetComponent<Button>().onClick.AddListener(OnClickTask);

        // In case player was kicked from the game.
        if (NetworkManager.instance.kicked)
        {
            messageText.text = "You have been kicked from the game.";
            NetworkManager.instance.kicked = false;
        }
    }

    private void OnClickTask()
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
        if (!NetworkManager.instance.ConnectToServer())
        {
            messageText.text = "Could not communicate with the server.";
            EnableButtons();
            return;
        }

        // Authenticate.
        messageText.text = "Authenticating...";
        status = -1;
        NetworkManager.instance.ChannelSend(new AccountAuthenticationRequest(clientVersion, account, password));

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
            PlayerManager.instance.accountName = account;
            SceneFader.Fade("CharacterSelection", Color.white, 0.5f);
        }
        else // Enable buttons.
        {
            NetworkManager.instance.DisconnectFromServer();
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
}
