/**
 * @author Pantelis Andrianakis
 */
public class CharacterSelectionInfoRequest : SendablePacket
{
    public CharacterSelectionInfoRequest()
    {
        WriteShort(2); // Packet id.
        WriteString(PlayerManager.instance.accountName);
    }
}
