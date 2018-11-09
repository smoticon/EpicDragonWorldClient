using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: June 10th 2018
 */
public class Logout
{
    public static void notify(ReceivablePacket packet)
    {
        // Used for kicked message.
        NetworkManager.instance.kicked = true;
        // Go to login screen.
        SceneFader.Fade("LoginScreen", Color.white, 0.5f);
    }
}
