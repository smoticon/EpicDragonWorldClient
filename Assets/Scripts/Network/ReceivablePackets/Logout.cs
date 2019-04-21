/**
 * Author: Pantelis Andrianakis
 * Date: June 10th 2018
 */
public class Logout
{
    public static void Notify(ReceivablePacket packet)
    {
        // Used for kicked message.
        NetworkManager.forcedDisconnection = true;
        NetworkManager.DisconnectFromServer();
        // Go to login screen.
        // MainManager.Instance.LoadScene(MainManager.LOGIN_SCENE);
        // TODO: Show disconnected message.
        System.Diagnostics.Process.GetCurrentProcess().Kill();
    }
}
