/**
 * @author Pantelis Andrianakis
 */
public class CharacterSlotUpdate : SendablePacket
{
    public CharacterSlotUpdate(int oldSlotId, int newSlotId)
    {
        WriteShort(5); // Packet id.
        WriteByte(oldSlotId);
        WriteByte(newSlotId);
    }
}
