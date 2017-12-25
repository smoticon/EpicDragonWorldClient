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

    void Start()
    {
        loginButton.GetComponent<Button>().onClick.AddListener(OnClickTask);
    }

    private void OnClickTask()
    {
        DisableButtons();
        
        // Field text checks.
        if (accountNameField.text == "")
        {
            messageText.text = "Please enter your account name.";
            EnableButtons();
            return;
        }
        if (passwordField.text == "")
        {
            messageText.text = "Please enter your password.";
            EnableButtons();
            return;
        }
        if (accountNameField.text.Length < 2)
        {
            messageText.text = "Account name length is too short.";
            EnableButtons();
            return;
        }
        if (passwordField.text.Length < 2)
        {
            messageText.text = "Password length is too short.";
            EnableButtons();
            return;
        }

        // Try to connect to server.
        if (!NetworkManager.instance.connectToServer())
        {
            messageText.text = "Could not communicate with the server.";
            EnableButtons();
            return;
        }

        // TODO: Authenticate.

        messageText.text = "Connected!";
        // TODO: Go to character selection scene.
    }

    private void DisableButtons()
    {
        loginButton.enabled = false;
        accountNameField.enabled = false;
        passwordField.enabled = false;
        messageText.text = "Connecting..."; // Clean any old messages.
    }

    private void EnableButtons()
    {
        loginButton.enabled = true;
        accountNameField.enabled = true;
        passwordField.enabled = true;
    }
}
