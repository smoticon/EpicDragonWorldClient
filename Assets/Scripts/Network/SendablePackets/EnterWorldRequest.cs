/**
* @author Pantelis Andrianakis
*/
public class EnterWorldRequest : SendablePacket
{
    public EnterWorldRequest(string characterName)
    {
        WriteShort(7); // Packet id.
        WriteString(characterName);
    }
}
