/**
 * Author: Pantelis Andrianakis
 * Date: December 26th 2017
 */
public class AccountAuthenticationResult
{
    public static void notify(ReceivablePacket packet)
    {
        AuthenticationManager.instance.status = packet.ReadByte();
    }
}
