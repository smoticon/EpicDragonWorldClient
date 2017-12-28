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
using System.Collections;
using UnityEngine;

/**
 * @author Pantelis Andrianakis
 */
public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager instance;
    public static bool waitingServer = true;

    private void Start()
    {
        // Return if account name is empty.
        if (NetworkManager.accountName == "")
        {
            return;
        }

        // Set instance.
        instance = this;

        // Change music.
        MusicManager.instance.PlayMusic(MusicManager.instance.PlayerSelection);

        // Request info.
        NetworkManager.instance.ChannelSend(new CharacterSelectionInfoRequest());

        // Wait until server sends existing player data.
        while (waitingServer)
        {
            Debug.Log("waiting");
        }

        // Schedule to exit to login screen.
        StartCoroutine(ExitToLoginScreen());
    }

    private IEnumerator ExitToLoginScreen()
    {
        yield return new WaitForSeconds(900); // Wait 15 minutes.
        SceneFader.Fade("LoginScreen", Color.white, 0.5f);
    }
}
