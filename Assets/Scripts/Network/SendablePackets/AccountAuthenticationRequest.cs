/**
 * Author: Pantelis Andrianakis
 * Date: December 26th 2017
 */
public class AccountAuthenticationRequest : SendablePacket
{
    public AccountAuthenticationRequest(string accountName, string password)
    {
        WriteShort(1); // Packet id.
        WriteDouble(VersionConfigurations.CLIENT_VERSION);
        WriteString(accountName);
        WriteString(SHA256Generator.Calculate(password));
    }
}
