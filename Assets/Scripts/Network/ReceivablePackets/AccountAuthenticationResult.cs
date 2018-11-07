/**
 * @author Pantelis Andrianakis
 */
public class AccountAuthenticationResult
{
    public static void notify(ReceivablePacket packet)
    {
        AuthenticationManager.instance.status = packet.ReadByte();
    }
}
