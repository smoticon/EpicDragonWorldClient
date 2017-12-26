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
public class ButtonLogin : MonoBehaviour
{
    public Button loginButton;
    public InputField accountNameField;
    public InputField passwordField;
    public Text messageText;

    public static ButtonLogin instance;
    public static int status = -1000;
    private bool authenticating = false;

    private void Start()
    {
        instance = this;
        loginButton.GetComponent<Button>().onClick.AddListener(OnClickTask);
    }

    private void OnClickTask()
    {
        DisableButtons();
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
        NetworkManager.instance.ChannelSend(new AccountAuthenticationRequest(account, password));

        // Wait for result.
        authenticating = true;
        while (authenticating)
        {
            switch (status)
            {
                case 0:
                    messageText.text = "Account does not exist.";
                    NetworkManager.instance.DisconnectFromServer();
                    authenticating = false;
                    status = -1000;
                    EnableButtons();
                    break;

                case 1:
                    messageText.text = "Account is banned.";
                    NetworkManager.instance.DisconnectFromServer();
                    authenticating = false;
                    status = -1000;
                    EnableButtons();
                    break;

                case 2:
                    messageText.text = "Account requires activation.";
                    NetworkManager.instance.DisconnectFromServer();
                    authenticating = false;
                    status = -1000;
                    EnableButtons();
                    break;

                case 3:
                    messageText.text = "Wrong password.";
                    NetworkManager.instance.DisconnectFromServer();
                    authenticating = false;
                    status = -1000;
                    EnableButtons();
                    break;

                case 4:
                    messageText.text = "Authenticated.";
                    authenticating = false;
                    break;
            }
        }

        // Go to character selection screen.
        if (status == 4)
        {
            SceneFader.Fade("CharacterSelection", Color.white, 0.5f);
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
