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

    private void Start()
    {
        instance = this;
        NetworkManager.instance.DisconnectFromServer(); // In case player exits to login screen.
        loginButton.GetComponent<Button>().onClick.AddListener(OnClickTask);
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
        NetworkManager.instance.ChannelSend(new AccountAuthenticationRequest(account, password));

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
                    messageText.text = "Too many online, please try again later.";
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
