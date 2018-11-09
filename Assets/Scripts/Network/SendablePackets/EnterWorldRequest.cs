/**
 * Author: Pantelis Andrianakis
 * Date: May 18th 2018
 */
public class EnterWorldRequest : SendablePacket
{
    public EnterWorldRequest(string characterName)
    {
        WriteShort(7); // Packet id.
        WriteString(characterName);
    }
}
