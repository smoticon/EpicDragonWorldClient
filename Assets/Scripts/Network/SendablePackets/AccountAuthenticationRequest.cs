/**
* @author Pantelis Andrianakis
*/
public class AccountAuthenticationRequest : SendablePacket
{
    public AccountAuthenticationRequest(string accountName, string password)
    {
        WriteShort(1); // Packet id.
        WriteString(accountName);
        WriteString(SHA256Generator.Calculate(password));
    }
}
