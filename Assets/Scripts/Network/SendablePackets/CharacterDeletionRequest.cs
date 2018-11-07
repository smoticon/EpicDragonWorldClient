/**
 * @author Pantelis Andrianakis
 */
public class CharacterDeletionRequest : SendablePacket
{
    public CharacterDeletionRequest(int slot)
    {
        WriteShort(4); // Packet id.
        WriteByte(slot);
    }
}
