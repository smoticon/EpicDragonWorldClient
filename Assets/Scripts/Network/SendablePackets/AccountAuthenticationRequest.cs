/**
* @author Pantelis Andrianakis
*/
public class AccountAuthenticationRequest : SendablePacket
{
    public AccountAuthenticationRequest(double clientVersion, string accountName, string password)
    {
        WriteShort(1); // Packet id.
        WriteDouble(clientVersion);
        WriteString(accountName);
        WriteString(SHA256Generator.Calculate(password));
    }
}
